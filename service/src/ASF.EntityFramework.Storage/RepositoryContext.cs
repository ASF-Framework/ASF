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
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<LogInfoModel> LogInfos { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

   
    }
}
