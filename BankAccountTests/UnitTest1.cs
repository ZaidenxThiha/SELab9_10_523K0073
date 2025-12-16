using BankAccountApp;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace BankAccountTests;

public class BankAccountInlineTests
{
    [Theory]
    [InlineData("John Doe", 1000, 200, 800)]
    [InlineData("Jane Smith", 500, 100, 400)]
    [InlineData("Alice Johnson", 300, 50, 250)]
    [InlineData("Boundary Case", 100, 100, 0)] // BVA: debit == balance
    public void Debit_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, decimal expectedBalance)
    {
        var account = new BankAccount(customerName, initialBalance);
        account.Debit(debitAmount);
        Assert.Equal(expectedBalance, account.Balance);
    }

    [Theory]
    [InlineData("Bob Brown", 100, 150)]
    [InlineData("Boundary Case", 100, 101)] // BVA: debit just above balance
    public void Debit_InsufficientFunds_ThrowsException(string customerName, decimal initialBalance, decimal debitAmount)
    {
        var account = new BankAccount(customerName, initialBalance);
        Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
    }

    [Theory]
    [InlineData("Invalid Debit", 1000, 0)]
    [InlineData("Invalid Debit", 1000, -1)]
    public void Debit_NonPositiveAmount_ThrowsArgumentOutOfRange(string customerName, decimal initialBalance, decimal debitAmount)
    {
        var account = new BankAccount(customerName, initialBalance);
        Assert.Throws<ArgumentOutOfRangeException>(() => account.Debit(debitAmount));
    }

    [Theory]
    [InlineData("John Doe", 1000, 200, 1200)]
    [InlineData("Jane Smith", 500, 100, 600)]
    [InlineData("Alice Johnson", 300, 50, 350)]
    [InlineData("Boundary Case", 0, 1, 1)] // BVA: minimal positive credit
    public void Credit_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal creditAmount, decimal expectedBalance)
    {
        var account = new BankAccount(customerName, initialBalance);
        account.Credit(creditAmount);
        Assert.Equal(expectedBalance, account.Balance);
    }

    [Theory]
    [InlineData("Invalid Credit", 1000, 0)]
    [InlineData("Invalid Credit", 1000, -1)]
    public void Credit_NonPositiveAmount_ThrowsArgumentOutOfRange(string customerName, decimal initialBalance, decimal creditAmount)
    {
        var account = new BankAccount(customerName, initialBalance);
        Assert.Throws<ArgumentOutOfRangeException>(() => account.Credit(creditAmount));
    }

    [Theory]
    [InlineData("John Doe", 1000, 200, 800)]
    [InlineData("Jane Smith", 500, 100, 400)]
    [InlineData("Alice Johnson", 300, 50, 250)]
    public void Withdraw_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal withdrawAmount, decimal expectedBalance)
    {
        var account = new BankAccount(customerName, initialBalance);
        account.Withdraw(withdrawAmount);
        Assert.Equal(expectedBalance, account.Balance);
    }
}

public class BankAccountCsvTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        using var reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, "BankAccountTestData.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        foreach (var record in csv.GetRecords<TestData>())
        {
            yield return new object[]
            {
                record.CustomerName,
                record.InitialBalance,
                record.DebitAmount,
                record.ExpectedBalance
            };
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Debit_CsvData_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, string expectedBalance)
    {
        var account = new BankAccount(customerName, initialBalance);

        if (expectedBalance == "Insufficient funds")
        {
            Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
            return;
        }

        account.Debit(debitAmount);
        Assert.Equal(decimal.Parse(expectedBalance, CultureInfo.InvariantCulture), account.Balance);
    }

    public sealed class TestData
    {
        public string CustomerName { get; set; } = "";
        public decimal InitialBalance { get; set; }
        public decimal DebitAmount { get; set; }
        public string ExpectedBalance { get; set; } = "";
    }
}
