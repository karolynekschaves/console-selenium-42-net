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

**Passos do teste:**
1. Navega até `https://duckduckgo.com`
2. Localiza o campo de pesquisa pelo atributo `name="q"`
3. Digita o termo `Selenium WebDriver C#` via `SendKeys`
4. Submete o formulário com `Keys.Enter`
5. Valida que o título ou a URL da página de resultados contém o termo pesquisado

---

### Exercício 2 — Autocompletar (Suggestions)
**Arquivo:** `Exercicio2/AutocompletarTest.cs`

Lida com menus suspensos dinâmicos gerados via JavaScript enquanto o usuário digita, usando espera explícita para aguardar os elementos no DOM.

**Passos do teste:**
1. Navega até `https://duckduckgo.com`
2. Digita `C# unit testing` no campo de pesquisa sem pressionar Enter
3. Aguarda a lista de sugestões aparecer no DOM com `WebDriverWait` usando o seletor `[role='option']`
4. Clica na terceira sugestão da lista (índice 2)
5. Valida que a URL de resultados contém `q=`

> **Nota:** o seletor `[role='option']` foi confirmado por inspeção do DOM real. Os seletores `.search-suggestions-list li` e `.sbox-suggestions li` não retornam elementos no layout atual do DuckDuckGo.

---

### Exercício 3 — Navegação por Histórico
**Arquivo:** `Exercicio3/HistoricoNavegacaoTest.cs`

Simula navegação complexa utilizando os botões Voltar e Avançar do navegador.

**Passos do teste:**
1. Realiza uma busca por `Automated Testing`
2. Aguarda a página de resultados carregar e salva a URL
3. Clica no primeiro link orgânico visível usando o seletor `a[data-testid='result-title-a']`
4. Valida que a URL mudou (página externa carregada)
5. Chama `Driver.Navigate().Back()` para retornar
6. Valida que a URL de resultados foi restaurada

> **Nota:** sem um `--user-agent` de browser real, o DuckDuckGo omite os resultados orgânicos no modo headless. O ChromeDriver é configurado com um user-agent do Chrome 131 para contornar esse comportamento.

---

## Configuração do ChromeDriver

Todos os testes usam o Chrome em modo headless com as seguintes opções:

```csharp
options.AddArgument("--headless=new");
options.AddArgument("--no-sandbox");
options.AddArgument("--disable-dev-shm-usage");
options.AddArgument("--window-size=1920,1080");
options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) ...");
```

O `[TestInitialize]` abre o browser antes de cada teste e o `[TestCleanup]` garante o fechamento mesmo em caso de falha.

---

## Histórico de commits

```
feat(setup): cria estrutura inicial da solution console-selenium-42
test(duckduckgo): implementa cenario de pesquisa basica com validacao de titulo e url
test(duckduckgo): implementa cenario de autocompletar com WebDriverWait e clique na terceira sugestao
test(duckduckgo): implementa cenario de historico de navegacao com Back() e validacao de url
fix(duckduckgo): corrige seletores e user-agent para exercicios 2 e 3
refactor(tests): separa exercicios em pastas e extrai classe base compartilhada
refactor(tests): remove classe base e internaliza setup em cada classe de teste
```
