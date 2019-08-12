using ASF.DependencyInjection;
using Ocelot.DependencyInjection;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加ASF框架
        /// </summary>
        /// <param name="services"></param>
        /// <param name="startupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddASF(this IServiceCollection services, Action<ASFBuilder> startupAction)
        {
            services.AddOcelot()
               .AddASF(build =>
               {
                   startupAction?.Invoke(build);
               });
            return services;
        }
     

        /// <summary>
        /// 添加ASF核心服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="startupAction">ASF启动配置函数</param>
        /// <returns></returns>
        internal static IServiceCollection AddASFCore(this IServiceCollection services, Action<ASFBuilder> startupAction)
        {
            ASFBuilder builder = new ASFBuilder(services);
            startupAction?.Invoke(builder);
            builder.Build();
            return services;
        }

      

    }
}
