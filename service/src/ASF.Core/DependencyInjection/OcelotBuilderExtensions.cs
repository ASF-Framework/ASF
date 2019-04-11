using ASF;
using ASF.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OcelotBuilderExtensions
    {
        /// <summary>
        /// 添加ASF
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddASF(this IOcelotBuilder builder)
        {
            builder.AddASF(null);
            return builder;
        }
        /// <summary>
        /// 添加ASF
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="startupAction">配置ASF函数</param>
        /// <returns></returns>
        public static IOcelotBuilder AddASF(this IOcelotBuilder builder, Action<ASFBuilder> startupAction)
        {
            builder.MvcCoreBuilder
                .AddApplicationPart(typeof(ASFBuilder).Assembly)
                .AddApiExplorer();
            builder.Services.AddASFCore(startupAction);
            builder.Services.AddSingleton<OcelotMiddlewareConfigurationDelegate>(ASFMiddlewareConfigurationProvider.Get);
            return builder;
        }

        public static OcelotPipelineConfiguration AddASF(this OcelotPipelineConfiguration configuration)
        {
            configuration.AuthorisationMiddleware = ASFPermissionAuthorizationMiddleware.Invoke;
            configuration.PreErrorResponderMiddleware = ASFRequestLoggerMiddleware.Invoke;
            return configuration;
        }
    }
}
