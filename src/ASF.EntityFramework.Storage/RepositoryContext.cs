using ASF.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;

namespace ASF.EntityFramework.Repository
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<LogInfoModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<PermissionModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<RoleModel>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<LogInfoModel> LogInfos { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

   
    }
}
