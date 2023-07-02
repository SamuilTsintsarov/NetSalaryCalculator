using DataAccessLayer.Repository.Entities;
using System.Collections.Generic;

namespace DataAccessLayer.Repository
{
    public interface IFakeCalculatorDbContext
    {
        IList<TaxSettings> TaxSettings { get; set; }
    }
}
