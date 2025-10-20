namespace cp3.Models;

public class RequisicaoHashtag
{
    public string Texto { get; set; } = string.Empty;
    public int? Quantidade { get; set; }
    public string Modelo { get; set; } = "llama3.2:3b";
}
