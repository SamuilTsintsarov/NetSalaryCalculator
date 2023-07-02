namespace BusinessLogicLayer.Models.Contracts
{
    public interface ITaxes
    {
        decimal GrossIncome { get; set; }
        decimal? CharitySpent { get; set; }
        decimal IncomeTax { get; set; }
        decimal SocialTax { get; set; }
        decimal TotalTax { get; set; }
        decimal NetIncome { get; set; }
    }
}
