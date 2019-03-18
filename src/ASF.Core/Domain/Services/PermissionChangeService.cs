using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 权限修改服务
    /// </summary>
    public class PermissionChangeService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionChangeService(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 修改功能权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="apiTemplate">api 地址</param>
        /// <param name="isLogger">是否记录日志</param>
        /// <param name="sort">菜单排序</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyAction (string pid, string name, string description, bool enable, string apiTemplate, bool isLogger, int sort)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != Values.PermissionType.Action)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }
            
            permission.Name = name;
            permission.IsLogger = isLogger;
            permission.Enable = enable;
            permission.Description = description;
            permission.SetApiTemplate(apiTemplate);
            permission.Sort = sort;

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }
        /// <summary>
        /// 修改菜单权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="parentId">分类</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="sort">菜单排序</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyMenu(string pid, string name, string parentId,string description,bool enable, int sort)
        { 
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != Values.PermissionType.Menu)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }

            //如果配置上级权限，需要验证上级权限必须为菜单权限
            if (permission.ParentId != parentId)
            {
                //判断上级权限,上级权限不能为菜单权限
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

            permission.Name = name;
            permission.ParentId = parentId;
            permission.Sort = sort;
            permission.Enable = enable;
            permission.Description = description;

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }

        /// <summary>
        /// 修改开放API权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="apiTemplate">api 地址</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyOpenApi(string pid, string name, string description, bool enable, string apiTemplate)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            if (permission.Type != Values.PermissionType.OpenApi)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }
            permission.Name = name;
            permission.Enable = enable;
            permission.Description = description;
            permission.SetApiTemplate(apiTemplate);

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }
     
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifySort(string pid,int sort)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            permission.Sort = sort;
            return Result<Permission>.ReSuccess(permission);
        }
    }
}
