using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 权限创建服务
    /// </summary>
    public class PermissionCreateService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionCreateService(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 创建功能权限
        /// </summary>
        /// <param name="permission">权限对象</param>
        /// <returns></returns>
        public async Task<Result> CreateAction(Permission permission)
        {
            permission.Type = Values.PermissionType.Action;
            //判断权限代码是否被使用
            if (await this._permissionRepository.HasById(permission.Id))
                return Result.ReFailure(ResultCodes.PermissionIdExist.ToFormat(permission.Code));

            //判断上级权限,上级权限不能为功能权限
            var paremt = await this._permissionRepository.GetAsync(permission.ParentId);
            if (paremt == null)
            {
                return Result.ReFailure(ResultCodes.PermissionNotExist);
            }
            if (paremt.Type == Values.PermissionType.Action)
            {
                return Result.ReFailure(ResultCodes.PermissionParemtNotAction);
            }

            //验证权限聚合数据是否合法
            return permission.Valid();
        }

        /// <summary>
        /// 创建菜单权限
        /// </summary>
        /// <param name="permission">权限对象</param>
        /// <returns></returns>
        public async Task<Result> CreateMenu(Permission permission)
        {
            permission.Type = Values.PermissionType.Menu;
            //判断权限代码是否被使用
            if (await this._permissionRepository.HasById(permission.Id))
                return Result.ReFailure(ResultCodes.PermissionIdExist.ToFormat(permission.Code));

            //如果配置上级权限，需要验证上级权限必须为菜单权限
            if (permission.ParentId != permission.ParentId)
            {
                var paremt = await this._permissionRepository.GetAsync(permission.ParentId);
                if (paremt == null)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
                }
                if (paremt.Type == Values.PermissionType.Action)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionParemtNotAction);
                }
            }

            //验证权限聚合数据是否合法
            return permission.Valid();
        }
    }
}
