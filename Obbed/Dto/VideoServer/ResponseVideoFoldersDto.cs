using System.Net;

namespace Obbed.Dto.VideoServer;

public class ResponseVideoFoldersDto
{
    public IList<VideoFolderDto> Folders { get; set; } = new List<VideoFolderDto>();
    public string? Message { get; set; }
    public HttpStatusCode Status { get; set; }
    public long NewsId { get; set; }
}