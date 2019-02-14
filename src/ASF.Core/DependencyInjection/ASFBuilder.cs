using ASF.Domain.Services;
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
            Services.AddTransient<AccountTelephoneChangeService>();
            Services.AddTransient<LogLoginRecordService>();
            Services.AddTransient<LogOperateRecordService>();
            Services.AddTransient<PermissionCreateService>();
            Services.AddTransient<PermissionChangeService>();
            Services.AddTransient<RoleCreateService>();
            Services.AddTransient<RoleInfoChangeService>();
            Services.AddTransient<RolePermissionAssignationService>();
        }
    }
}
