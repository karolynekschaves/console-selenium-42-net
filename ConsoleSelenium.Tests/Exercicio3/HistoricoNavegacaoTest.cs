using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Exercicio3;

/// <summary>
/// Exercício 3: Navegação por Histórico e Botões de Ação.
/// Simula interações de navegação utilizando os botões
/// Voltar e Avançar do navegador integrados ao fluxo de teste.
/// </summary>
[TestClass]
public sealed class HistoricoNavegacaoTest
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TestCleanup]
    public void Teardown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [TestMethod]
    public void HistoricoNavegacao_VoltarRestauraPaginaDeResultados()
    {
        // 1. Realizar uma busca inicial por "Automated Testing"
        _driver.Navigate().GoToUrl("https://duckduckgo.com");

        var campoBusca = _wait.Until(d => d.FindElement(By.Name("q")));
        campoBusca.SendKeys("Automated Testing");
        campoBusca.SendKeys(Keys.Enter);

        // Aguardar a página de resultados carregar
        _wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));
        var urlResultados = _driver.Url;

        // 2. Clicar no primeiro link orgânico relevante
        // Seletor confirmado via inspeção: a[data-testid='result-title-a']
        // O DuckDuckGo renderiza via JS; o wait explícito aguarda ao menos um link visível
        var primeiroLink = _wait.Until(d =>
        {
            var links = d.FindElements(By.CssSelector("a[data-testid='result-title-a']"));
            return links.FirstOrDefault(l => l.Displayed && !string.IsNullOrWhiteSpace(l.GetAttribute("href")));
        });

        Assert.IsNotNull(primeiroLink, "Nenhum link de resultado orgânico encontrado na página de resultados.");

        primeiroLink!.Click();

        // 3. Validar que a página externa foi carregada (URL mudou)
        _wait.Until(d => !d.Url.Equals(urlResultados, StringComparison.OrdinalIgnoreCase));
        var urlExterna = _driver.Url;

        Assert.AreNotEqual(
            urlResultados, urlExterna,
            "Esperado que a URL mudasse após clicar no link externo."
        );

        // 4. Voltar à página de resultados de busca
        _driver.Navigate().Back();

        // 5. Validar que a página de resultados foi restaurada
        _wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(
            _driver.Url.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"Esperado retornar à página de resultados com 'q=' na URL. URL atual: '{_driver.Url}'"
        );
    }
}
