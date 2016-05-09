namespace ProjectsApp.DAL
{
    using System.Data.Entity;

    public partial class ProjectsDB : DbContext
    {
        public ProjectsDB()
            : base("name=ProjectsDB")
        {
        }

        public virtual DbSet<ProjectExecutors> ProjectExecutors { get; set; }
        public virtual DbSet<ProjectInfo> ProjectInfo { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectInfo>()
                .HasMany(e => e.ProjectExecutors)
                .WithRequired(e => e.ProjectInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.ProjectExecutors)
                .WithRequired(e => e.Staff)
                .HasForeignKey(e => e.ProjectExecutorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.ProjectInfo)
                .WithRequired(e => e.Staff)
                .HasForeignKey(e => e.ProjectManagerId)
                .WillCascadeOnDelete(false);
        }
    }
}
