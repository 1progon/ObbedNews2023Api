namespace ObbedNews.Dto.Categories;

public class AddCategoryDto : BaseModelDto
{
    public long? ParentCategoryId { get; set; }
}