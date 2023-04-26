using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ObbedNews.Enums.News;

namespace ObbedNews.Dto.News;

public class UpdateNewsDto : BaseModelDto
{
    [Required] public long Id { get; set; }
    [JsonPropertyName("categoryId")] public long CategoryId { get; set; }
    [JsonPropertyName("languageId")] public long LanguageId { get; set; }
    [JsonPropertyName("level")] public NewsLevel Level { get; set; }

    public IFormFile? MainImage { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("article")] public string? Article { get; set; }

    [JsonPropertyName("newsLink")] public string? NewsLink { get; set; }
    [JsonPropertyName("torrentLink")] public string? TorrentLink { get; set; }

    [JsonPropertyName("popular")] public bool Popular { get; set; }

    // todo add tags dto
    [JsonPropertyName("tags")] public IList<TagDto>? Tags { get; set; }

    [JsonPropertyName("removeImage")] public bool RemoveImage { get; set; }


    [JsonPropertyName("likes")] public long Likes { get; set; }
    [JsonPropertyName("disLikes")] public long DisLikes { get; set; }
}