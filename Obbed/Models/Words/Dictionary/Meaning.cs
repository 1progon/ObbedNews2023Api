namespace Obbed.Models.Words.Dictionary;

public class Meaning
{
    public string Mean { get; set; } = null!;
    public IList<Definition> Definitions { get; set; } = new List<Definition>();
    public int SectionOrder { get; set; }
}