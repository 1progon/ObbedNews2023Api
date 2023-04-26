using System.ComponentModel.DataAnnotations;
using ObbedNews.Models.Middle;
using ObbedNews.Models.Payment;

namespace ObbedNews.Models;

public class User
{
    [Key] public long Id { get; set; }

    [Required] public Account Account { get; set; } = null!;
    [Required] public long AccountId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required] public string Login { get; set; } = null!;

    public string? Avatar { get; set; }

    public IList<UserNewsFavorite> UserNewsFavorites { get; set; } = new List<UserNewsFavorite>();

    public IList<PayPalOrder> PayPalOrders { get; set; } = new List<PayPalOrder>();
}