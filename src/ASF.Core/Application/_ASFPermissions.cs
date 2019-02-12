using System;

namespace ASF.Application
{
    /// <summary>
    /// ASF 权限集
    /// </summary>
    public class ASFPermissions
    {
        public static ValueTuple<string, string> AccountCreate = ("ASF.Acount.Create", "创建管理账户");
        public static ValueTuple<string, string> AccountDelete = ("ASF.Acount.Delete", "删除管理账户");
        public static ValueTuple<string, string> AccountModifyStatus = ("ASF.Acount.ModifyStatus ", "修改管理账户状态");
        public static ValueTuple<string, string> AccountModifyInfo = ("ASF.Acount.ModifyInfo", "修改管理账户信息");
        public static ValueTuple<string, string> AccountModifyPassword = ("ASF.Acount.ModifyPassword", "修改管理账号密码");
        public static ValueTuple<string, string> AccountSetPassword = ("ASF.Acount.SetPassword", "设置管理账号密码");
        public static ValueTuple<string, string> AccountModifyEmail = ("ASF.Acount.ModifyEmail", "修改管理账号邮箱");
        public static ValueTuple<string, string> AccountModifyTelephone = ("ASF.Acount.ModifyTelephone", "修改管理账号电话号码");

        public static ValueTuple<string, string> RoleCreate = ("ASF.Role.Create", "创建角色");
        public static ValueTuple<string, string> RoleDelete = ("ASF.Role.Delete", "删除角色");
        public static ValueTuple<string, string> RoleModify = ("ASF.Role.Modify", "修改角色");
        public static ValueTuple<string, string> RoleModifyStatus = ("ASF.Role.ModifyStatus", "修改角色状态");

        public static ValueTuple<string, string> PermissionCreateMenu = ("ASF.Permission.CreateMenu", "创建导航权限");
        public static ValueTuple<string, string> PermissionModifyMenu = ("ASF.Permission.ModifyMenu", "修改导航权限");
        public static ValueTuple<string, string> PermissionCreateAction = ("ASF.Permission.CreateAction", "创建操作权限");
        public static ValueTuple<string, string> PermissionModifyAction = ("ASF.Permission.ModifyAction", "修改操作权限");
        public static ValueTuple<string, string> PermissionDelete = ("ASF.Permission.Delete", "删除权限");



    }
}
