using System.Text.Json.Serialization;
using ObbedNews.Models.Payment;

namespace ObbedNews.Dto.Payments.PayPal;

public class PayPalCreateOrderResponseDto
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("links")] public object[] Links { get; set; } = null!;
    [JsonPropertyName("order")] public PayPalOrder PayPalOrder { get; set; } = null!;
}