using Obbed.Enums.Payments;

namespace Obbed.Dto.Payments.PayPal;

public class CreateOrderDto
{
    public PaymentSystem System { get; set; }
    public PaymentPlan Plan { get; set; }
    public double Discount { get; set; }
}