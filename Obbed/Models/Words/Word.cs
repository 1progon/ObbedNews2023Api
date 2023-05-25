using System.ComponentModel.DataAnnotations.Schema;
using Obbed.Models.Middle;
using Obbed.Models.Words.Dictionary;

namespace Obbed.Models.Words;

public class Word : BaseModelWithDates
{
    private long _likesRate;

    public override long Id { get; set; }

    public bool IsDraft { get; set; }

    public string? MainThumb { get; set; }
    public string? MainImage { get; set; }

    public string? Description { get; set; }
    public string? Article { get; set; }

    public string? NewsLink { get; set; }

    public long DislikesCount { get; set; }
    public long LikesCount { get; set; }

    public bool Popular { get; set; }

    public long LikesRate
    {
        get => LikesCount - DislikesCount;
        set => _likesRate = value;
    }

    public Category Category { get; set; } = null!;
    public long CategoryId { get; set; }

    public IList<WordComment> Comments { get; set; } = new List<WordComment>();
    public IList<Tag>? Tags { get; set; }

    public IList<UserWordLike> UserWordLikes { get; set; } = new List<UserWordLike>();

    public IList<UserWordFavorite> UserWordFavorites { get; set; } = new List<UserWordFavorite>();

    public IList<WordVideoSection> VideoSections { get; set; } = new List<WordVideoSection>();

    public bool IsFree { get; set; }

    [Column(TypeName = "jsonb")] public WordSection? WordSection { get; set; }
}