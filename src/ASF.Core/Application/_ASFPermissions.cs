using System;

namespace ASF.Application
{
    /// <summary>
    /// ASF 权限集
    /// </summary>
    public class ASFPermissions
    {
        public static ValueTuple<string, string> AccountList = ("asf.acount", "管理员列表");
        public static ValueTuple<string, string> AccountDetails = ("asf.acount.details", "管理员详情");
        public static ValueTuple<string, string> AccountCreate = ("asf.acount.create", "创建管理账户");
        public static ValueTuple<string, string> AccountDelete = ("asf.acount.delete", "删除管理账户");
        public static ValueTuple<string, string> AccountSetPassword = ("asf.acount.set_password", "设置管理账号密码");
        public static ValueTuple<string, string> AccountModifyStatus = ("asf.acount.modify_status", "修改管理账户状态");
        public static ValueTuple<string, string> AccountModifyInfo = ("asf.acount.modify_info", "修改管理账户信息");
        public static ValueTuple<string, string> AccountModifyPassword = ("asf.acount.modif_password", "修改管理账号密码");
        public static ValueTuple<string, string> AccountModifyEmail = ("asf.acount.modify_email", "修改管理账号邮箱");
        public static ValueTuple<string, string> AccountModifyTelephone = ("asf.acount.modify_telephone", "修改管理账号电话号码");

        public static ValueTuple<string, string> RoleList = ("asf.role", "角色列表");
        public static ValueTuple<string, string> RoleDetails = ("asf.role.details", "角色详情");
        public static ValueTuple<string, string> RoleCreate = ("asf.role.create", "创建角色");
        public static ValueTuple<string, string> RoleDelete = ("asf.role.delete", "删除角色");
        public static ValueTuple<string, string> RoleModify = ("asf.role.modify", "修改角色");
        public static ValueTuple<string, string> RoleModifyStatus = ("asf.role.modify_status", "修改角色状态");

        public static ValueTuple<string, string> PermissionList = ("asf.permission", "权限列表");
        public static ValueTuple<string, string> PermissionMenuDelatils = ("asf.permission.menu_details", "菜单权限详情");
        public static ValueTuple<string, string> PermissionActionDelatils = ("asf.permission.action_details", "功能权限详情");
        public static ValueTuple<string, string> PermissionCreateMenu = ("asf.permission.create_menu", "创建菜单权限");
        public static ValueTuple<string, string> PermissionCreateAction = ("asf.permission.create_action", "创建功能权限");
        public static ValueTuple<string, string> PermissionModifyMenu = ("asf.permission.modify_menu", "修改菜单权限");
        public static ValueTuple<string, string> PermissionModifyAction = ("asf.permission.modify_action", "修改功能权限");
        public static ValueTuple<string, string> PermissionDelete = ("asf.permission.delete", "删除权限");
        public static ValueTuple<string, string> PermissionModifySort = ("asf.permission.sort", "修改权限排序");

        public static ValueTuple<string, string> LoggingDelete = ("asf.logging.delete", "删除日志");

    }
}
