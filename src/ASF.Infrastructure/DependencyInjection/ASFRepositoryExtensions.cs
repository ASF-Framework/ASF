using ASF.Infrastructure.Repositories;
using ASF.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Infrastructure.DependencyInjection
{
   public static class ASFRepositoryExtensions
    {
        /// <summary>
        /// 注入ASF仓储服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddASFRepository(this IServiceCollection services)
        {
            //services.AddScoped<RepositoryContext>();
            services.AddDbContext<RepositoryContext>(ServiceLifetime.Scoped);
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILoggingRepository, LogInfoRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            return services;
        }
    }
}
