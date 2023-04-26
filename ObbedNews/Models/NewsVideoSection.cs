using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ObbedNews.Models;

public class NewsVideoSection
{
    [Key] [JsonPropertyName("id")] public virtual long Id { get; set; }

    [Required] [JsonPropertyName("name")] public string Name { get; set; } = null!;

    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int SortNumber { get; set; }

    public News News { get; set; } = null!;
    public long NewsId { get; set; }

    public IList<NewsVideo> Videos { get; set; } = new List<NewsVideo>();

    public bool IsFree { get; set; }
}