using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 删除权限服务
    /// </summary>
    public class PermissionDeleteService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionDeleteService(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionId">权限标识</param>
        /// <returns></returns>
        public async Task<Result> Delete(string permissionId)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(permissionId);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //系统权限不能进行删除
            if (permission.IsSystem)
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotDelete.ToFormat(permission.Name));

            return Result.ReSuccess();
        }
    }
}
