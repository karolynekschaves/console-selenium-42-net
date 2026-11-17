using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests;

[TestClass]
public sealed class BuscaDesafioTest
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

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TestCleanup]
    public void Teardown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    // Exercício 1: A Experiência de Pesquisa Básica e Validação de Título
    [TestMethod]
    public void Exercicio1_PesquisaBasica_TituloOuUrlContemTermoPesquisado()
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
