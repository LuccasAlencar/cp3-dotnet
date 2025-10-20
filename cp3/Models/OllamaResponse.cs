using System.Text.Json.Serialization;

namespace cp3.Models;

public class RespostaOllama
{
    [JsonPropertyName("model")]
    public string Modelo { get; set; } = string.Empty;
    
    [JsonPropertyName("created_at")]
    public string CriadoEm { get; set; } = string.Empty;
    
    [JsonPropertyName("response")]
    public string Resposta { get; set; } = string.Empty;
    
    [JsonPropertyName("done")]
    public bool Concluido { get; set; }
}

public class ResultadoHashtagOllama
{
    [JsonPropertyName("hashtags")]
    public List<string> Hashtags { get; set; } = new();
}
