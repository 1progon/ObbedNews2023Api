using System.Text.Json.Serialization;
using Obbed.Models.Words;

namespace Obbed.Dto.Words;

public class GetWordDto
{
    [JsonPropertyName("word")] public Word Word { get; set; } = null!;
    [JsonPropertyName("nearbyWords")] public IList<Word> NearbyWords { get; set; } = null!;
}