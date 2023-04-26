using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ObbedNews.Data;
using ObbedNews.Dto.Accounts;
using ObbedNews.Dto.Payments.PayPal;
using ObbedNews.Enums.Payments;
using ObbedNews.Enums.Payments.PayPal;
using ObbedNews.Models.Payment;

namespace ObbedNews.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private const string PaypalTokenKey = "paypalToken";
    private readonly IMemoryCache _cache;
    private readonly HttpClient _client;
    private readonly IConfiguration _conf;
    private readonly AppDbContext _context;

    private readonly PaymentConfig _paypalConfig;

    private string? _payPalToken;

    public PaymentController(AppDbContext context,
        IMemoryCache cache,
        IConfiguration conf)
    {
        _context = context;
        _cache = cache;
        _conf = conf;


        _paypalConfig = new PaymentConfig
        {
            System = PaymentSystem.PayPal,
            Account = _conf.GetSection("PayPal")["Account"] ?? "",
            ClientId = _conf.GetSection("PayPal")["ClientId"] ?? "",
            ClientSecret = _conf.GetSection("PayPal")["ClientSecret"] ?? "",
            Url = _conf.GetSection("PayPal")["Url"]
        };

        _client = new HttpClient
        {
            BaseAddress = new Uri(_paypalConfig.Url ?? "")
        };
    }


    private async Task<string> _getToken()
    {
        var payPalTokenDto = _cache.Get<GetPayPalTokenDto>(PaypalTokenKey);
        if (payPalTokenDto is not null && DateTime.Now < payPalTokenDto.ExpiresAt)
            return _payPalToken = payPalTokenDto.AccessToken;

        _cache.Remove(PaypalTokenKey);


        var authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(_paypalConfig.ClientId + ":" + _paypalConfig.ClientSecret))
        );

        _client.DefaultRequestHeaders.Authorization = authorization;
        var response = await _client.PostAsync("/v1/oauth2/token",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                }
            )
        );


        var d = await response.Content.ReadFromJsonAsync<GetPayPalTokenDto>();
        if (d is null) return string.Empty;
        d.ExpiresAt = DateTime.Now.AddSeconds(d.ExpiresIn);

        _cache.Set(PaypalTokenKey, d, TimeSpan.FromMinutes(3));
        return _payPalToken = d.AccessToken;
    }

    // POST: api/Payment/GetInitialData
    [HttpGet("GetInitialData")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<AccountPremiumInitialDto>> GetInitialData()
    {
        var prices = await _context.Prices.ToListAsync();

        Guid.TryParse(User.Identity?.Name, out var guid);

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Account.Guid == guid);

        if (user is null) return Unauthorized();

        var orders = await _context.PayPalOrders
            .OrderBy(o => o.CreatedAt)
            .Where(o => o.UserId == user.Id && o.Status == OrderStatus.Active)
            .ToListAsync();

        orders = orders.Where(o => o.IsActive).ToList();


        return new AccountPremiumInitialDto
        {
            Prices = prices,
            PayPalOrders = orders
        };
    }

    // POST: api/Payment/CreateOrder
    [HttpPost("CreateOrder")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<PayPalCreateOrderResponseDto>> CreateOrder(
        [FromBody] CreateOrderDto dto)
    {
        if (User.Identity?.Name is null) return Unauthorized();

        // create order
        // todo hardcoded price discount currency data
        // todo check discount and prices from db
        var paypalRequestId = Guid.NewGuid().ToString();

        _client.DefaultRequestHeaders.Add("PayPal-Request-Id", paypalRequestId);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken());
        var res = await _client.PostAsync("/v2/checkout/orders",
            JsonContent.Create(
                new
                {
                    intent = "CAPTURE",
                    purchase_units = new object[]
                    {
                        new
                        {
                            amount = new
                            {
                                // todo hardcoded discount not from db
                                value = Math.Round(
                                    Convert.ToDouble(dto.Plan) * 1.99 * ((100 - dto.Discount) / 100), 2),
                                currency_code = "USD"
                            },
                            description = "Plan: " + (int)dto.Plan
                        }
                    },
                    payment_source = new
                    {
                        paypal = new
                        {
                            experience_context = new
                            {
                                // todo hardcoded site name for paypal
                                brand_name = "News",
                                return_url = _conf.GetValue<string>("Origin") + "/account/check-payment",
                                shipping_preference = "NO_SHIPPING",
                                user_action = "PAY_NOW"
                            }
                        }
                    },
                    plan = dto.Plan,
                    user = User.Identity.Name
                },
                new object().GetType(),
                new MediaTypeHeaderValue("application/json")
            )
        );


        if (!res.IsSuccessStatusCode) return BadRequest();

        var createOrderResponse = await res.Content.ReadFromJsonAsync<PayPalCreateOrderResponseDto>();
        if (createOrderResponse is null) return BadRequest();

        // save to db
        Guid.TryParse(User.Identity?.Name, out var guid);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Account.Guid == guid);
        if (user is null) return Unauthorized();


        // todo add price sum order
        var order = await _context.PayPalOrders
            .SingleOrDefaultAsync(o => o.PayPalRequestId == paypalRequestId);
        if (order is null)
        {
            order = new PayPalOrder
            {
                UserId = user.Id,
                PaymentPlan = dto.Plan,
                PaymentSystem = dto.System,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths((int)dto.Plan),
                Status = OrderStatus.NotPaid,
                PayPalOrderId = createOrderResponse.Id,
                PayPalRequestId = paypalRequestId,
                PayPalOrderStatus =
                    Enum.Parse<PayPalStatuses>(createOrderResponse.Status?.Replace("_", "") ?? string.Empty,
                        true)
            };

            _context.Entry(order).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        createOrderResponse.PayPalOrder = order;

        return createOrderResponse;
    }


    // POST: api/Payment/CaptureOrder
    [HttpPost("CaptureOrder")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<PayPalCreateOrderResponseDto>> CaptureOrder(
        [FromBody] CaptureOrderDto dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _getToken());
        var path = $"/v2/checkout/orders/{dto.Token}/capture";
        var res = await _client.PostAsJsonAsync(path,
            new StringContent(string.Empty, null, "application/json"));

        if (!res.IsSuccessStatusCode) return BadRequest();

        var orderDto = await res.Content.ReadFromJsonAsync<PayPalCreateOrderResponseDto>();
        if (orderDto is null) return BadRequest();

        // todo hardcoded payment provider
        var order = await _context.PayPalOrders.SingleOrDefaultAsync(p => p.PayPalOrderId == orderDto.Id);
        if (order is null) return NotFound();

        // update fields in db
        order.PayPalOrderStatus =
            Enum.Parse<PayPalStatuses>(orderDto.Status?.Replace("_", "") ?? string.Empty, true);
        order.Status = OrderStatus.Active;
        order.UpdatedAt = DateTime.UtcNow;

        // save db
        await _context.SaveChangesAsync();

        orderDto.PayPalOrder = order;

        return orderDto;
    }
}