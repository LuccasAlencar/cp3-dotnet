using cp3.Models;
using cp3.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra HttpClient e ServicoOllama
builder.Services.AddHttpClient<ServicoOllama>();

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint POST /hashtags
app.MapPost("/hashtags", async (RequisicaoHashtag requisicao, ServicoOllama servicoOllama) =>
    {
        try
        {
            // Valida a requisição
            if (string.IsNullOrWhiteSpace(requisicao.Texto))
            {
                return Results.BadRequest(new { erro = "O campo 'texto' é obrigatório e não pode estar vazio." });
            }

            // Define quantidade padrão se não fornecida, com máximo de 30
            var quantidade = requisicao.Quantidade ?? 10;
            if (quantidade < 1)
            {
                return Results.BadRequest(new { erro = "O campo 'quantidade' deve ser maior que 0." });
            }
            if (quantidade > 30)
            {
                quantidade = 30;
            }

            // Define modelo padrão se não fornecido
            var modelo = string.IsNullOrWhiteSpace(requisicao.Modelo) ? "llama3.2:3b" : requisicao.Modelo;

            // Gera as hashtags
            var hashtags = await servicoOllama.GerarHashtagsAsync(requisicao.Texto, quantidade, modelo);

            // Cria a resposta
            var resposta = new RespostaHashtag
            {
                Modelo = modelo,
                Quantidade = hashtags.Count,
                Hashtags = hashtags
            };

            return Results.Ok(resposta);
        }
        catch (HttpRequestException ex)
        {
            return Results.BadRequest(new 
            { 
                erro = "Erro ao se comunicar com o Ollama. Verifique se o serviço está rodando em http://localhost:11434",
                detalhes = ex.Message 
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new 
            { 
                erro = "Erro ao gerar hashtags.",
                detalhes = ex.Message 
            });
        }
    })
    .WithName("GerarHashtags")
    .WithOpenApi()
    .Produces<RespostaHashtag>(StatusCodes.Status200OK)
    .Produces<object>(StatusCodes.Status400BadRequest);

app.Run();