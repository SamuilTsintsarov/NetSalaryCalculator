namespace CalculatorTests.CalculatorServiceTests
{
    using BusinessLogicLayer.Services.Contracts;
    using System;
    using Xunit;
    using Assert = Xunit.Assert;

    public class CalculateCharityGiveawayBonusTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculateCharityGiveawayBonusTests(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [Theory]
        [InlineData(0, 15, 100)]
        [InlineData(-1, 15, 100)]
        [InlineData(1000, -1, 100)]
        [InlineData(1000, 15, 1001)]
        public void TestCalculateCharityGiveawayBonusErrorHandling(decimal grossIncome, decimal charityDeductionPercentage, decimal charitySpent)
        {
            Assert.Throws<ArgumentException>(() => _calculatorService.CalculateCharityGiveawayBonus(grossIncome, charityDeductionPercentage, charitySpent));
        }

        [Theory]
        [InlineData(1000, 15, 0, 0)]
        [InlineData(1000, 10, 99, 99)]
        [InlineData(1000, 10, 100, 100)]
        [InlineData(1000, 10, 101, 100)]
        [InlineData(1000, 15, 66.65, 66.65)]
        [InlineData(1000, 15, 15, 15)]
        public void TestCalculateCharityGiveawayBonusEdgeCaseCalculation(decimal grossIncome, decimal charityDeductionPercentage, decimal charitySpent, decimal expectedResult)
        {
            var bonus = _calculatorService.CalculateCharityGiveawayBonus(grossIncome, charityDeductionPercentage, charitySpent);

            Assert.Equal(expectedResult, bonus);
        }
    }
}
