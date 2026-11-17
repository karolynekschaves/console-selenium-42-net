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

    // Exercício 2: Interagindo com o Autocompletar (Suggestions)
    [TestMethod]
    public void Exercicio2_Autocompletar_ClicaNaTerceiraOpcaoEValidaResultado()
    {
        // 1. Acessar a página inicial do DuckDuckGo
        _driver.Navigate().GoToUrl("https://duckduckgo.com");

        // 2. Digitar o termo sem pressionar Enter
        var campoBusca = _wait.Until(d => d.FindElement(By.Name("q")));
        campoBusca.SendKeys("C# unit testing");

        // 3. Aguardar a listagem de sugestões aparecer no DOM
        var seletoresSugestoes = new[]
        {
            ".search-suggestions-list li",
            ".sbox-suggestions li",
            "[role='option']"
        };

        IReadOnlyCollection<IWebElement>? sugestoes = null;
        _wait.Until(d =>
        {
            foreach (var seletor in seletoresSugestoes)
            {
                var elementos = d.FindElements(By.CssSelector(seletor));
                if (elementos.Count >= 3)
                {
                    sugestoes = elementos;
                    return true;
                }
            }
            return false;
        });

        Assert.IsNotNull(sugestoes, "Nenhuma lista de sugestões encontrada com os seletores fornecidos.");
        Assert.IsGreaterThanOrEqualTo(sugestoes!.Count, 3, $"Esperado ao menos 3 sugestões, encontrado: {sugestoes.Count}");

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
