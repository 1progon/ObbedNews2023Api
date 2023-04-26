using ObbedNews.Models;

namespace ObbedNews.Dto.News;

public class AddNewsDtoInitial
{
    public IList<Category> Categories { get; set; } = null!;
}