using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Obbed.Models.Words;

public class WordVideo
{
    [Key] [JsonPropertyName("id")] public virtual long Id { get; set; }

    [Required] [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [Required] [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Required] [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    public int SortNumber { get; set; }
    public string? MainThumb { get; set; }

    public string? Folder { get; set; }
    public string? Filename { get; set; }

    [NotMapped] public string? FilePath => Folder + '/' + Filename;

    public string? RemoteUrl { get; set; }

    public string? Description { get; set; }

    // todo comments for current video?

    public Word Word { get; set; } = null!;
    public long WordId { get; set; }

    public WordVideoSection Section { get; set; } = null!;
    public long SectionId { get; set; }

    public bool IsFree { get; set; }

    public string VideoLength { get; set; } = null!;
}