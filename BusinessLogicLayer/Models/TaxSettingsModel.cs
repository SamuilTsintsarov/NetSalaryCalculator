namespace BusinessLogicLayer.Models
{
    public class TaxSettingsModel
    {
        public decimal IncomeTaxPercentage { get; set; }
        public decimal SocialContributionTaxPercentage { get; set; }
        public decimal CharityAllowedDeductionPercentage { get; set; }
        public decimal MinimumMoneyUnitsThatTaxIsApplied { get; set; }
        public decimal MaximumMoneyUnitsThatSocialContributionTaxIsApplied { get; set; }
        public bool IsValid => !(IncomeTaxPercentage < 0 || IncomeTaxPercentage > 100 || SocialContributionTaxPercentage < 0 || SocialContributionTaxPercentage > 100
                                || CharityAllowedDeductionPercentage < 0 || CharityAllowedDeductionPercentage > 100
                                || MinimumMoneyUnitsThatTaxIsApplied < 0 || MaximumMoneyUnitsThatSocialContributionTaxIsApplied < 0
                                || MinimumMoneyUnitsThatTaxIsApplied >= MaximumMoneyUnitsThatSocialContributionTaxIsApplied);

        public override bool Equals(object obj)
        {            
            if (obj == null)
            {
                return false;
            }
         
            if (!(obj is TaxSettingsModel))
            {
                return false;
            }

            var comparedTaxSettingsModel = (TaxSettingsModel)obj;

            return (IncomeTaxPercentage == comparedTaxSettingsModel.IncomeTaxPercentage)
                && (CharityAllowedDeductionPercentage == comparedTaxSettingsModel.CharityAllowedDeductionPercentage)
                && (SocialContributionTaxPercentage == comparedTaxSettingsModel.SocialContributionTaxPercentage)
                && (MinimumMoneyUnitsThatTaxIsApplied == comparedTaxSettingsModel.MinimumMoneyUnitsThatTaxIsApplied)
                && (MaximumMoneyUnitsThatSocialContributionTaxIsApplied == comparedTaxSettingsModel.MaximumMoneyUnitsThatSocialContributionTaxIsApplied);
        }

        public static bool operator ==(TaxSettingsModel taxSettings1, TaxSettingsModel taxSettings2)
        {
            return taxSettings1.Equals(taxSettings2);
        }

        public static bool operator !=(TaxSettingsModel taxSettings1, TaxSettingsModel taxSettings2)
        {
            return !taxSettings1.Equals(taxSettings2);
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", 
                IncomeTaxPercentage, SocialContributionTaxPercentage, MinimumMoneyUnitsThatTaxIsApplied,
                MaximumMoneyUnitsThatSocialContributionTaxIsApplied, CharityAllowedDeductionPercentage)
                .GetHashCode();
        }
    }
}
