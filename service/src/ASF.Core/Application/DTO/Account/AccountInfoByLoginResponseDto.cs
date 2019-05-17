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
               .OrderBy(f => f.Sort)
               .Select(p =>
               {
                   var permissionInfo = new AccountInfoByLoginResponseDto.PermissionInfo(p);
                   permissionInfo.Children = this.FilterSubMenus(p.Id);
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
                this.Name = permission.Id;
                this.Title = permission.Name;
                this.Redirect = permission.MenuRedirect;
                this.Icon = permission.MenuIcon;
                this.Hidden = permission.MenuHidden;
            }
            /// <summary>
            /// 菜单名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 菜单标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 菜单重定向
            /// </summary>
            public string Redirect { get; set; }
            /// <summary>
            /// 菜单图标
            /// </summary>
            public string Icon { get; set; }
            /// <summary>
            /// 是否隐藏
            /// </summary>
            public bool Hidden { get; set; }
            /// <summary>
            /// 可访问的子菜单集合
            /// </summary>
            public List<PermissionInfo> Children { get; set; } = new List<PermissionInfo>();
            /// <summary>
            /// 操作集合
            /// </summary>
            public List<string> Actions { get; private set; } = new List<string>();
        }



    }

}
