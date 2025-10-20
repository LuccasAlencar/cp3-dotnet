# Hashtag Generator API - Script de Teste PowerShell
# Certifique-se de que a API está rodando em http://localhost:5066

$urlBase = "http://localhost:5066/hashtags"

function Test-EndpointHashtag {
    param(
        [string]$NomeTeste,
        [hashtable]$Corpo
    )
    
    Write-Host "`n=== $NomeTeste ===" -ForegroundColor Cyan
    
    try {
        $jsonCorpo = $Corpo | ConvertTo-Json -Compress
        $resposta = Invoke-RestMethod -Uri $urlBase -Method Post -Body $jsonCorpo -ContentType "application/json"
        $resposta | ConvertTo-Json -Depth 10
    }
    catch {
        Write-Host "Erro: $_" -ForegroundColor Red
        if ($_.Exception.Response) {
            $leitor = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $leitor.BaseStream.Position = 0
            $leitor.DiscardBufferedData()
            $corpoResposta = $leitor.ReadToEnd()
            Write-Host "Resposta: $corpoResposta" -ForegroundColor Yellow
        }
    }
}

# Teste 1: Gerar 8 hashtags com modelo específico
Test-EndpointHashtag -NomeTeste "Teste 1: Gerar 8 hashtags com modelo específico" -Corpo @{
    texto = "Explorando as belas praias do Rio de Janeiro com vistas incríveis do pôr do sol e comida brasileira deliciosa"
    quantidade = 8
    modelo = "llama3.2:3b"
}

# Teste 2: Gerar hashtags com quantidade padrão (10)
Test-EndpointHashtag -NomeTeste "Teste 2: Gerar hashtags com quantidade padrão (10)" -Corpo @{
    texto = "Aprendendo .NET 8 Minimal APIs e integrando com modelos de IA para soluções inovadoras"
    modelo = "llama3.2:3b"
}

# Teste 3: Gerar 5 hashtags
Test-EndpointHashtag -NomeTeste "Teste 3: Gerar 5 hashtags" -Corpo @{
    texto = "Inovação tecnológica, inteligência artificial, aprendizado de máquina e o futuro do desenvolvimento de software"
    quantidade = 5
    modelo = "llama3.2:3b"
}

# Teste 4: Teste com quantidade máxima (30)
Test-EndpointHashtag -NomeTeste "Teste 4: Teste com quantidade máxima (30)" -Corpo @{
    texto = "Conscientização sobre mudanças climáticas, sustentabilidade, energia renovável, proteção ambiental, tecnologia verde"
    quantidade = 30
    modelo = "llama3.2:3b"
}

# Teste 5: Teste com quantidade > 30 (deve limitar a 30)
Test-EndpointHashtag -NomeTeste "Teste 5: Teste com quantidade > 30 (deve limitar a 30)" -Corpo @{
    texto = "Estratégias de marketing digital para crescimento e engajamento nas redes sociais"
    quantidade = 50
    modelo = "llama3.2:3b"
}

# Teste 6: Quantidade ausente (deve usar padrão 10)
Test-EndpointHashtag -NomeTeste "Teste 6: Quantidade ausente (deve usar padrão 10)" -Corpo @{
    texto = "Aventuras de viagem pelo mundo, explorando novas culturas e culinárias"
}

# Teste 7: Erro - Texto vazio
Test-EndpointHashtag -NomeTeste "Teste 7: Erro - Texto vazio" -Corpo @{
    texto = ""
    quantidade = 5
    modelo = "llama3.2:3b"
}

# Teste 8: Erro - Texto ausente
Test-EndpointHashtag -NomeTeste "Teste 8: Erro - Texto ausente" -Corpo @{
    quantidade = 5
    modelo = "llama3.2:3b"
}

# Teste 9: Teste com diferentes temas
Test-EndpointHashtag -NomeTeste "Teste 9: Teste com diferentes temas" -Corpo @{
    texto = "Desenvolvimento de software com inteligência artificial e aprendizado de máquina usando .NET"
    quantidade = 7
    modelo = "llama3.2:3b"
}

Write-Host "`n=== Todos os testes concluídos ===" -ForegroundColor Green
