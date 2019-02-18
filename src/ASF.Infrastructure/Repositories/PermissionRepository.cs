using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASF.Application.DTO;
using ASF.Domain.Entities;

namespace ASF.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        static IList<Permission> permissions = new List<Permission>();
        static PermissionRepository()
        {
            permissions.Add(new Permission("dashboard", "", "仪表盘", Domain.Values.PermissionType.Menu, "仪表盘"));
            permissions.Add(new Permission("add", "dashboard", "新增", Domain.Values.PermissionType.Action, "新增"));
            permissions.Add(new Permission("query", "dashboard", "查询", Domain.Values.PermissionType.Action, "查询"));
            permissions.Add(new Permission("get", "dashboard", "详情", Domain.Values.PermissionType.Action, "详情"));
            permissions.Add(new Permission("update", "dashboard", "修改", Domain.Values.PermissionType.Action, "修改"));
            permissions.Add(new Permission("delete", "dashboard", "删除", Domain.Values.PermissionType.Action, "删除"));

            permissions.Add(new Permission("exception", "", "异常页面权限", Domain.Values.PermissionType.Menu, "异常页面权限"));
            permissions.Add(new Permission("result", "", "结果权限", Domain.Values.PermissionType.Menu, "结果权限"));
            permissions.Add(new Permission("permission", "", "权限管理", Domain.Values.PermissionType.Menu, "权限管理"));
            permissions.Add(new Permission("role", "", "角色管理", Domain.Values.PermissionType.Menu, "角色管理"));

            permissions.Add(new Permission("user", "", "用户管理", Domain.Values.PermissionType.Menu, "用户管理"));
            permissions.Add(new Permission("query", "user", "查询", Domain.Values.PermissionType.Action, "查询"));
        }
        public Task<Permission> AddAsync(Permission entity)
        {
            throw new NotImplementedException();
        }

        public Task<Permission> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Permission> GetByApiAddress(string apiAddress)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetList(IList<string> ids)
        {
            return Task.FromResult(permissions);
        }

        public Task<IList<Permission>> GetList(PermissionInfoListRequestDto requestDto)
        {
            return Task.FromResult(permissions);
        }

        public Task<bool> HasById(string id)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string primaryKey)
        {
            throw new NotImplementedException();
        }

    
    }
}
