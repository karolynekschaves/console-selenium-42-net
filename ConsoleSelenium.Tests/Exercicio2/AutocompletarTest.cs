using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Exercicio2;

/// <summary>
/// Exercício 2: Interagindo com o Autocompletar (Suggestions).
/// Lida com menus suspensos dinâmicos gerados via JavaScript
/// enquanto o usuário digita no campo de pesquisa.
/// </summary>
[TestClass]
public sealed class AutocompletarTest
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
    public void Autocompletar_ClicaNaTerceiraOpcaoEValidaResultado()
    {
        // 1. Acessar a página inicial do DuckDuckGo
        _driver.Navigate().GoToUrl("https://duckduckgo.com");

        // 2. Digitar o termo sem pressionar Enter
        var campoBusca = _wait.Until(d => d.FindElement(By.Name("q")));
        campoBusca.SendKeys("C# unit testing");

        // 3. Aguardar a listagem de sugestões aparecer no DOM
        // Seletor confirmado via inspeção: [role='option'] com id=suggestion-N
        IReadOnlyCollection<IWebElement>? sugestoes = null;
        _wait.Until(d =>
        {
            var elementos = d.FindElements(By.CssSelector("[role='option']"));
            if (elementos.Count >= 3)
            {
                sugestoes = elementos;
                return true;
            }
            return false;
        });

        Assert.IsNotNull(sugestoes, "Nenhuma lista de sugestões encontrada com o seletor [role='option'].");
        Assert.IsGreaterThanOrEqualTo(3, sugestoes!.Count, $"Esperado ao menos 3 sugestões, encontrado: {sugestoes.Count}");

        // 4. Clicar na terceira opção (índice 2)
        var terceiraOpcao = sugestoes.ElementAt(2);
        terceiraOpcao.Click();

        // 5. Validar que a navegação redirecionou para os resultados
        _wait.Until(d =>
            d.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase) &&
            d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(
            _driver.Url.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"Esperado que a URL de resultados contivesse 'q='. URL atual: '{_driver.Url}'"
        );
    }
}
