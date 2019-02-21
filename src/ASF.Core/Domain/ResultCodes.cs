using System;

namespace ASF.Domain
{
    /// <summary>
    /// 错误结果代码
    /// </summary>
    public class ResultCodes : BaseResultCodes
    {
        //账户错误码
        public static ValueTuple<int, string> AccountUsernameExist = (10001, "输入的用户名已经被使用，请重新输入");
        public static ValueTuple<int, string> AccountOldPasswordAndNewPasswordSame = (10002, "新密码和旧密码不能一样，为了安全请重新输入");
        public static ValueTuple<int, string> AccountTelephoneExist = (10003, "输入的手机号码已经被使用，请重新输入");
        public static ValueTuple<int, string> AccountNotExist = (10004, "用户不存在");
        public static ValueTuple<int, string> AccountEmailExist = (10006, "输入的邮箱地址已经被使用，请重新输入");
        public static ValueTuple<int, string> AccountRoleAssignationFailed = (10007, "账户角色分配失败，请重试");
        public static ValueTuple<int, string> AccountNotAllowedLogin = (10008, "账户被禁止登陆，请联系管理员");
        public static ValueTuple<int, string> AccountUnavailable = (10009, "账户暂不可以使用");
        public static ValueTuple<int, string> AccountPasswordNotSame = (10010, "账户密码不匹配");
        public static ValueTuple<int, string> AccountPasswordNotSame2 = (10012, "账户密码不匹配，失败{0}次之后自动锁定账号");
        public static ValueTuple<int, string> AccountPasswordNotSameOverrun = (10012, "登录失败超限，请30分钟之后再试");
        public static ValueTuple<int, string> AccountResetPasswordPasswordNotSame = (10013, "重置密码失败，管理员密码不匹配");
        public static ValueTuple<int, string> AccountSuperAdminNoAllowedModify = (10014, "超级管理员不能修改");
        public static ValueTuple<int, string> AccountSuperAdminNoAllowedDelete = (10014, "超级管理员不能删除");


        //角色错误码
        public static ValueTuple<int, string> RoleNotExist = (11000, "角色不存在");
        public static ValueTuple<int, string> RoleUnavailable = (11001, "{0}角色已禁止使用");
        public static ValueTuple<int, string> RoleCreateFailed = (11003, "创建角色失败");
        public static ValueTuple<int, string> RolePermissionAssignationFailed = (11004, "角色权限分配失败，请重试");

        //权限错误码
        public static ValueTuple<int, string> PermissionNotExist = (12000, "权限不存在");
        public static ValueTuple<int, string> PermissionUnavailable = (12001, "{0}权限已禁止使用");
        public static ValueTuple<int, string> PermissionIdExist = (12002, "{0}权限标识已经被使用，请重新输入");
        public static ValueTuple<int, string> PermissionSystemNotModify = (12003, "{0}权限为系统权限无法修改");
        public static ValueTuple<int, string> PermissionSystemNotDelete = (12004, "{0}权限为系统权限不能删除，否则系统无法正常运行");
        public static ValueTuple<int, string> PermissionParemtNotAction = (12005, "上级权限不能设置为操作权限");

        //日志记录
        public static ValueTuple<int, string> LogginDeletedCannoBeWithinThreeMonths = (13000, "三个月之内的日志不能删除");

    }
}
