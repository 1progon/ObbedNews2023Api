namespace Obbed.Models.Words;

public class Category : BaseModel
{
    public ParentCategory? ParentCategory { get; set; }
    public long? ParentCategoryId { get; set; }

    public IList<Word> WordList { get; set; } = new List<Word>();
}