using ASF.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace ASF.Infrastructure.Repository
{
    public class RepositoryContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LogInfoModel> LogInfos { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

   
    }
}
