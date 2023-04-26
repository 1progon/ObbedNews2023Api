using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ObbedNews.Data;
using ObbedNews.Dto.Users;
using ObbedNews.Enums.Payments;
using ObbedNews.Enums.Users;
using ObbedNews.Models;

namespace ObbedNews.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string _generateToken(Account account)
        {
            var now = DateTime.UtcNow;

            var k = _configuration.GetSection("Jwt")["Key"];
            if (k is null) return string.Empty;

            var key = Encoding.UTF8.GetBytes(k);

            var claims = new List<Claim>()
            {
                new(ClaimsIdentity.DefaultNameClaimType, account.Guid.ToString()),
                new(ClaimsIdentity.DefaultRoleClaimType, account.UserType.ToString()),
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt")["Issuer"],
                audience: _configuration.GetSection("Jwt")["Audience"],
                claims: claims,
                notBefore: now,
                expires: now.AddHours(8),
                signingCredentials: signingCredentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET: api/Auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Account)
                .Include(u => u.PayPalOrders
                    .Where(o =>
                        o.Status == OrderStatus.Active
                        && o.ExpiresAt > DateTime.UtcNow)
                )
                .SingleOrDefaultAsync(u => u.Account.Email == dto.Email);

            if (user is null) return NotFound();

            var account = user.Account;

            var hasher = new PasswordHasher<Account>();
            var isPasswordGood = hasher
                .VerifyHashedPassword(account, account.Password, dto.Password);

            if (isPasswordGood == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            return new UserDto
            {
                Guid = user.Account.Guid,
                Email = user.Account.Email,
                UserType = user.Account.UserType,
                Token = _generateToken(user.Account),
                HasPremium = user.PayPalOrders?.Count() > 0
            };
        }


        // GET: api/Auth/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
        {
            var userFromDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Account.Email == dto.Email);

            if (userFromDb is not null) return Conflict();

            if (dto.Password != dto.PasswordConfirm || dto.Password.Length < 5)
            {
                return BadRequest();
            }

            var hasher = new PasswordHasher<Account>();


            var account = new Account
            {
                Guid = Guid.NewGuid(),
                Email = dto.Email,
                UserType = UserType.User,
                User = new User
                {
                    Login = dto.Email.Split('@')[0]
                }
            };

            await _context.Accounts.AddAsync(account);

            account.Password = hasher.HashPassword(account, dto.Password);

            await _context.SaveChangesAsync();


            return new UserDto
            {
                Guid = account.Guid,
                Email = account.Email,
                UserType = account.UserType,
                Token = _generateToken(account)
            };
        }
    }
}