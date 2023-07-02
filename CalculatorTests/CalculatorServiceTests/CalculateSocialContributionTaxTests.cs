namespace CalculatorTests.CalculatorServiceTests
{
    using BusinessLogicLayer.Services.Contracts;
    using System;
    using Xunit;
    using Assert = Xunit.Assert;

    public class CalculateSocialContributionTaxTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculateSocialContributionTaxTests(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [Theory]
        [InlineData(0, 15, 1000, 3000)]
        [InlineData(-1, 15, 1000, 3000)]
        [InlineData(1000, -1, 1000, 3000)]
        [InlineData(1000, 101, 1000, 3000)]
        [InlineData(1000, 15, -1, 3000)]
        [InlineData(1000, 15, 1000, -1)]
        [InlineData(1000, 15, 1000, 999)]
        public void TestCalculateSocialContributionTaxErrorHandling(decimal grossIncome, decimal socialContributionTaxPercentage,
            decimal minimumMoneyUnitsThatTaxIsApplied, decimal maximumMoneyUnitsThatSocialContributionTaxIsApplied)
        {
            Assert.Throws<ArgumentException>(() =>
            _calculatorService.CalculateSocialContributionTax(grossIncome, socialContributionTaxPercentage,
                                                              minimumMoneyUnitsThatTaxIsApplied, maximumMoneyUnitsThatSocialContributionTaxIsApplied));
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(999)]
        [InlineData(0.1)]
        public void TestCalculateSocialContributionTaxEdgeCaseCalculation(decimal grossIncome)
        {
            var tax = _calculatorService.CalculateSocialContributionTax(grossIncome, 15, 1000, 3000);

            Assert.Equal(0, tax);
        }

        [Theory]
        [InlineData(3400, 300)]
        [InlineData(2999, 299.85)]
        [InlineData(3000, 300)]
        [InlineData(3001, 300)]
        [InlineData(1623.34534245463245, 93.5018013681945)]
        [InlineData(1000.1, 0.015)]
        public void TestCalculateSocialContributionTaxCalculation(decimal grossIncome, decimal expectedResult)
        {
            var tax = _calculatorService.CalculateSocialContributionTax(grossIncome, 15, 1000, 3000);

            Assert.Equal(expectedResult, tax);
        }
    }
}
