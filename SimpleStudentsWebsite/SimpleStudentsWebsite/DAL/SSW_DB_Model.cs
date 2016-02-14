using System.Data.Entity;
namespace SimpleStudentsWebsite.DAL
{
    public partial class SSW_DB_Model : DbContext
    {
        public SSW_DB_Model()
            : base("name=SSW_DB")
        {
        }

        public virtual DbSet<Deans> Deans { get; set; }
        public virtual DbSet<Journal> Journal { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
