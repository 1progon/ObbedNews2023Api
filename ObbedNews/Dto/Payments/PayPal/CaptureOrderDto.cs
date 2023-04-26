using System.Text.Json.Serialization;

namespace ObbedNews.Dto.Payments.PayPal;

public class CaptureOrderDto
{
    [JsonPropertyName("token")] public string Token { get; set; } = null!;
    [JsonPropertyName("PayerID")] public string PayerId { get; set; } = null!;
}