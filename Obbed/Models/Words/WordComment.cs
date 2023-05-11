using System.ComponentModel.DataAnnotations;
using Obbed.Enums.News.Comments;

namespace Obbed.Models.Words;

public class WordComment
{
    [Key] public long Id { get; set; }

    public string? Title { get; set; }
    [Required] public string Message { get; set; } = null!;

    public long Likes { get; set; }
    public long Dislikes { get; set; }

    [Required] public Word Word { get; set; } = null!;
    [Required] public long WordId { get; set; }


    public WordComment? ParentComment { get; set; }
    public long? ParentCommentId { get; set; }

    public IList<WordComment> ChildComments { get; set; } = new List<WordComment>();

    [Required] public User User { get; set; } = null!;
    [Required] public long UserId { get; set; }

    [Required] public CommentStatus Status { get; set; }
}