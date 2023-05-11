namespace Obbed.Dto.Categories;

public class AddCategoryDto : BaseModelDto
{
    public long? ParentCategoryId { get; set; }
}