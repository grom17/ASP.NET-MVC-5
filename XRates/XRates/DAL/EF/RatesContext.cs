using System.Data.Entity;
using XRates.DAL.EF.Entities;

namespace XRates.DAL.EF
{
    public class RatesContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Rate> Rates { get; set; }

        public RatesContext(string connectionString) : base(connectionString)
        {
        }
    }
}