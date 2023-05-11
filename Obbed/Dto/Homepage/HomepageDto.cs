using Obbed.Models.Words;

namespace Obbed.Dto.Homepage;

public class HomepageDto
{
    public IList<Word>? Popular { get; set; }
    public IList<Word>? MostCommented { get; set; }
    public IList<Word>? Last { get; set; }
}