namespace Obbed.Dto.VideoServer;

public class VideoFileDto
{
    public int SortNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public string Filename { get; set; } = null!;
    public string? VideoLength { get; set; }
}