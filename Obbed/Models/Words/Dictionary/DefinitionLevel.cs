using Obbed.Enums.News;

namespace Obbed.Models.Words.Dictionary;

public class DefinitionLabel
{
    public string Name { get; set; } = null!;
    public SpeechPartEnum SpeechPartEnum { get; set; }
    public string Description { get; set; } = null!;
}