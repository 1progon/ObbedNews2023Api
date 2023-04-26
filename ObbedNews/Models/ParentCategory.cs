namespace ObbedNews.Models;

public class ParentCategory : BaseModel
{
    public IList<Category> Categories { get; set; } = new List<Category>();
}