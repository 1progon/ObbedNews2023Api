using ObbedNews.Enums.News;
using ObbedNews.Models.Middle;

namespace ObbedNews.Models;

public class News : BaseModelWithDates
{
    private long _likesRate;

    public override long Id { get; set; }

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

    public IList<Comment> Comments { get; set; } = new List<Comment>();
    public IList<Tag>? Tags { get; set; }

    public IList<UserNewsLike> UserNewsLikes { get; set; } = new List<UserNewsLike>();

    public IList<UserNewsFavorite> UserNewsFavorites { get; set; } = new List<UserNewsFavorite>();

    public IList<NewsVideoSection> VideoSections { get; set; } = new List<NewsVideoSection>();

    public NewsLevel Level { get; set; }

    public bool IsFree { get; set; }

}