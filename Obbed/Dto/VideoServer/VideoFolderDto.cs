namespace Obbed.Dto.VideoServer;

public class VideoFolderDto
{
    public string Name { get; set; } = null!;
    public int SortNumber { get; set; }

    public IList<VideoFileDto> Files { get; set; } = new List<VideoFileDto>();
}