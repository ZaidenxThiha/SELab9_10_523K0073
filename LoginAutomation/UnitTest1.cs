using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LoginAutomation;

public sealed class LoginTests
{
    private static IEnumerable<TestCaseData> GetTestData()
    {
        using var reader = new StreamReader(Path.Combine(TestContext.CurrentContext.TestDirectory, "LoginTestData.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        foreach (var record in csv.GetRecords<Credentials>())
        {
            yield return new TestCaseData(record.Username, record.Password)
                .SetName($"Login_WithValidCredentials_ShouldSucceed({record.Username})");
        }
    }

    [TestCaseSource(nameof(GetTestData))]
    public void Login_WithValidCredentials_ShouldSucceed(string username, string password)
    {
        if (!string.Equals(System.Environment.GetEnvironmentVariable("RUN_SELENIUM"), "1", System.StringComparison.Ordinal))
        {
            Assert.Ignore("Set RUN_SELENIUM=1 to enable Selenium tests (requires local Chrome + ChromeDriver).");
        }

        var htmlPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "login.html");
        var url = new System.Uri(htmlPath).AbsoluteUri;

        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");

        using IWebDriver driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl(url);
        driver.FindElement(By.Id("username")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.Id("loginButton")).Click();

        var result = driver.FindElement(By.Id("result")).Text;
        Assert.That(result, Does.Contain("Login successful"));
    }

    private sealed class Credentials
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
