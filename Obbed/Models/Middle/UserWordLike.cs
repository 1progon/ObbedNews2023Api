using Microsoft.EntityFrameworkCore;
using Obbed.Enums.News;
using Obbed.Models.Words;

namespace Obbed.Models.Middle;

[PrimaryKey(nameof(WordId), nameof(UserId))]
public class UserWordLike
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public Word Word { get; set; } = null!;
    public long WordId { get; set; }

    public LikeType LikeType { get; set; }
}