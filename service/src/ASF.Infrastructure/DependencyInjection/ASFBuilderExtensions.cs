using ASF;
using ASF.DependencyInjection;
using ASF.Infrastructure.Anticorrsives;
using ASF.Infrastructure.DependencyInjection;
using ASF.Infrastructure.Repositories;
using ASF.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ASFBuilderExtensions
    {
        public static ASFBuilder AddSQLite(this ASFBuilder builder,string dbConnectionString)
        {
            var options = builder.Services.GetDefaultConfiguration();
            options.DbConnectionString = dbConnectionString;
            builder.Services.AddRepositories(options);
            builder.Services.AddAnticorrsives(options);
            return builder;
        }

        public static ASFBuilder AddSQLite(this ASFBuilder builder, Action<ASFOptions> startupAction)
        {
            ASFOptions options = new ASFOptions();
            startupAction?.Invoke(options);
            builder.Services.AddRepositories(options);
            builder.Services.AddAnticorrsives(options);
            return builder;
        }
        public static ASFBuilder AddSQLiteCache(this ASFBuilder builder, string dbConnectionString)
        {
            var options = builder.Services.GetDefaultConfiguration();
            options.DbConnectionString = dbConnectionString;
            builder.Services.AddAnticorrsives(options);
            builder.AddRepositoriesCache(options);
            return builder;
        }

        public static ASFBuilder AddSQLiteCache(this ASFBuilder builder, Action<ASFOptions> startupAction)
        {
            ASFOptions options = new ASFOptions();
            startupAction?.Invoke(options);
            builder.Services.AddAnticorrsives(options);
            builder.AddRepositoriesCache(options);
            return builder;
        }
        /// <summary>
        /// 注入仓储层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddRepositories(this IServiceCollection services, ASFOptions options)
        {
           
            services.AddDbContext<RepositoryContext>(opt => opt.UseSqlite(options.DbConnectionString), ServiceLifetime.Scoped);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILoggingRepository, LogInfoRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
        /// <summary>
        /// 注入缓存仓储层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddRepositoriesCache(this ASFBuilder builder, ASFOptions options)
        {
            builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseSqlite(options.DbConnectionString), ServiceLifetime.Scoped);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ILoggingRepository, LogInfoRepository>();
            builder.AddAccountRepositoryCache<AccountRepository>();
            builder.AddPermissionRepositoryCache<PermissionRepository>();
            builder.AddRoleRepositoryCache<RoleRepository>();
        }
        /// <summary>
        /// 注入防腐层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddAnticorrsives(this IServiceCollection services, ASFOptions options)
        {
            services.AddSingleton<IAccessTokenGenerate>(new AccessTokenGenerate(options));
            services.AddAuthenticationJwtBearer(options);
        }
        /// <summary>
        /// 添加Jwt授权
        /// </summary>
        /// <param name="builder"></param>
        private static void AddAuthenticationJwtBearer(this IServiceCollection services, ASFOptions options)
        {
            var tokenOpt = options.JwtToken;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenOpt.SigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = tokenOpt.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenOpt.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = tokenOpt.DefaultScheme;
            })
            .AddJwtBearer(tokenOpt.DefaultScheme, opt =>
            {
                opt.RequireHttpsMetadata = tokenOpt.RequireHttpsMetadata;
                opt.TokenValidationParameters = tokenValidationParameters;
            });
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static ASFOptions GetDefaultConfiguration(this IServiceCollection services)
        {
            var configuration = services
                   .SingleOrDefault(s => s.ServiceType.Name == typeof(IConfiguration).Name)?.ImplementationInstance as IConfiguration;
            if (configuration == null)
            {
                throw new ApplicationException("can't find JwtAuthorize section in appsetting.json");
            }
            ASFOptions options = configuration.GetSection("ASFOptions").Get<ASFOptions>();
            if (options == null)
            {
                options = new ASFOptions();
            }
            return options;
        }
    }
}
