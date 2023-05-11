using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Obbed.Models.Words;

public class WordVideoSection
{
    [Key] [JsonPropertyName("id")] public virtual long Id { get; set; }

    [Required] [JsonPropertyName("name")] public string Name { get; set; } = null!;

    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int SortNumber { get; set; }

    public Word Word { get; set; } = null!;
    public long WordId { get; set; }

    public IList<WordVideo> Videos { get; set; } = new List<WordVideo>();

    public bool IsFree { get; set; }
}