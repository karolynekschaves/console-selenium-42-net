using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Exercicio1;

/// <summary>
/// Exercício 1: A Experiência de Pesquisa Básica e Validação de Título.
/// Automatiza a digitação na barra de pesquisa do DuckDuckGo,
/// aciona a submissão e valida o carregamento da página de resultados.
/// </summary>
[TestClass]
public sealed class PesquisaBasicaTest
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
    public void PesquisaBasica_TituloOuUrlContemTermoPesquisado()
    {
        // 1. Navegar até a URL base
        _driver.Navigate().GoToUrl("https://duckduckgo.com");

        // 2. Localizar o campo de pesquisa pelo atributo name="q"
        var campoBusca = _wait.Until(d => d.FindElement(By.Name("q")));

        // 3. Digitar o termo de pesquisa
        campoBusca.SendKeys("Selenium WebDriver C#");

        // 4. Submeter pressionando Enter
        campoBusca.SendKeys(Keys.Enter);

        // 5. Validar que o título ou a URL contém o termo pesquisado
        _wait.Until(d =>
            d.Title.Contains("Selenium", StringComparison.OrdinalIgnoreCase) ||
            d.Url.Contains("Selenium", StringComparison.OrdinalIgnoreCase));

        bool tituloContemTermo = _driver.Title.Contains("Selenium", StringComparison.OrdinalIgnoreCase);
        bool urlContemTermo = _driver.Url.Contains("Selenium", StringComparison.OrdinalIgnoreCase);

        Assert.IsTrue(
            tituloContemTermo || urlContemTermo,
            $"Esperado que o título ou URL contivesse 'Selenium'. Título: '{_driver.Title}' | URL: '{_driver.Url}'"
        );
    }
}
