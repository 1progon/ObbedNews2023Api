using System.ComponentModel.DataAnnotations;

namespace ObbedNews.Dto.Users;

public class LoginDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] [MinLength(5)] public string Password { get; set; } = null!;
}