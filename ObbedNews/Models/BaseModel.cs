using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ObbedNews.Models;

[Index(nameof(Slug), IsUnique = true)]
public abstract class BaseModel
{
    [Key] [JsonPropertyName("id")] public virtual long Id { get; set; }

    [Required] [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [Required] [JsonPropertyName("slug")] public string Slug { get; set; } = null!;
}