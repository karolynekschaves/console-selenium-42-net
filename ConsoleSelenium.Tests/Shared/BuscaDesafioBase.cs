using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Shared;

public abstract class BuscaDesafioBase
{
    protected IWebDriver Driver = null!;
    protected WebDriverWait Wait = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1920,1080");
        // User-agent real necessário: sem ele o DuckDuckGo omite os resultados orgânicos no modo headless
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");

        Driver = new ChromeDriver(options);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
    }

    [TestCleanup]
    public void Teardown()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}
