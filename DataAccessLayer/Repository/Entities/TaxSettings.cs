namespace DataAccessLayer.Repository.Entities
{
    public class TaxSettings
    {
        public decimal IncomeTaxPercentage { get; set; }
        public decimal SocialContributionTaxPercentage { get; set; }
        public decimal MinimumMoneyUnitsThatTaxIsApplied { get; set; }
        public decimal MaximumMoneyUnitsThatSocialContributionTaxIsApplied { get; set; }
        public decimal CharityAllowedDeductionPercentage { get; set; }
    }
}
