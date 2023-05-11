namespace Obbed.Models.Words.Dictionary;

public class WordSound
{
    public string Language { get; set; } = null!;
    public IFormFile? Sound { get; set; }
    public string? Transcription { get; set; }
}