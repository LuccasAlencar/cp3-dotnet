using System.Text;
using System.Text.Json;
using cp3.Models;

namespace cp3.Services;

public class ServicoOllama
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ServicoOllama> _logger;
    private const string UrlApiOllama = "http://localhost:11434/api/generate";

    public ServicoOllama(HttpClient httpClient, ILogger<ServicoOllama> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<string>> GerarHashtagsAsync(string texto, int quantidade, string modelo)
    {
        try
        {
            // Define o schema JSON para saída estruturada
            var schemaJson = new
            {
                type = "object",
                properties = new
                {
                    hashtags = new
                    {
                        type = "array",
                        items = new { type = "string" },
                        description = "Array de hashtags"
                    }
                },
                required = new[] { "hashtags" }
            };

            // Cria o prompt
            var prompt = $@"Gere exatamente {quantidade} hashtags relevantes para o seguinte texto. 
Regras:
1. Cada hashtag deve começar com #
2. Sem espaços nas hashtags (use camelCase ou PascalCase se necessário)
3. Sem duplicatas
4. Retorne APENAS um objeto JSON com um array 'hashtags'
5. Cada hashtag deve ser relevante ao conteúdo

Texto: {texto}

Responda apenas com JSON.";

            var requisicao = new RequisicaoOllama
            {
                Modelo = modelo,
                Prompt = prompt,
                Stream = false,
                Formato = schemaJson
            };

            var jsonConteudo = JsonSerializer.Serialize(requisicao);
            var conteudo = new StringContent(jsonConteudo, Encoding.UTF8, "application/json");

            _logger.LogInformation("Enviando requisição para API do Ollama com modelo: {Modelo}", modelo);
            
            var resposta = await _httpClient.PostAsync(UrlApiOllama, conteudo);
            
            if (!resposta.IsSuccessStatusCode)
            {
                var conteudoErro = await resposta.Content.ReadAsStringAsync();
                _logger.LogError("Erro na API do Ollama: {StatusCode} - {Conteudo}", resposta.StatusCode, conteudoErro);
                throw new HttpRequestException($"API do Ollama retornou {resposta.StatusCode}: {conteudoErro}");
            }

            var conteudoResposta = await resposta.Content.ReadAsStringAsync();
            _logger.LogInformation("Resposta recebida da API do Ollama");
            
            var respostaOllama = JsonSerializer.Deserialize<RespostaOllama>(conteudoResposta);
            
            if (respostaOllama == null || string.IsNullOrEmpty(respostaOllama.Resposta))
            {
                throw new InvalidOperationException("Resposta inválida da API do Ollama");
            }

            // Extrai as hashtags da resposta JSON
            var resultadoHashtag = JsonSerializer.Deserialize<ResultadoHashtagOllama>(respostaOllama.Resposta);
            
            if (resultadoHashtag == null || resultadoHashtag.Hashtags == null)
            {
                throw new InvalidOperationException("Falha ao extrair hashtags da resposta do Ollama");
            }

            // Limpa e valida as hashtags
            var hashtagsLimpas = resultadoHashtag.Hashtags
                .Select(h => h.Trim())
                .Where(h => !string.IsNullOrEmpty(h))
                .Select(h => h.StartsWith("#") ? h : $"#{h}")
                .Select(h => h.Replace(" ", ""))
                .Distinct()
                .Take(quantidade)
                .ToList();

            // Se não obtivemos hashtags suficientes, adiciona genéricas
            while (hashtagsLimpas.Count < quantidade)
            {
                var tagGenerica = $"#Tag{hashtagsLimpas.Count + 1}";
                if (!hashtagsLimpas.Contains(tagGenerica))
                {
                    hashtagsLimpas.Add(tagGenerica);
                }
            }

            return hashtagsLimpas.Take(quantidade).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar hashtags");
            throw;
        }
    }
}
