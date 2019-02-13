using ASF.DependencyInjection;
using ASF.Infrastructure.Anticorrsives;
using ASF.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            ASFOptions options = new ASFOptions();
            if (configuration.GetSection("ASFOptions") != null)
            {
                options = configuration.GetSection("ASFOptions").Get<ASFOptions>();
            }
            builder.Services.ConfigureOptions(options);

            builder.AddRepositories();
            builder.AddAnticorrsives();
            builder.AddAuthenticationJwtBearer(options);
            return builder;
        }

        public static ASFBuilder AddSQLite(this ASFBuilder builder, Action<ASFOptions> startupAction)
        {
            ASFOptions options = new ASFOptions();
            startupAction?.Invoke(options);
            builder.Services.ConfigureOptions(options);

            builder.AddRepositories();
            builder.AddAnticorrsives();
            builder.AddAuthenticationJwtBearer(options);
            return builder;

        }
        /// <summary>
        /// 注入仓储层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddRepositories(this ASFBuilder builder)
        {
            var services = builder.Services;
        }
        /// <summary>
        /// 注入防腐层
        /// </summary>
        /// <param name="builder"></param>
        private static void AddAnticorrsives(this ASFBuilder builder)
        {
            var services = builder.Services;
            services.AddSingleton<IAccessTokenGenerate, AccessTokenGenerate>();
        }
        /// <summary>
        /// 添加Jwt授权
        /// </summary>
        /// <param name="builder"></param>
        private static void AddAuthenticationJwtBearer(this ASFBuilder builder, ASFOptions options)
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
            builder.Services.AddAuthentication(opt =>
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
