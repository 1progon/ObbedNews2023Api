using Obbed.Models.Words;

namespace Obbed.Models;

public class Tag : BaseModel
{
    public IList<Word>? WordList { get; set; }
}