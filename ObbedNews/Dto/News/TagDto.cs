using System.Text.Json.Serialization;

namespace ObbedNews.Dto.News;

public class TagDto : BaseModelDto
{
    [JsonPropertyName("id")] public long? Id { get; set; }
}