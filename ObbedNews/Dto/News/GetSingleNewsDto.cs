using System.Text.Json.Serialization;

namespace ObbedNews.Dto.News;

public class GetSingleNewsDto
{
    [JsonPropertyName("news")] public Models.News News { get; set; } = null!;
    [JsonPropertyName("nearbyNews")] public IList<Models.News> NearbyNews { get; set; } = null!;
}