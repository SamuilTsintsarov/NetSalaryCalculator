namespace BusinessLogicLayer.Models.Contracts
{
    public interface ITaxPayer
    {
        string FullName { get; set; }
        ulong SSN { get; set; }
        decimal GrossIncome { get; set; }
        decimal? CharitySpent { get; set; }
    }
}
