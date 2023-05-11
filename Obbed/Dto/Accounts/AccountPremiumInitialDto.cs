using Obbed.Models.Payment;

namespace Obbed.Dto.Accounts;

public class AccountPremiumInitialDto
{
    public IList<Price>? Prices { get; set; }
    public IList<PayPalOrder>? PayPalOrders { get; set; }
}