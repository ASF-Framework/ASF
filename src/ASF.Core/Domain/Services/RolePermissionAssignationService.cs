using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 角色权限分配服务
    /// </summary>
    public class RolePermissionAssignationService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        public RolePermissionAssignationService(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            this._roleRepository = roleRepository;
            this._permissionRepository = permissionRepository;
        }
        public async Task<Result<Role>> Assignation(int roleId, IList<string> pids)
        {
            //获取角色数据
            var role = await _roleRepository.GetAsync(roleId);
            return await this.Assignation(role, pids);
        }

        public async Task<Result<Role>> Assignation(Role role, IList<string> pids)
        {
            if (role == null)
                return Result<Role>.ReFailure(ResultCodes.RoleNotExist);

            //获取所有的权限
            var permissions = await _permissionRepository.GetList(pids);
            if (permissions.Count != pids.Count)
                return Result<Role>.ReFailure(ResultCodes.RolePermissionAssignationFailed);
            foreach (var permission in permissions)
            {
                if (!permission.IsNormal())
                    return Result<Role>.ReFailure(ResultCodes.PermissionUnavailable.ToFormat($"{permission.Id}【{permission.Name}】"));
            }

            role.SetPermissions(pids);
            return Result<Role>.ReSuccess(role);

        }
    }
}
