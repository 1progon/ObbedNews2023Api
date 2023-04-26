using ObbedNews.Enums.News;

namespace ObbedNews.Dto.News;

public class PostUpdateLikeDto
{
    public LikeType Type { get; set; }
    public long NewsId { get; set; }
}