using ASF.Domain.Services;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ASF.DependencyInjection
{
    /// <summary>
    /// ASF 建造器
    /// </summary>
    public class ASFBuilder
    {
        public IServiceCollection Services { get; }

        public ASFBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        public void Build()
        {
            Services.AddAutoMapper();
            Services.AddMemoryCache();

            this.AddDomainServices();
            this.AddAuthorization();
        }
        /// <summary>
        /// 添加授权
        /// </summary>
        private void AddAuthorization()
        {
            this.Services.AddSingleton<IAuthorizationHandler, ASFPermissionAuthorizationHandler>();
            this.Services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = new AuthorizationPolicyBuilder().AddRequirements(new OperationAuthorizationRequirement()).Build();
            });
        }
        /// <summary>
        /// 添加领域服务
        /// </summary>
        private void AddDomainServices()
        {
            Services.AddTransient<AccountCreateService>();
            Services.AddTransient<AccountAuthorizationService>();
            Services.AddTransient<AccountEmailChangeService>();
            Services.AddTransient<AccountInfoChangeService>();
            Services.AddTransient<AccountLoginService>();
            Services.AddTransient<AccountPasswordChangeService>();
            Services.AddTransient<AccountRoleAssignationService>();
            Services.AddTransient<AccountPermissionService>();
            Services.AddTransient<AccountTelephoneChangeService>();
            Services.AddTransient<AccountDeleteService>();
            Services.AddTransient<LogLoginRecordService>();
            Services.AddTransient<LogOperateRecordService>();
            Services.AddTransient<PermissionCreateService>();
            Services.AddTransient<PermissionChangeService>();
            Services.AddTransient<PermissionDeleteService>();
            Services.AddTransient<RoleCreateService>();
            Services.AddTransient<RoleInfoChangeService>();
            Services.AddTransient<RolePermissionAssignationService>();
        }

        /// <summary>
        /// 添加账户仓储缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ASFBuilder AddAccountRepositoryCache<T>()
            where T : IAccountRepository
        {
            Services.AddTransient(typeof(T));
            Services.AddTransient<IAccountRepository, CachingAccountRepository<T>>();
            return this;
        }
        /// <summary>
        /// 添加权限仓储缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ASFBuilder AddPermissionRepositoryCache<T>()
            where T : IPermissionRepository
        {
            Services.AddTransient(typeof(T));
            Services.AddTransient<IPermissionRepository, CachingPermissionRepository<T>>();
            return this;
        }
        /// <summary>
        /// 添加角色仓储缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ASFBuilder AddRoleRepositoryCache<T>()
            where T : IRoleRepository
        {
            Services.AddTransient(typeof(T));
            Services.AddTransient<IRoleRepository, CachingRoleRepository<T>>();
            return this;
        }

       
    }
}
