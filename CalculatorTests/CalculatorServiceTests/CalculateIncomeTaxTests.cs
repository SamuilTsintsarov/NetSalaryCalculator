namespace CalculatorTests.CalculatorServiceTests
{
    using BusinessLogicLayer.Services.Contracts;
    using System;
    using Xunit;
    using Assert = Xunit.Assert;

    public class CalculateIncomeTaxTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculateIncomeTaxTests(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [Theory]
        [InlineData(0, 10, 1000)]
        [InlineData(-1, 10, 1000)]
        [InlineData(1000, -1, 1000)]
        [InlineData(1000, 101, 1000)]
        [InlineData(100, 10, -1)]
        public void TestCalculateIncomeTaxErrorHandling(decimal grossIncome, decimal incomeTaxPercentage, decimal minimumMoneyUnitsThatTaxIsApplied)
        {
            Assert.Throws<ArgumentException>(() => _calculatorService.CalculateIncomeTax(grossIncome, incomeTaxPercentage, minimumMoneyUnitsThatTaxIsApplied));
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(999)]
        [InlineData(0.1)]
        public void TestCalculateIncomeTaxEdgeCaseCalculation(decimal grossIncome)
        {
            var tax = _calculatorService.CalculateIncomeTax(grossIncome, 10, 1000);

            Assert.Equal(0, tax);
        }

        [Theory]
        [InlineData(3400, 240)]
        [InlineData(1200, 20)]
        [InlineData(5441.6546579876541654777654, 444.1654657987650)]
        public void TestCalculateIncomeTaxCalculation(decimal grossIncome, decimal expectedResult)
        {
            var tax = _calculatorService.CalculateIncomeTax(grossIncome, 10, 1000);

            Assert.Equal(expectedResult, tax);
        }
    }
}
