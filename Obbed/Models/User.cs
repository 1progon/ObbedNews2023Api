using System.ComponentModel.DataAnnotations;
using Obbed.Models.Middle;
using Obbed.Models.Payment;

namespace Obbed.Models;

public class User
{
    [Key] public long Id { get; set; }

    [Required] public Account Account { get; set; } = null!;
    [Required] public long AccountId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required] public string Login { get; set; } = null!;

    public string? Avatar { get; set; }

    public IList<UserWordFavorite> UserNewsFavorites { get; set; } = new List<UserWordFavorite>();

    public IList<PayPalOrder> PayPalOrders { get; set; } = new List<PayPalOrder>();
}