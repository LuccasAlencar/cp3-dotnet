# Hashtag Generator API
## 👨‍💻 Desenvolvido por Luccas de Alencar Rufino | RM 558253


Uma Minimal API em .NET 8 que gera hashtags relevantes usando o Ollama (LLM local).

## 📋 Pré-requisitos

- **.NET 8+** instalado ([Download](https://dotnet.microsoft.com/download))
- **Ollama** rodando localmente na porta 11434 ([Download](https://ollama.ai))
- Modelo do Ollama baixado (ex: `llama3.2:3b`)

### Instalação do Ollama e Modelo

```bash
# Baixar o modelo
ollama pull llama3.2:3b

# Verificar se o Ollama está rodando
# O serviço deve estar disponível em http://localhost:11434
```

## 🚀 Como Executar

1. **Clone o repositório**
```bash
cd [local do arquivo]
```

2. **Execute a aplicação**
```bash
dotnet run
```

3. **Acesse o Swagger**
```
http://localhost:5066/swagger
```

## 📡 API Endpoint

### POST /hashtags

Gera hashtags relevantes para um texto fornecido.

**Request:**
```json
{
  "texto": "Explorando as belas praias do Rio de Janeiro",
  "quantidade": 8,
  "modelo": "llama3.2:3b"
}
```

**Response (200 OK):**
```json
{
  "modelo": "llama3.2:3b",
  "quantidade": 8,
  "hashtags": [
    "#RioDeJaneiro",
    "#PraiasDoRio",
    "#ViagemBrasil",
    "#Paraíso",
    "#VidasNaPraia",
    "#TurismoRJ",
    "#BrasilViagem",
    "#VerãoCarioca"
  ]
}
```

**Response (400 Bad Request):**
```json
{
  "erro": "O campo 'texto' é obrigatório e não pode estar vazio."
}
```

## 📝 Parâmetros da Request

| Campo      | Tipo   | Obrigatório | Padrão         | Descrição                                      |
|------------|--------|-------------|----------------|------------------------------------------------|
| texto      | string | Sim         | -              | Texto para gerar hashtags                      |
| quantidade | int    | Não         | 10             | Número de hashtags (mínimo: 1, máximo: 30)     |
| modelo     | string | Não         | llama3.2:3b    | Modelo do Ollama a ser usado                   |

## 🧪 Testando a API

### Usando o arquivo .http (VS Code / Rider)

O arquivo `cp3.http` contém vários casos de teste. Abra-o no VS Code (com a extensão REST Client) ou no Rider e execute os testes.

### Usando cURL

```bash
# Exemplo básico
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"texto":"Inteligência artificial e aprendizado de máquina","quantidade":5,"modelo":"llama3.2:3b"}'

# Exemplo com quantidade padrão (10)
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"texto":"Aventuras de viagem pelo mundo"}'

# Exemplo desenvolvimento
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"texto":"Desenvolvimento de software com .NET","quantidade":7}'
```

### Usando PowerShell

```powershell
$corpo = @{
    texto = "Inteligência artificial e aprendizado de máquina"
    quantidade = 5
    modelo = "llama3.2:3b"
} | ConvertTo-Json

Invoke-RestMethod -Uri http://localhost:5066/hashtags -Method Post -Body $corpo -ContentType "application/json"

# Ou use o script de teste pronto
.\test-api.ps1
```

## 🏗️ Estrutura do Projeto

```
cp3/
├── 📄 README.md                    # Documentação principal
├── 📄 .gitignore                   # Arquivos a ignorar no git
├── 📄 curl-examples.sh             # Exemplos cURL (Linux/Mac)
├── 📄 test-api.ps1                 # Script de teste PowerShell (Windows)
├── 📄 cp3.sln                      # Solution .NET
├── 📄 global.json                  # Configuração SDK .NET
│
└── cp3/                            # Projeto principal
    ├── 📄 Program.cs               # Configuração da API e endpoints
    ├── 📄 cp3.csproj               # Arquivo de projeto .NET 8
    ├── 📄 cp3.http                 # Testes HTTP (9 casos de teste)
    ├── 📄 appsettings.json         # Configurações da aplicação
    ├── 📄 appsettings.Development.json
    │
    ├── Models/                     # Modelos de dados
    │   ├── HashtagRequest.cs       # Request do endpoint
    │   ├── HashtagResponse.cs      # Response do endpoint
    │   ├── OllamaRequest.cs        # Request para Ollama API
    │   └── OllamaResponse.cs       # Response do Ollama API
    │
    └── Services/                   # Serviços
        └── OllamaService.cs        # Integração com Ollama
```

## 🔧 Tecnologias Utilizadas

- **.NET 8** - Framework
- **ASP.NET Core Minimal APIs** - Arquitetura da API
- **Ollama** - LLM local para geração de hashtags
- **HttpClient** - Cliente HTTP para comunicação com Ollama
- **Swagger/OpenAPI** - Documentação da API

## 📖 Como Funciona

1. O cliente envia uma requisição POST para `/hashtags` com um texto
2. A API valida a requisição e aplica valores padrão se necessário
3. O `ServicoOllama` cria um prompt estruturado com instruções específicas em português
4. A requisição é enviada ao Ollama API com `stream=false` e JSON schema para saída estruturada
5. O Ollama processa o prompt e retorna hashtags em formato JSON
6. A API valida, limpa e formata as hashtags (adiciona #, remove espaços, elimina duplicatas)
7. A resposta é retornada ao cliente com exatamente N hashtags

### Fluxo de Dados

```
Cliente → API → ServicoOllama → Ollama (LLM)
   ↓                                    ↓
   ←───────← Hashtags Limpas ←─────────
```

## 🎯 Destaques da Implementação

### Structured Outputs (JSON Schema)
A API usa JSON schema ao chamar o Ollama, garantindo que a resposta sempre seja um JSON válido com o formato esperado:

```csharp
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
```

### Validação em Múltiplas Camadas

1. **Validação de Entrada** (`Program.cs`)
   - Texto obrigatório e não vazio
   - Quantidade entre 1 e 30
   - Modelo válido

2. **Limpeza de Dados** (`ServicoOllama.cs`)
   - Adiciona `#` se ausente
   - Remove espaços das hashtags
   - Elimina duplicatas
   - Garante quantidade exata

3. **Tratamento de Edge Cases**
   - Quantidade ausente → padrão 10
   - Quantidade > 30 → limitado a 30
   - Hashtags insuficientes → padding com genéricas
   - Ollama offline → erro claro e útil

### Código em Português
Todo o código foi desenvolvido em português brasileiro:
- `RequisicaoHashtag`, `RespostaHashtag`
- `ServicoOllama.GerarHashtagsAsync()`
- Variáveis: `texto`, `quantidade`, `modelo`
- Mensagens de erro em PT-BR

## 🐛 Troubleshooting

### Erro: "Erro ao se comunicar com o Ollama"

**Solução:** Verifique se o Ollama está rodando:
```bash
# Verificar status
ollama list

# Se não estiver rodando, inicie o serviço Ollama
```

### Erro: "model 'llama3.2:3b' not found"

**Solução:** Baixe o modelo:
```bash
ollama pull llama3.2:3b
```

### API não responde

**Solução:** Verifique se a porta 5066 está disponível ou altere em `launchSettings.json`

## 📊 Casos de Teste Incluídos

O arquivo `cp3.http` contém **9 casos de teste** completos:

| # | Teste | Objetivo |
|---|-------|----------|
| 1 | 8 hashtags com modelo específico | Funcionamento básico |
| 2 | Quantidade padrão (10) | Valor padrão |
| 3 | 5 hashtags | Quantidade customizada |
| 4 | Quantidade máxima (30) | Limite máximo |
| 5 | Quantidade > 30 | Limitação automática |
| 6 | Quantidade ausente | Padrão sem especificar |
| 7 | Texto vazio | Validação de erro |
| 8 | Texto ausente | Validação de erro |
| 9 | Diferentes temas | Versatilidade |

## 📄 Documentação Adicional

- **`test-api.ps1`** - Script automatizado de testes

## 👨‍💻 Desenvolvido por Luccas de Alencar Rufino | RM 558253

Projeto criado para demonstrar integração de .NET Minimal APIs com Ollama para geração de conteúdo com IA.
