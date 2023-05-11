using Obbed.Enums.News.Comments;

namespace Obbed.Dto.Words;

public class AddedWordDto
{
    public long CommentId { get; set; }
    public CommentStatus Status { get; set; }

    public string Message { get; set; } = null!;
    public string? Title { get; set; }

    public long? ParentCommentId { get; set; }
}