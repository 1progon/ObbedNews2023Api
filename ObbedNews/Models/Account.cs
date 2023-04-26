using System.ComponentModel.DataAnnotations;
using ObbedNews.Enums.Users;

namespace ObbedNews.Models;

public class Account
{
    [Key] public long Id { get; set; }
    [Required] public Guid Guid { get; set; }

    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;

    public string? Token { get; set; }
    public DateTime? TokenExpire { get; set; }

    public User User { get; set; } = null!;

    public UserType UserType { get; set; }
}