using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Obbed.Enums.Payments;
using Obbed.Enums.Payments.PayPal;

namespace Obbed.Models.Payment;

// todo need abstract order extends
public class PayPalOrder
{
    [Key] public long Id { get; set; }

    [Required] public User User { get; set; } = null!;
    [Required] public long UserId { get; set; }

    [Required] public PaymentPlan PaymentPlan { get; set; }
    [Required] public PaymentSystem PaymentSystem { get; set; }

    [Required] public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }

    [Required] public DateTime ExpiresAt { get; set; }

    [NotMapped] public bool IsActive => DateTime.Now < ExpiresAt;

    [Required] public OrderStatus Status { get; set; }

    [Required] public string PayPalOrderId { get; set; } = null!;
    [Required] public PayPalStatuses PayPalOrderStatus { get; set; }
    public string? PayPalRequestId { get; set; }
}