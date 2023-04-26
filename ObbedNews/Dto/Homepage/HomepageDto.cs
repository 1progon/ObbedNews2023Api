namespace ObbedNews.Dto.Homepage;

public class HomepageDto
{
    public IList<Models.News>? Popular { get; set; }
    public IList<Models.News>? MostCommented { get; set; }
    public IList<Models.News>? Last { get; set; }
}