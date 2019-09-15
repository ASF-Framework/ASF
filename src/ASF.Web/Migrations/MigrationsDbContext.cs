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
            //optionsBuilder.UseMySql("server=127.0.0.1;port=30006;database=asf;userid=root;password=root");
            //optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=asf;User ID=SA;Password=sa;Trusted_Connection=false;");
            base.OnConfiguring(optionsBuilder);

        }
    }
}
