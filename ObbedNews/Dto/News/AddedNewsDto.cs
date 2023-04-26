using ObbedNews.Enums.News.Comments;

namespace ObbedNews.Dto.News;

public class AddedNewsDto
{
    public long CommentId { get; set; }
    public CommentStatus Status { get; set; }

    public string Message { get; set; } = null!;
    public string? Title { get; set; }

    public long? ParentCommentId { get; set; }
}


