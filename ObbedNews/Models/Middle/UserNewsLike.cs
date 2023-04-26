using Microsoft.EntityFrameworkCore;
using ObbedNews.Enums.News;

namespace ObbedNews.Models.Middle;

[PrimaryKey(nameof(NewsId), nameof(UserId))]
public class UserNewsLike
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public News News { get; set; } = null!;
    public long NewsId { get; set; }

    public LikeType LikeType { get; set; }
}