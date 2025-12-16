using CalculatorApp;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace CalculatorAppTests;

public class CalculatorCsvTests
{
    public static IEnumerable<object[]> GetTestData(string fileName)
    {
        using var reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, fileName));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        foreach (var record in csv.GetRecords<TestData>())
        {
            yield return new object[] { record.A, record.B, record.Expected };
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData), parameters: "add_testdata.csv")]
    public void Add_CsvData_ReturnsCorrectSum(int a, int b, double expected)
    {
        var calculator = new Calculator();
        var result = calculator.Add(a, b);
        Assert.Equal(expected, (double)result);
    }

    [Theory]
    [MemberData(nameof(GetTestData), parameters: "subtract_testdata.csv")]
    public void Subtract_CsvData_ReturnsCorrectDifference(int a, int b, double expected)
    {
        var calculator = new Calculator();
        var result = calculator.Subtract(a, b);
        Assert.Equal(expected, (double)result);
    }

    [Theory]
    [MemberData(nameof(GetTestData), parameters: "multiply_testdata.csv")]
    public void Multiply_CsvData_ReturnsCorrectProduct(int a, int b, double expected)
    {
        var calculator = new Calculator();
        var result = calculator.Multiply(a, b);
        Assert.Equal(expected, (double)result);
    }

    [Theory]
    [MemberData(nameof(GetTestData), parameters: "divide_testdata.csv")]
    public void Divide_CsvData_ReturnsCorrectQuotient(int a, int b, double expected)
    {
        var calculator = new Calculator();
        var result = calculator.Divide(a, b);
        Assert.Equal(expected, result);
    }

    public sealed class TestData
    {
        public int A { get; set; }
        public int B { get; set; }
        public double Expected { get; set; }
    }
}
