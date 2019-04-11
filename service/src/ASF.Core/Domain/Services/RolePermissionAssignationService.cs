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
        public async Task<Result<Role>> Assignation(int roleId, List<string> pids)
        {
            //获取角色数据
            var role = await _roleRepository.GetAsync(roleId);
            return await this.Assignation(role, pids);
        }

        public async Task<Result<Role>> Assignation(Role role, List<string> pids)
        {
            if (role == null)
                return Result<Role>.ReFailure(ResultCodes.RoleNotExist);
            if (pids.Count == 0)
                return Result<Role>.ReFailure(ResultCodes.RolePermissionAssignationFailed);

            //获取所有的权限
            var permissions = await _permissionRepository.GetList(pids);
            if (permissions.Count != pids.Count)
                return Result<Role>.ReFailure(ResultCodes.RolePermissionAssignationFailed);

            foreach (var permission in permissions)
            {
                if (!permission.IsNormal())
                    return Result<Role>.ReFailure(ResultCodes.PermissionUnavailable.ToFormat($"{permission.Id}【{permission.Name}】"));

                //如果是操作权限，检查是否赋值菜单权限
                if (permission.Type == Values.PermissionType.Action)
                {
                    string parentId = permission.ParentId;
                    while (true)
                    {
                        await Task.Delay(1);
                        //如果没有父级停止循环
                        if (string.IsNullOrEmpty(parentId))
                            break;

                        var parent = await _permissionRepository.GetAsync(parentId);
                        if (parent == null)
                            break;
                        if (!pids.Contains(parentId))
                            pids.Add(parentId);
                        parentId = parent.ParentId;
                    }
                }
            }
            role.SetPermissions(pids);
            return Result<Role>.ReSuccess(role);

        }
    }
}
