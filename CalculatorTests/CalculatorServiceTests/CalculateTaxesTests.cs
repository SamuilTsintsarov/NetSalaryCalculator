namespace CalculatorTests.CalculatorServiceTests
{
    using BusinessLogicLayer.Models;
    using BusinessLogicLayer.Services.Contracts;
    using System;
    using Xunit;
    using Assert = Xunit.Assert;

    public class CalculateTaxesTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculateTaxesTests(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [Theory]
        [InlineData(-1, 15, 10, 1000, 3000)]
        [InlineData(101, 15, 10, 1000, 3000)]
        [InlineData(10, -1, 10, 1000, 3000)]
        [InlineData(10, 101, 10, 1000, 3000)]
        [InlineData(10, 15, -1, 1000, 3000)]
        [InlineData(10, 15, 101, 1000, 3000)]
        [InlineData(10, 15, 10, -1, 3000)]
        [InlineData(10, 15, 10, 1000, -1)]
        [InlineData(10, 15, 10, 1000, 1000)]
        [InlineData(10, 15, 10, 1000, 999)]
        public void TestCalculateTaxesTestsSystemExceptionHandling(decimal incomeTaxPercentage, decimal socialContributionTaxPercentage,
            decimal charityAllowedDeductionPercentage, decimal minimumMoneyUnitsThatTaxIsApplied, decimal maximumMoneyUnitsThatSocialContributionTaxIsApplied)
        {
            var taxPayer = new TaxPayer { CharitySpent = 100, FullName = "Ivan Ivanov", GrossIncome = 1000, SSN = 1 };

            var taxSettings = new TaxSettingsModel
            {
                IncomeTaxPercentage = incomeTaxPercentage,
                SocialContributionTaxPercentage = socialContributionTaxPercentage,
                CharityAllowedDeductionPercentage = charityAllowedDeductionPercentage,
                MinimumMoneyUnitsThatTaxIsApplied = minimumMoneyUnitsThatTaxIsApplied,
                MaximumMoneyUnitsThatSocialContributionTaxIsApplied = maximumMoneyUnitsThatSocialContributionTaxIsApplied
            };

            Assert.Throws<Exception>(() => _calculatorService.CalculateTaxes(taxPayer, taxSettings));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(1000, 1001)]
        public void TestCalculateTaxesTestsArgumentExceptionHandling(decimal grossIncome, decimal charitySpent )
        {
            var taxPayer = new TaxPayer { CharitySpent = charitySpent, FullName = "Ivan Ivanov", GrossIncome = grossIncome, SSN = 1 };

            var taxSettings = new TaxSettingsModel
            {
                IncomeTaxPercentage = 10,
                SocialContributionTaxPercentage = 15,
                CharityAllowedDeductionPercentage = 10,
                MinimumMoneyUnitsThatTaxIsApplied = 1000,
                MaximumMoneyUnitsThatSocialContributionTaxIsApplied = 3000
            };

            Assert.Throws<ArgumentException>(() => _calculatorService.CalculateTaxes(taxPayer, taxSettings));
        }

        [Fact]
        public void TestCalculateTaxes()
        {
            var taxPayer = new TaxPayer { CharitySpent = 654.123M, FullName = "Samuil Tsintsarov", GrossIncome = 10000, SSN = 1 };

            var taxSettings = new TaxSettingsModel
            {
                IncomeTaxPercentage = 10,
                SocialContributionTaxPercentage = 15,
                CharityAllowedDeductionPercentage = 10,
                MinimumMoneyUnitsThatTaxIsApplied = 1000,
                MaximumMoneyUnitsThatSocialContributionTaxIsApplied = 3000
            };

            var taxes = _calculatorService.CalculateTaxes(taxPayer, taxSettings);

            Assert.Equal(300, taxes.SocialTax);
            Assert.Equal(654.123M, taxes.CharitySpent);
            Assert.Equal(10000M, taxes.GrossIncome);
            Assert.Equal(834.58770M, taxes.IncomeTax);
            Assert.Equal(1134.58770M, taxes.TotalTax);
            Assert.Equal(8865.41230M, taxes.NetIncome);
        }
    }
}
