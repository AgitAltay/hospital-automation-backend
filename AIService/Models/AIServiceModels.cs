using System.Text.Json.Serialization;

namespace AIService.Models;

public class ComplaintRequest
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}

public class PredictionResponse
{
    [JsonPropertyName("label")]
    public required string Label { get; set; }

    [JsonPropertyName("score")]
    public float Score { get; set; }
}