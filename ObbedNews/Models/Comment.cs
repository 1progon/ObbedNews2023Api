using System.ComponentModel.DataAnnotations;
using ObbedNews.Enums.News.Comments;

namespace ObbedNews.Models;

public class Comment
{
    [Key] public long Id { get; set; }

    public string? Title { get; set; }
    [Required] public string Message { get; set; } = null!;

    public long Likes { get; set; }
    public long Dislikes { get; set; }

    [Required] public News News { get; set; } = null!;
    [Required] public long NewsId { get; set; }


    public Comment? ParentComment { get; set; }
    public long? ParentCommentId { get; set; }

    public IList<Comment> ChildComments { get; set; } = new List<Comment>();

    [Required] public User User { get; set; } = null!;
    [Required] public long UserId { get; set; }

    [Required] public CommentStatus Status { get; set; }
}