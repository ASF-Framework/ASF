using ASF;
using ASF.DependencyInjection;
using ASF.Domain.Entities;
using ASF.Infrastructure.Anticorrsives;
using ASF.Infrastructure.DependencyInjection;
using ASF.Infrastructure.Repositories;
using ASF.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ASFBuilderExtensions
    {
        public static ASFBuilder AddSQLite(this ASFBuilder builder)
        {
            var configuration = builder.Services
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
            builder.Services.Configure<ASFOptions>(configuration.GetSection("ASFOptions"));

            builder.Services.AddRepositories();
            builder.Services.AddAnticorrsives();
            builder.Services.AddAuthenticationJwtBearer(options);
            return builder;
        }

        public static ASFBuilder AddSQLite(this ASFBuilder builder, Action<ASFOptions> startupAction)
        {
            ASFOptions options = new ASFOptions();
            startupAction?.Invoke(options);
            builder.Services.ConfigureOptions(startupAction);

            builder.Services.AddRepositories();
            builder.Services.AddAnticorrsives();
            builder.Services.AddAuthenticationJwtBearer(options);
            return builder;

        }
        /// <summary>
        /// 注入仓储层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<RepositoryContext>(options => options.UseSqlite("Data Source=ASF.db"), ServiceLifetime.Scoped);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            //services.TryAddScoped(typeof(AccountRepository));
            //services.AddScoped<IAccountRepository, CachingAccount<AccountRepository>>();

            services.AddScoped<ILoggingRepository, LogInfoRepository>();

            //services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.TryAddScoped(typeof(PermissionRepository));
            services.AddScoped<IPermissionRepository, CachingPermission<PermissionRepository>>();

            //services.AddScoped<IRoleRepository, RoleRepository>();
            services.TryAddScoped(typeof(RoleRepository));
            services.AddScoped<IRoleRepository, CachingRole<RoleRepository>>();
        }
        /// <summary>
        /// 注入防腐层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddAnticorrsives(this IServiceCollection services)
        {
            services.AddSingleton<IAccessTokenGenerate, AccessTokenGenerate>();
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
    }
}
