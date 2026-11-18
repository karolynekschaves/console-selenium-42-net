using OpenQA.Selenium;
using ConsoleSelenium.Tests.Shared;

namespace ConsoleSelenium.Tests.Exercicio1;

/// <summary>
/// Exercício 1: A Experiência de Pesquisa Básica e Validação de Título.
/// Automatiza a digitação na barra de pesquisa do DuckDuckGo,
/// aciona a submissão e valida o carregamento da página de resultados.
/// </summary>
[TestClass]
public sealed class PesquisaBasicaTest : BuscaDesafioBase
{
    [TestMethod]
    public void PesquisaBasica_TituloOuUrlContemTermoPesquisado()
    {
        // 1. Navegar até a URL base
        Driver.Navigate().GoToUrl("https://duckduckgo.com");

        // 2. Localizar o campo de pesquisa pelo atributo name="q"
        var campoBusca = Wait.Until(d => d.FindElement(By.Name("q")));

        // 3. Digitar o termo de pesquisa
        campoBusca.SendKeys("Selenium WebDriver C#");

        // 4. Submeter pressionando Enter
        campoBusca.SendKeys(Keys.Enter);

        // 5. Validar que o título ou a URL contém o termo pesquisado
        Wait.Until(d =>
            d.Title.Contains("Selenium", StringComparison.OrdinalIgnoreCase) ||
            d.Url.Contains("Selenium", StringComparison.OrdinalIgnoreCase));

        bool tituloContemTermo = Driver.Title.Contains("Selenium", StringComparison.OrdinalIgnoreCase);
        bool urlContemTermo = Driver.Url.Contains("Selenium", StringComparison.OrdinalIgnoreCase);

        Assert.IsTrue(
            tituloContemTermo || urlContemTermo,
            $"Esperado que o título ou URL contivesse 'Selenium'. Título: '{Driver.Title}' | URL: '{Driver.Url}'"
        );
    }
}
