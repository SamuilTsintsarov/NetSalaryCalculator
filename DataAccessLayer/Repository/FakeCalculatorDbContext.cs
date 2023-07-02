namespace DataAccessLayer.Repository
{
    using DataAccessLayer.Repository.Entities;
    using System.Collections.Generic;


    public class FakeCalculatorDbContext: IFakeCalculatorDbContext
    {
        // Should be DBSet, just quickly mock some really fake database
        public IList<TaxSettings> TaxSettings { get; set; }

        public FakeCalculatorDbContext()
        {
            // add silly data seeding
            TaxSettings = new List<TaxSettings>
            {
                new TaxSettings
                {
                    IncomeTaxPercentage = 10,
                    SocialContributionTaxPercentage = 15,
                    CharityAllowedDeductionPercentage = 10,
                    MinimumMoneyUnitsThatTaxIsApplied = 1000,
                    MaximumMoneyUnitsThatSocialContributionTaxIsApplied = 3000,  
                }
            };   
        }
    }
}
