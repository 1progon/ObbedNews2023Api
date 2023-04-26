using Microsoft.EntityFrameworkCore;

namespace ObbedNews.Models.Middle;

[PrimaryKey(nameof(UserId), nameof(NewsId))]
public class UserNewsFavorite
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public News News { get; set; } = null!;
    public long NewsId { get; set; }
}