using System.Text.Json.Serialization;

namespace Obbed.Dto.Words;

public class TagDto : BaseModelDto
{
    [JsonPropertyName("id")] public long? Id { get; set; }
}