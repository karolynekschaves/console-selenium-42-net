using OpenQA.Selenium;
using ConsoleSelenium.Tests.Shared;

namespace ConsoleSelenium.Tests.Exercicio3;

/// <summary>
/// Exercício 3: Navegação por Histórico e Botões de Ação.
/// Simula interações de navegação utilizando os botões
/// Voltar e Avançar do navegador integrados ao fluxo de teste.
/// </summary>
[TestClass]
public sealed class HistoricoNavegacaoTest : BuscaDesafioBase
{
    [TestMethod]
    public void HistoricoNavegacao_VoltarRestauraPaginaDeResultados()
    {
        // 1. Realizar uma busca inicial por "Automated Testing"
        Driver.Navigate().GoToUrl("https://duckduckgo.com");

        var campoBusca = Wait.Until(d => d.FindElement(By.Name("q")));
        campoBusca.SendKeys("Automated Testing");
        campoBusca.SendKeys(Keys.Enter);

        // Aguardar a página de resultados carregar
        Wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));
        var urlResultados = Driver.Url;

        // 2. Clicar no primeiro link orgânico relevante
        // Seletor confirmado via inspeção: a[data-testid='result-title-a']
        // O DuckDuckGo renderiza via JS; o wait explícito aguarda ao menos um link visível
        var primeiroLink = Wait.Until(d =>
        {
            var links = d.FindElements(By.CssSelector("a[data-testid='result-title-a']"));
            return links.FirstOrDefault(l => l.Displayed && !string.IsNullOrWhiteSpace(l.GetAttribute("href")));
        });

        Assert.IsNotNull(primeiroLink, "Nenhum link de resultado orgânico encontrado na página de resultados.");

        primeiroLink!.Click();

        // 3. Validar que a página externa foi carregada (URL mudou)
        Wait.Until(d => !d.Url.Equals(urlResultados, StringComparison.OrdinalIgnoreCase));
        var urlExterna = Driver.Url;

        Assert.AreNotEqual(
            urlResultados, urlExterna,
            "Esperado que a URL mudasse após clicar no link externo."
        );

        // 4. Voltar à página de resultados de busca
        Driver.Navigate().Back();

        // 5. Validar que a página de resultados foi restaurada
        Wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(
            Driver.Url.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"Esperado retornar à página de resultados com 'q=' na URL. URL atual: '{Driver.Url}'"
        );
    }
}
