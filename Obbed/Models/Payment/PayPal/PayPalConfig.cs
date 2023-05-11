namespace Obbed.Models.Payment.PayPal;

public class PayPalConfig
{
    public PaymentConfig Sandbox { get; set; } = null!;
    public PaymentConfig Production { get; set; } = null!;
}