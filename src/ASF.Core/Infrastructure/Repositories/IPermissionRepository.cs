using ASF.Application.DTO;
using ASF.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public interface IPermissionRepository : IRepository<Permission, string>
    {
        /// <summary>
        /// 根据Api地址获取权限
        /// </summary>
        /// <param name="apiAddress"></param>
        /// <returns></returns>
        Task<Permission> GetByApiAddress(string apiAddress);
        /// <summary>
        /// 这标识是否被使用
        /// </summary>
        /// <param name="id">权限标识</param>
        /// <returns></returns>
        Task<bool> HasById(string id);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <returns></returns>
        Task ModifyAsync(Permission permission);
        /// <summary>
        /// 根据ID获取权限集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetList(IList<string> ids);
        /// <summary>
        /// 分页获取角色集合
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IList<Permission>> GetList(PermissionInfoListRequestDto requestDto);
    }
}
