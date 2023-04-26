namespace ObbedNews.Dto.News;

public class AddCommentDto
{
    public string? Title { get; set; }
    public string Message { get; set; } = null!;

    public long NewsId { get; set; }

    public long? ParentCommentId { get; set; }
}