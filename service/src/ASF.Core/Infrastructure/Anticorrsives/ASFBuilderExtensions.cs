using ASF.DependencyInjection;
using ASF.Infrastructure.Anticorrsives;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ASFBuilderExtensions
    {
        /// <summary>
        /// 添加Jwt授权
        /// </summary>
        /// <param name="build"><see cref="ASFBuilder"/></param>
        public static ASFBuilder AddAuthenticationJwtBearer(this ASFBuilder build)
        {
          return   build.AddAuthenticationJwtBearer(new TokenOptions());
        }
        /// <summary>
        /// 添加Jwt授权
        /// </summary>
        /// <param name="build"><see cref="ASFBuilder"/></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ASFBuilder AddAuthenticationJwtBearer(this ASFBuilder build, Action<TokenOptions> action)
        {
            TokenOptions options = new TokenOptions();
            action?.Invoke(options);
            return build.AddAuthenticationJwtBearer(options);
        }

        /// <summary>
        /// 添加Jwt授权
        /// </summary>
        /// <param name="build"><see cref="ASFBuilder"/></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ASFBuilder AddAuthenticationJwtBearer(this ASFBuilder build, IConfiguration configuration)
        {
            TokenOptions options = configuration.Get<TokenOptions>();
            return build.AddAuthenticationJwtBearer(options);
        }

        public static ASFBuilder AddAuthenticationJwtBearer(this ASFBuilder build, TokenOptions options)
        {
            build.Services.AddSingleton<IAccessTokenGenerate>(new AccessTokenGenerate(options));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.SigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            build.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = options.DefaultScheme;
            })
            .AddJwtBearer(options.DefaultScheme, opt =>
            {
                opt.RequireHttpsMetadata = options.RequireHttpsMetadata;
                opt.TokenValidationParameters = tokenValidationParameters;
            });

            return build;
        }

    }
}
