namespace Obbed.Models.Words.Dictionary;

public class DefinitionExample
{
    public string Example { get; set; } = null!;
    public string ExampleTranslation { get; set; } = null!;
    public IList<DefinitionLabel>? Labels { get; set; } = new List<DefinitionLabel>();
}