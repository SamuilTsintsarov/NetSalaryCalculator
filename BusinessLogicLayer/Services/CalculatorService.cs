namespace BusinessLogicLayer.Services
{
    using AutoMapper;
    using BusinessLogicLayer.Models;
    using BusinessLogicLayer.Models.Contracts;
    using BusinessLogicLayer.Services.Contracts;
    using DataAccessLayer;
    using DataAccessLayer.Repository.Entities;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class CalculatorService : AutoMapperService, ICalculatorService
    {
        private readonly ILogger<CalculatorService> _logger;
        private readonly ICalculatorDAL _calculatorDal;

        public CalculatorService(ICalculatorDAL calculatorDal, ILogger<CalculatorService> logger)
        {
            _calculatorDal = calculatorDal;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the taxes based on the given Tax Payer and Tax Settings
        /// </summary>
        /// <param name="taxPayer"></param>
        /// <param name="taxSettings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Tax Payer Data is invalid</exception>
        /// <exception cref="Exception">Tax Settings Data is corrupted</exception>
        public ITaxes CalculateTaxes(ITaxPayer taxPayer, TaxSettingsModel taxSettings)
        {            
            if (!taxSettings.IsValid)
            {
                // _logger.LogError("CalculateTaxes Failed!", new { TaxPayer = taxPayer, TaxSettings = taxSettings });
                throw new Exception("Tax Settings data is incorrect");
            }

            if (taxPayer.GrossIncome <= 0 || (taxPayer.CharitySpent.HasValue && taxPayer.CharitySpent > taxPayer.GrossIncome))
            {
                // _logger.LogError("CalculateTaxes Failed!", new { TaxPayer = taxPayer, TaxSettings = taxSettings });
                throw new ArgumentException("Tax Payer data is incorrect");
            }

            if (taxPayer.GrossIncome <= taxSettings.MinimumMoneyUnitsThatTaxIsApplied)
            {
                return new Taxes
                {
                    GrossIncome = taxPayer.GrossIncome,
                    CharitySpent = taxPayer.CharitySpent,
                    IncomeTax = 0,
                    SocialTax = 0,
                    TotalTax = 0,
                    NetIncome = taxPayer.GrossIncome
                };
            }

            var charityDeduction = CalculateCharityGiveawayBonus(taxPayer.GrossIncome, taxSettings.CharityAllowedDeductionPercentage, taxPayer.CharitySpent);
            var grossIncomeAfterCharityDeduction = taxPayer.GrossIncome - charityDeduction;

            var incomeTax = CalculateIncomeTax(grossIncomeAfterCharityDeduction, taxSettings.IncomeTaxPercentage, taxSettings.MinimumMoneyUnitsThatTaxIsApplied);
            var socialContributionTax = CalculateSocialContributionTax(grossIncomeAfterCharityDeduction, taxSettings.SocialContributionTaxPercentage,
                taxSettings.MinimumMoneyUnitsThatTaxIsApplied, taxSettings.MaximumMoneyUnitsThatSocialContributionTaxIsApplied);
            var totalTax = socialContributionTax + incomeTax;

            var netIncome = taxPayer.GrossIncome - totalTax;

            return new Taxes
            {
                GrossIncome = taxPayer.GrossIncome,
                CharitySpent = taxPayer.CharitySpent ?? 0,
                IncomeTax = incomeTax,
                SocialTax = socialContributionTax,
                TotalTax = totalTax,
                NetIncome = netIncome
            };
        }

        /// <summary>
        /// Fetches the TaxSettings from the Data Access Layer
        /// </summary>
        /// <returns>TaxSettings object that hold the tax data</returns>
        public async Task<TaxSettingsModel> GetTaxSettings()
        {
            var taxSettings = await _calculatorDal.GetTaxSettings();

            return Mapper.Map<TaxSettings, TaxSettingsModel> (taxSettings);
        }

        /// <summary>
        /// Calculates Charity Gross income deduction
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="charityDeductionPercentage"></param>
        /// <param name="charitySpent"></param>
        /// <returns>Gross Income deduction amount</returns>
        /// <exception cref="ArgumentException"></exception>
        public decimal CalculateCharityGiveawayBonus(decimal grossIncome, decimal charityDeductionPercentage, decimal? charitySpent)
        {
            if (grossIncome <= 0 || charityDeductionPercentage < 0  || (charitySpent.HasValue && charitySpent > grossIncome))
            {
                // _logger.LogError("CalculateCharityGiveawayBonus Failed!", provide the input parameters here);
                throw new ArgumentException("Input data is incorrect");
            }

            if (charitySpent == null)
            {
                return 0;
            }

            var maxBonus = grossIncome / charityDeductionPercentage;

            if (charitySpent >= maxBonus)
            {
                return maxBonus;
            }

            return (decimal)charitySpent;
        }

        /// <summary>
        /// Calculates the Income Tax
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="incomeTaxPercentage"></param>
        /// <param name="minimumMoneyUnitsThatTaxIsApplied"></param>
        /// <returns>Amount of income tax</returns>
        /// <exception cref="ArgumentException"></exception>
        public decimal CalculateIncomeTax(decimal grossIncome, decimal incomeTaxPercentage, decimal minimumMoneyUnitsThatTaxIsApplied)
        {
            if (grossIncome <= 0 || incomeTaxPercentage < 0 || incomeTaxPercentage > 100 || minimumMoneyUnitsThatTaxIsApplied < 0)
            {
                // _logger.LogError("CalculateIncomeTax Failed!", provide the input parameters here);
                throw new ArgumentException("Input data is incorrect");
            }

            if (grossIncome < minimumMoneyUnitsThatTaxIsApplied)
            {
                return 0;
            }

            var tax = (grossIncome - minimumMoneyUnitsThatTaxIsApplied) / 100 * incomeTaxPercentage;

            return tax;
        }

        /// <summary>
        /// Calculates the Social Contribution Tax
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="socialContributionTaxPercentage"></param>
        /// <param name="minimumMoneyUnitsThatTaxIsApplied"></param>
        /// <param name="maximumMoneyUnitsThatSocialContributionTaxIsApplied"></param>
        /// <returns>Social Contribution Tax amount</returns>
        /// <exception cref="ArgumentException"></exception>
        public decimal CalculateSocialContributionTax
            (decimal grossIncome, decimal socialContributionTaxPercentage,
            decimal minimumMoneyUnitsThatTaxIsApplied, decimal maximumMoneyUnitsThatSocialContributionTaxIsApplied)
        {
            if (grossIncome <= 0 || socialContributionTaxPercentage < 0 || socialContributionTaxPercentage > 100
                || minimumMoneyUnitsThatTaxIsApplied < 0 || maximumMoneyUnitsThatSocialContributionTaxIsApplied < 0
                || minimumMoneyUnitsThatTaxIsApplied >= maximumMoneyUnitsThatSocialContributionTaxIsApplied)
            {
                // _logger.LogError("CalculateSocialContributionTax Failed!", provide the input parameters here);
                throw new ArgumentException("Input data is incorrect");
            }

            if (grossIncome < minimumMoneyUnitsThatTaxIsApplied)
            {
                return 0;
            }

            if (grossIncome > maximumMoneyUnitsThatSocialContributionTaxIsApplied)
            {
                return (maximumMoneyUnitsThatSocialContributionTaxIsApplied - minimumMoneyUnitsThatTaxIsApplied) / 100 * socialContributionTaxPercentage;

            }

            return (grossIncome - minimumMoneyUnitsThatTaxIsApplied) / 100 * socialContributionTaxPercentage;
        }
    }
}
