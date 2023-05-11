namespace Obbed.Dto.Words;

public class UpdatedLike
{
    public long DislikesCount { get; set; }
    public long LikesCount { get; set; }
    public long Rate { get; set; }
    public bool RemoveVote { get; set; }
}