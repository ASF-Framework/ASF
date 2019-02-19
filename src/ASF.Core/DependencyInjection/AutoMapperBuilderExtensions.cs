using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// AutoMapper 建造控制类
    /// </summary>
    public static class AutoMapperBuilderExtensions
    {
        private static object lockObj = new object();
        /// <summary>
        /// 添加AutoMapper服务,自动加载所有继承Profile配置,配置可见性
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            //AutoMapper 配置服务
            lock (lockObj)
            {
                Mapper.Reset();
                Mapper.Initialize(config =>
                {
                    config.ShouldMapProperty = p => p.GetMethod.IsPrivate || p.GetMethod.IsPublic || p.GetMethod.IsConstructor || p.GetMethod.IsAssembly || p.GetMethod.IsFamily;
                    config.RegisterAllMappings();
                });
                return services;

            }

        }

        /// <summary>
        /// 添加AutoMapper服务,自动加载所有继承Profile配置
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configAction">配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction)
        {
            lock (lockObj)
            {
                //AutoMapper 配置服务
                Mapper.Reset();
                Mapper.Initialize(config =>
                {
                    configAction?.Invoke(config);
                });
                return services;
            }
        }


        /// <summary>
        /// 注入程序集Mappings
        /// </summary>
        /// <param name="config"><see cref="IMapperConfigurationExpression"/></param>
        /// <returns></returns>
        public static void RegisterAllMappings(this IMapperConfigurationExpression config)
        {
            //获取所有的程序集
            config.RegisterAllMappings(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// 注入程序集Mappings
        /// </summary>
        /// <param name="config"><see cref="IMapperConfigurationExpression"/></param>
        /// <returns></returns>
        public static void RegisterAllMappings(this IMapperConfigurationExpression config, Assembly assembly)
        {
            //获取所有的程序集
            List<AssemblyName> assemblys = assembly
                  .GetReferencedAssemblies().ToList();//获取所有引用程序集
            assemblys.Add(assembly.GetName());

            //获取所有IProfile实现类
            IEnumerable<Type> allType = assemblys
                   .Select(f => Assembly.Load(f))
                   .SelectMany(y => y.ExportedTypes)
                   .Where(type => typeof(Profile).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract && type.Module.Name != "AutoMapper.dll")
                   .ToList();

            foreach (var type in allType)
            {
                //注册映射
                config.AddProfiles(type); // Initialise each Profile classe
            }
        }


    }
}
