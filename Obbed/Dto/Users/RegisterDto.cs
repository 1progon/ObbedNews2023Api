using System.ComponentModel.DataAnnotations;

namespace Obbed.Dto.Users;

public class RegisterDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] [MinLength(5)] public string Password { get; set; } = null!;

    [Required] [MinLength(5)] public string PasswordConfirm { get; set; } = null!;
}