using Obbed.Enums.News;

namespace Obbed.Models.Words.Dictionary;

public class Definition
{
    public string DefText { get; set; } = null!;
    public EngLevel? Level { get; set; }
    public IList<DefinitionLabel>? Labels { get; set; }
    public IList<DefinitionExample>? Examples { get; set; }

    public int? OrderInsideMeaning { get; set; }
    public int SectionOrder { get; set; }
}