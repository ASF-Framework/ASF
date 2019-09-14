using ASF.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;

namespace ASF.Web.Migrations
{
    public class MigrationsDbContext : RepositoryContext
    {
        public MigrationsDbContext() : base(new DbContextOptions<RepositoryContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=AppData/ASF.db");
            optionsBuilder.UseMySql("server=120.25.226.107;port=30006;database=asf;userid=root;password=rootzop112233");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
