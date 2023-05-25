using Obbed.Enums.News;

namespace Obbed.Models.Words.Dictionary;

public class SpeechPartSection
{
    public int Order { get; set; }
    public SpeechPartEnum SpeechPartEnum { get; set; }
    public IList<WordSound>? Sounds { get; set; } = new List<WordSound>();
    public IList<Meaning>? Meanings { get; set; } = new List<Meaning>();
    public IList<Definition>? Definitions { get; set; } = new List<Definition>();
    public IList<SpecialDefinitionBlock>? SpecBlocks { get; set; } = new List<SpecialDefinitionBlock>();
}