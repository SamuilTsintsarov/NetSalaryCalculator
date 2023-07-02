namespace DataAccessLayer
{
    using DataAccessLayer.Repository.Entities;
    using System.Threading.Tasks;

    public interface ICalculatorDAL
    {
        Task<TaxSettings> GetTaxSettings();
    }
}
