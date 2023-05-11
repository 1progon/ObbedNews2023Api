namespace Obbed.Models.Words;

public class ParentCategory : BaseModel
{
    public IList<Category> Categories { get; set; } = new List<Category>();
}