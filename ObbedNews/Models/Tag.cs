namespace ObbedNews.Models;

public class Tag : BaseModel
{
    public IList<News>? NewsList { get; set; }
}