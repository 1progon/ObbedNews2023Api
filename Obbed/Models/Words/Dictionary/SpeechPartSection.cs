using Obbed.Enums.News;

namespace Obbed.Models.Words.Dictionary;

public class SpeechPartSection
{
    public int Order { get; set; }
    public SpeechPartEnum SpeechPartEnum { get; set; }
    public IList<WordSound>? Sounds { get; set; }
    public IList<Meaning>? Meanings { get; set; }
    public IList<Definition>? Definitions { get; set; }
    public IList<SpecialDefinitionBlock>? SpecBlocks { get; set; }
}