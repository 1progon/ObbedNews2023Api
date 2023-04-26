using ObbedNews.Models.Payment;

namespace ObbedNews.Dto.Accounts;

public class AccountPremiumInitialDto
{
    public IList<Price>? Prices { get; set; }
    public IList<PayPalOrder>? PayPalOrders { get; set; }
}