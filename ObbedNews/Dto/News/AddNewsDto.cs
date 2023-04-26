using System.Text.Json.Serialization;
using ObbedNews.Enums.News;

namespace ObbedNews.Dto.News;

public class AddNewsDto : BaseModelDto
{
    [JsonPropertyName("categoryId")] public long CategoryId { get; set; }

    public IFormFile? MainImage { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("article")] public string? Article { get; set; }

    [JsonPropertyName("newsLink")] public string? NewsLink { get; set; }

    [JsonPropertyName("popular")] public bool Popular { get; set; }

    // todo add tags dto
    [JsonPropertyName("tags")] public IList<TagDto>? Tags { get; set; }


    [JsonPropertyName("likes")] public long Likes { get; set; }
    [JsonPropertyName("disLikes")] public long DisLikes { get; set; }
}