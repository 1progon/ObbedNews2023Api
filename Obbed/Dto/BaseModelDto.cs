using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Obbed.Dto;

public abstract class BaseModelDto
{
    [Required] [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [Required] [JsonPropertyName("slug")] public string Slug { get; set; } = null!;
}