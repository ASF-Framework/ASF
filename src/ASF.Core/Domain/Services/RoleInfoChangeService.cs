using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 角色信息修改
    /// </summary>
    public class RoleInfoChangeService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IServiceProvider _serviceProvider;
        public RoleInfoChangeService(IRoleRepository roleRepository, IServiceProvider serviceProvider)
        {
            this._roleRepository = roleRepository;
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleId">角色标识</param>
        /// <param name="name">角色昵称</param>
        /// <param name="enable">是否可用</param>
        /// <param name="description">角色描述</param>
        /// <returns></returns>
        public Result<Role> Modify(int roleId, string name, bool enable, string description)
        {
            //获取角色数据
            var role = _roleRepository.GetAsync(roleId).GetAwaiter().GetResult();
            if (role == null)
                return Result<Role>.ReFailure(ResultCodes.RoleNotExist);

            role.Name = name;
            role.Enable = enable;
            role.Description = description;

            return role.Valid<Role>();
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleId">角色标识</param>
        /// <param name="name">角色昵称</param>
        /// <param name="enable">是否可用</param>
        /// <param name="description">角色描述</param>
        /// <param name="pids">分配的角色</param>
        /// <returns></returns>
        public Result<Role> Modify(int roleId, string name, bool enable, string description, IList<string> pids)
        {
            var result = this.Modify(roleId, name, enable, description);
            if (!result.Success)
                return result;

            //分配角色
            return this._serviceProvider.GetRequiredService<RolePermissionAssignationService>()
                .Assignation(result.Data, pids)
                .GetAwaiter().GetResult();
        }
    }
}
