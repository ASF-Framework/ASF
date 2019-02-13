using ASF.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Infrastructure.Repository
{
   public class RepositoryContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LogInfo> LogInfos { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ASF.db");
        }
    }
}
