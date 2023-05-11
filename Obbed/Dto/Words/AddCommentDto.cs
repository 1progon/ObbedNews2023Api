namespace Obbed.Dto.Words;

public class AddCommentDto
{
    public string? Title { get; set; }
    public string Message { get; set; } = null!;

    public long WordId { get; set; }

    public long? ParentCommentId { get; set; }
}