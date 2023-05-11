using System.Text.Json.Serialization;

namespace Obbed.Dto.Payments.PayPal;

public class CaptureOrderDto
{
    [JsonPropertyName("token")] public string Token { get; set; } = null!;
    [JsonPropertyName("PayerID")] public string PayerId { get; set; } = null!;
}