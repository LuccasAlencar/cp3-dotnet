using System.Text.Json.Serialization;

namespace cp3.Models;

public class RequisicaoOllama
{
    [JsonPropertyName("model")]
    public string Modelo { get; set; } = string.Empty;
    
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
    
    [JsonPropertyName("format")]
    public object? Formato { get; set; }
}
