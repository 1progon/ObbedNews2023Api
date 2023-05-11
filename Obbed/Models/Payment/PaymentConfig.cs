using System.ComponentModel.DataAnnotations.Schema;
using Obbed.Enums.Payments;

namespace Obbed.Models.Payment;

[NotMapped]
public class PaymentConfig
{
    public PaymentSystem System { get; set; }
    public string Account { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public bool Production { get; set; }
    public string? Url { get; set; }
}