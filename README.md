# ConsoleSelenium42

Projeto de automação de testes web com Selenium WebDriver em C#, desenvolvido como exercício prático com três cenários de teste no DuckDuckGo.

## Tecnologias

| Tecnologia | Versão |
|---|---|
| .NET | 10.0 |
| Selenium.WebDriver | 4.53.0 |
| Selenium.Support | 4.53.0 |
| MSTest | 4.0.2 |
| Navegador | Google Chrome (headless) |

## Estrutura do projeto

```
ConsoleSelenium42/
├── ConsoleSelenium/
│   └── ConsoleSelenium.csproj
├── ConsoleSelenium.Tests/
│   ├── Exercicio1/
│   │   └── PesquisaBasicaTest.cs
│   ├── Exercicio2/
│   │   └── AutocompletarTest.cs
│   ├── Exercicio3/
│   │   └── HistoricoNavegacaoTest.cs
│   ├── MSTestSettings.cs
│   └── ConsoleSelenium.Tests.csproj
└── ConsoleSelenium42.slnx
```

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Google Chrome instalado na máquina
- ChromeDriver compatível com a versão do Chrome instalada (gerenciado automaticamente pelo pacote `Selenium.WebDriver`)

## Como executar

**Todos os testes:**
```bash
dotnet test ConsoleSelenium.Tests/ConsoleSelenium.Tests.csproj
```

**Um exercício específico:**
```bash
dotnet test --filter "PesquisaBasica"
dotnet test --filter "Autocompletar"
dotnet test --filter "HistoricoNavegacao"
```

---

## Exercícios

### Exercício 1 — Pesquisa Básica e Validação de Título
**Arquivo:** `Exercicio1/PesquisaBasicaTest.cs`

Automatiza o fluxo de pesquisa mais simples do DuckDuckGo e valida que a página de resultados foi carregada corretamente.
---

### Exercício 2 — Autocompletar (Suggestions)
**Arquivo:** `Exercicio2/AutocompletarTest.cs`

Lida com menus suspensos dinâmicos gerados via JavaScript enquanto o usuário digita, usando espera explícita para aguardar os elementos no DOM.

---

### Exercício 3 — Navegação por Histórico
**Arquivo:** `Exercicio3/HistoricoNavegacaoTest.cs`

Simula navegação complexa utilizando os botões Voltar e Avançar do navegador.

```csharp
options.AddArgument("--headless=new");
options.AddArgument("--no-sandbox");
options.AddArgument("--disable-dev-shm-usage");
options.AddArgument("--window-size=1920,1080");
options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) ...");
```
