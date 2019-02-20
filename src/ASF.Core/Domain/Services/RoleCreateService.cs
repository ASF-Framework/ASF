using ASF.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 角色创建服务
    /// </summary>
    public class RoleCreateService
    {
        private readonly IServiceProvider _serviceProvider;
        public RoleCreateService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <param name="description">结算描述</param>
        /// <param name="createOfAccountId">创建的账户标识</param>
        /// <returns></returns>
        public Result<Role> Create(string name, string description, int createOfAccountId)
        {
            //创建管理员不能为空
            if (createOfAccountId == 0)
                return Result<Role>.ReFailure(ResultCodes.RoleCreateFailed);

            var role = new Role(name, description, createOfAccountId);

            //验证角色聚合的数据合法性
            return role.Valid<Role>();
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <param name="description">结算描述</param>
        /// <param name="createOfAccountId">创建的账户标识</param>
        /// <param name="pids">分配的角色</param>
        /// <returns></returns>
        public async Task<Result<Role>> Create(string name, string description, int createOfAccountId, IList<string> pids)
        {
            var result = this.Create(name, description, createOfAccountId);
            if (!result.Success)
                return result;

            //分配角色
            var role = result.Data;
            return await _serviceProvider.GetRequiredService<RolePermissionAssignationService>().Assignation(role, pids);
         
        }
    }
}
