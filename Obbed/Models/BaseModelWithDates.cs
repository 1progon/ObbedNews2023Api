using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Obbed.Models;

public abstract class BaseModelWithDates : BaseModel
{
    [Required] [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Required] [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}