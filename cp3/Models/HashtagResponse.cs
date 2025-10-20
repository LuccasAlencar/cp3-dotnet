namespace cp3.Models;

public class RespostaHashtag
{
    public string Modelo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public List<string> Hashtags { get; set; } = new();
}
