using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Obbed.Models.Words.Dictionary;

namespace Obbed.Dto.Words;

public class UpdateWordDto : BaseModelDto
{
    [Required] public long Id { get; set; }
    [JsonPropertyName("categoryId")] public long CategoryId { get; set; }

    public IFormFile? MainImage { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("article")] public string? Article { get; set; }

    [JsonPropertyName("newsLink")] public string? NewsLink { get; set; }

    [JsonPropertyName("popular")] public bool Popular { get; set; }

    // todo add tags dto
    [JsonPropertyName("tags")] public IList<TagDto>? Tags { get; set; }

    [JsonPropertyName("removeImage")] public bool RemoveImage { get; set; }


    [JsonPropertyName("likes")] public long Likes { get; set; }
    [JsonPropertyName("disLikes")] public long DisLikes { get; set; }

    [JsonPropertyName("wordSection")] public WordSection? WordSection { get; set; }
}