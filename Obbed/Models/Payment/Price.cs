using Microsoft.EntityFrameworkCore;
using Obbed.Enums.Payments;

namespace Obbed.Models.Payment;

[PrimaryKey(nameof(System), nameof(Plan), nameof(CurrencyId))]
public class Price
{
    public decimal Sum { get; set; }
    public PaymentSystem System { get; set; }
    public PaymentPlan Plan { get; set; }

    public Currency Currency { get; set; } = null!;
    public int CurrencyId { get; set; }
}