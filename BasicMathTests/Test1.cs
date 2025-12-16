using BasicMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicMathTests;

[TestClass]
public sealed class BasicMathsTests
{
    [DataTestMethod]
    [DataRow(1, 1, 2d)] // EP: Positive numbers
    [DataRow(-1, -1, -2d)] // EP: Negative numbers
    [DataRow(0, 0, 0d)] // EP: Zero
    [DataRow(int.MaxValue, 1, (double)int.MaxValue + 1)] // BVA: Upper boundary
    [DataRow(int.MinValue, -1, (double)int.MinValue - 1)] // BVA: Lower boundary
    public void Add_MultipleValues_ReturnsExpected(double a, double b, double expected)
    {
        var bm = new BasicMaths();
        var actual = bm.Add(a, b);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(5, 3, 2d)] // EP: Positive numbers
    [DataRow(-5, -3, -2d)] // EP: Negative numbers
    [DataRow(0, 0, 0d)] // EP: Zero
    [DataRow(int.MaxValue, -1, (double)int.MaxValue + 1)] // BVA: Upper boundary
    [DataRow(int.MinValue, 1, (double)int.MinValue - 1)] // BVA: Lower boundary
    public void Subtract_MultipleValues_ReturnsExpected(double a, double b, double expected)
    {
        var bm = new BasicMaths();
        var actual = bm.Subtract(a, b);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(2, 3, 6d)] // EP: Positive numbers
    [DataRow(-2, -3, 6d)] // EP: Negative numbers
    [DataRow(0, 5, 0d)] // EP: Zero
    [DataRow(int.MaxValue, 1, (double)int.MaxValue)] // BVA: Upper boundary
    [DataRow(int.MinValue, 1, (double)int.MinValue)] // BVA: Lower boundary
    public void Multiply_MultipleValues_ReturnsExpected(double a, double b, double expected)
    {
        var bm = new BasicMaths();
        var actual = bm.Multiply(a, b);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(6, 3, 2d)] // EP: Positive numbers
    [DataRow(-6, -3, 2d)] // EP: Negative numbers
    [DataRow(0, 1, 0d)] // EP: Zero numerator
    [DataRow(int.MaxValue, 1, (double)int.MaxValue)] // BVA: Upper boundary
    [DataRow(int.MinValue, 1, (double)int.MinValue)] // BVA: Lower boundary
    public void Divide_MultipleValues_ReturnsExpected(double a, double b, double expected)
    {
        var bm = new BasicMaths();
        var actual = bm.Divide(a, b);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        var bm = new BasicMaths();
        Assert.ThrowsException<DivideByZeroException>(() => bm.Divide(10, 0));
    }
}
