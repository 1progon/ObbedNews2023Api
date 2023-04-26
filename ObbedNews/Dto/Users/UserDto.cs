using ObbedNews.Enums.Users;

namespace ObbedNews.Dto.Users;

public class UserDto
{
    public Guid Guid { get; set; }
    public string Email { get; set; } = null!;
    public UserType UserType { get; set; }
    public string Token { get; set; } = null!;
    public bool HasPremium { get; set; }
}