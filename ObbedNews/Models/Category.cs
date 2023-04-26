namespace ObbedNews.Models;

public class Category : BaseModel
{
    public ParentCategory? ParentCategory { get; set; }
    public long? ParentCategoryId { get; set; }

    public IList<News> NewsList { get; set; } = new List<News>();
}