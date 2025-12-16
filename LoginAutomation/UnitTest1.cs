using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;
using System.IO;

namespace LoginAutomation;

public sealed class LoginTests
{
    [Test]
    public void Login_WithValidCredentials_ShouldSucceed()
    {
        var htmlPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "login.html");
        var url = new System.Uri(htmlPath).AbsoluteUri;

        if (!string.Equals(System.Environment.GetEnvironmentVariable("RUN_SELENIUM"), "1", System.StringComparison.Ordinal))
        {
            return;
        }

        var credentials = ReadCredentials();

        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");

        foreach (var credential in credentials)
        {
            using IWebDriver driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.Id("username")).SendKeys(credential.Username);
            driver.FindElement(By.Id("password")).SendKeys(credential.Password);
            driver.FindElement(By.Id("loginButton")).Click();

            var result = driver.FindElement(By.Id("result")).Text;
            Assert.That(result, Does.Contain("Login successful"), $"Username: {credential.Username}");
        }
    }

    private static Credentials[] ReadCredentials()
    {
        using var reader = new StreamReader(Path.Combine(TestContext.CurrentContext.TestDirectory, "LoginTestData.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Credentials>().ToArray();
    }

    private sealed class Credentials
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
