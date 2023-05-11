namespace Obbed.Models.Words.Dictionary;

public class SpecialDefinitionBlock
{
    public string Name { get; set; } = null!;
    public Definition Definition { get; set; } = null!;
    public int SectionOrder { get; set; }
}