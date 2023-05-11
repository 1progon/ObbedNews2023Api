using Obbed.Enums.News;

namespace Obbed.Dto.Words;

public class PostUpdateLikeDto
{
    public LikeType Type { get; set; }
    public long WordId { get; set; }
}