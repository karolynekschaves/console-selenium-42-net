using OpenQA.Selenium;
using ConsoleSelenium.Tests.Shared;

namespace ConsoleSelenium.Tests.Exercicio2;

/// <summary>
/// Exercício 2: Interagindo com o Autocompletar (Suggestions).
/// Lida com menus suspensos dinâmicos gerados via JavaScript
/// enquanto o usuário digita no campo de pesquisa.
/// </summary>
[TestClass]
public sealed class AutocompletarTest : BuscaDesafioBase
{
    [TestMethod]
    public void Autocompletar_ClicaNaTerceiraOpcaoEValidaResultado()
    {
        // 1. Acessar a página inicial do DuckDuckGo
        Driver.Navigate().GoToUrl("https://duckduckgo.com");

        // 2. Digitar o termo sem pressionar Enter
        var campoBusca = Wait.Until(d => d.FindElement(By.Name("q")));
        campoBusca.SendKeys("C# unit testing");

        // 3. Aguardar a listagem de sugestões aparecer no DOM
        // Seletor confirmado via inspeção: [role='option'] com id=suggestion-N
        IReadOnlyCollection<IWebElement>? sugestoes = null;
        Wait.Until(d =>
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
        Wait.Until(d =>
            d.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase) &&
            d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        Assert.IsTrue(
            Driver.Url.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"Esperado que a URL de resultados contivesse 'q='. URL atual: '{Driver.Url}'"
        );
    }
}
