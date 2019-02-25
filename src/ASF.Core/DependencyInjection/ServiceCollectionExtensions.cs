using ASF.DependencyInjection;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加添加ASF核心服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="startupAction">ASF启动配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddASFCore(this IServiceCollection services, Action<ASFBuilder> startupAction)
        {
            var assembly = typeof(ASFBuilder).GetTypeInfo().Assembly;
            services.AddMvcCore()
                    .AddApplicationPart(assembly)
                    .AddControllersAsServices()
                    .AddAuthorization()
                    .AddJsonFormatters()
                    .AddApiExplorer();

            ASFBuilder builder = new ASFBuilder(services);
            startupAction?.Invoke(builder);
            builder.Build();
            return services;
        }

      

    }
}
