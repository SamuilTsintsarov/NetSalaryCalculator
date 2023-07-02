namespace BusinessLogicLayer.Services.Contracts
{
    using BusinessLogicLayer.Models;
    using BusinessLogicLayer.Models.Contracts;
    using System.Threading.Tasks;

    public interface ICalculatorService
    {
        Task<TaxSettingsModel> GetTaxSettings();
        ITaxes CalculateTaxes(ITaxPayer taxPayer, TaxSettingsModel taxSettings);
        decimal CalculateIncomeTax(decimal grossIncome, decimal incomeTaxPercentage, decimal minimumMoneyUnitsThatTaxIsApplied);
        decimal CalculateSocialContributionTax
            (decimal grossIncome, decimal socialContributionTaxPercentage,
            decimal minimumMoneyUnitsThatTaxIsApplied, decimal maximumMoneyUnitsThatSocialContributionTaxIsApplied);
        decimal CalculateCharityGiveawayBonus(decimal grossIncome, decimal charityDeductionPercentage, decimal? charitySpent);
    }
}
