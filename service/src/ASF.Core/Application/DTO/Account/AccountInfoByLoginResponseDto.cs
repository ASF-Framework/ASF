using ASF.Domain.Entities;
using ASF.Domain.Values;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户登录之后返回信息
    /// </summary>
    public class AccountInfoByLoginResponseDto : IDto
    {
        private IList<Permission> Permissions;
        public AccountInfoByLoginResponseDto(Account account)
        {
            this.Avatar = account.Avatar;
            this.Name = account.Name;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; private set; }

        /// <summary>
        /// 可访问的菜单集合
        /// </summary>
        public List<PermissionInfo> Menus { get; private set; } = new List<PermissionInfo>();


        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="permissions"></param>
        public void SetMenus(IList<Permission> permissions)
        {
            this.Permissions = permissions;
            this.Menus = FilterSubMenus("");
        }

        private List<PermissionInfo> FilterSubMenus(string parentId)
        {
            return this.Permissions.Where(f => f.IsNormal() && f.Type == PermissionType.Menu && f.ParentId == parentId)
               .Select(p =>
               {
                   var permissionInfo = new AccountInfoByLoginResponseDto.PermissionInfo(p);
                   permissionInfo.SubMenus = this.FilterSubMenus(p.Id);
                   //查询菜单下的功能
                   this.Permissions.Where(f => f.Type == PermissionType.Action && f.IsNormal() && f.ParentId == p.Id)
                      .ToList()
                      .ForEach(a =>
                      {
                          permissionInfo.Actions.Add(a.Code);
                      });
                   return permissionInfo;
               }).ToList();
        }
        public class PermissionInfo
        {
            public PermissionInfo(Permission permission)
            {
                this.Key = permission.Id;
                this.Name = permission.Name;
                this.Template = permission.ApiTemplate;
                this.Redirect = permission.MenuRedirect;
                this.Icon = permission.MenuIcon;
            }
            /// <summary>
            /// 权限ID
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// 权限名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 页面模板
            /// </summary>
            public string Template { get; set; }
            /// <summary>
            /// 菜单重定向
            /// </summary>
            public string Redirect { get; set; }
            /// <summary>
            /// 菜单图标
            /// </summary>
            public string Icon { get; set; }
            /// <summary>
            /// 可访问的子菜单集合
            /// </summary>
            public List<PermissionInfo> SubMenus { get; set; } = new List<PermissionInfo>();
            /// <summary>
            /// 操作集合
            /// </summary>
            public List<string> Actions { get; private set; } = new List<string>();
        }



    }

}
