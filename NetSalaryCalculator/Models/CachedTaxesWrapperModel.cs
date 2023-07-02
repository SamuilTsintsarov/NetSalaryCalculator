namespace NetSalaryCalculator.Models
{
    using BusinessLogicLayer.Models;
    using BusinessLogicLayer.Models.Contracts;

    public class CachedTaxesWrapperModel
    {
        public TaxSettingsModel TaxSettings { get; set; }
        public ITaxes Taxes { get; set; }
    }
}
