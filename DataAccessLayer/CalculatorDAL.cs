namespace DataAccessLayer
{
    using DataAccessLayer.Repository;
    using DataAccessLayer.Repository.Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public class CalculatorDAL : ICalculatorDAL
    {
        private readonly IFakeCalculatorDbContext _fakeDbContext;

        public CalculatorDAL(IFakeCalculatorDbContext fakeDbContext)
        {
            _fakeDbContext = fakeDbContext;
        }

        /// <summary>
        /// Fetches the TaxSettings from a Database
        /// </summary>
        /// <returns>TaxSettings parameters</returns>
        /// <exception cref="InvalidOperationException">There is no TaxSettings record in the Database, or there are more than one records</exception>
        /// <exception cref="TimeoutException ">Performance issues</exception>
        public async Task<TaxSettings> GetTaxSettings()
        {
            // simulate database query
            var getTaxSettingsFromDatabaseTaskSimulator =
                Task<TaxSettings>.Factory.StartNew(() => _fakeDbContext.TaxSettings.Single());

            return await getTaxSettingsFromDatabaseTaskSimulator;
        }
    }
}
