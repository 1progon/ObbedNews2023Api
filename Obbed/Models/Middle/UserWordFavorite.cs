using Microsoft.EntityFrameworkCore;
using Obbed.Models.Words;

namespace Obbed.Models.Middle;

[PrimaryKey(nameof(UserId), nameof(WordId))]
public class UserWordFavorite
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public Word Word { get; set; } = null!;
    public long WordId { get; set; }
}