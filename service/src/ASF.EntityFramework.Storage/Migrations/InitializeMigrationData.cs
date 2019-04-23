using ASF.Domain.Values;
using ASF.EntityFramework.Model;
using ASF.EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASF.EntityFramework.Migrations
{
    /// <summary>
    /// 初始化数据类
    /// </summary>
    public class InitializeMigrationData
    {
        private readonly IServiceProvider serviceProvider;
        private readonly RepositoryContext context;

        public InitializeMigrationData(IServiceProvider serviceProvider, RepositoryContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.context = dbContext;
        }

        public void Migration()
        {
            this.AccountMigration();
            this.PermissionMigration();

        }


        private void PermissionMigration()
        {
            if (context.Permissions.Any())
                return;

            List<PermissionModel> list = new List<PermissionModel>();
            list.Add(new PermissionModel() { Id = "asf", Code = "asf", ParentId="", Name = "控制面板", Type = PermissionType.Menu, Description = "控制面板", IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account", Code = "account", ParentId = "asf", Name = "管理员列表", Description = "管理员列表", Type = PermissionType.Menu, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_query", Code = "query", ParentId = "asf_account", Name = "查看", Description = "查看管理员账户列表", ApiTemplate = "/api/asf/account/getlist", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_create", Code = "create", ParentId = "asf_account", Name = "新增", Description = "创建管理账户", ApiTemplate = "/api/asf/account/create", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_delete", Code = "delete", ParentId = "asf_account", Name = "删除", Description = "删除管理账户", ApiTemplate = "/api/asf/account/delete/[0-9]{1,9}", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_details", Code = "details", ParentId = "asf_account", Name = "详情", Description = "管理员账户详情", ApiTemplate = "/api/asf/account/getdetails/[0-9]{1,9}", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_modify", Code = "modify", ParentId = "asf_account", Name = "修改", Description = "修改管理账户信息", ApiTemplate = "/api/asf/account/midify", Type = PermissionType.Action, IsLogger = true,  IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_modify_status", Code = "modify_status", ParentId = "asf_account", Name = "修改状态", Description = "修改管理账户状态", ApiTemplate = "/api/asf/account/midifystatus", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_account_reset_password", Code = "reset_password", ParentId = "asf_account", Name = "重置密码", Description = "重置管理账号密码", ApiTemplate = "/api/asf/account/resetpassword", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });

            list.Add(new PermissionModel() { Id = "asf_audit", Code = "audit", ParentId = "asf", Name = "审计管理", Description = "管理日志包含操作日志和权限日志", Type = PermissionType.Menu, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_audit_query", Code = "query", ParentId = "asf_audit", Name = "查看", Description = "审计日志列表", ApiTemplate = "/api/asf/logger/getlist", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_audit_delete", Code = "delete", ParentId = "asf_audit", Name = "批量删除", Description = "批量删除审计日志", ApiTemplate = "/api/asf/logger/delete", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });

            list.Add(new PermissionModel() { Id = "asf_permission", Code = "permission", ParentId = "asf", Name = "权限管理", Description = "权限列表", Type = PermissionType.Menu, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_action_details", Code = "action_details", ParentId = "asf_permission", Name = "功能详情", Description = "功能权限详情", ApiTemplate = "/api/asf/permission/getactiondetails/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_create_action", Code = "create_action", ParentId = "asf_permission", Name = "新增功能", Description = "创建功能权限", ApiTemplate = "/api/asf/permission/createaction", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_create_menu", Code = "create_menu", ParentId = "asf_permission", Name = "新增导航", Description = "创建导航权限", ApiTemplate = "/api/asf/permission/createmenu", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_delete", Code = "delete", ParentId = "asf_permission", Name = "删除", Description = "删除功能/导航权限", ApiTemplate = "/api/asf/permission/delete/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_menu_details", Code = "menu_details", ParentId = "asf_permission", Name = "导航详情", Description = "导航权限详情", ApiTemplate = "/api/asf/permission/getmenudetails/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_modify_action", Code = "modify_action", ParentId = "asf_permission", Name = "修改功能", Description = "修改功能权限", ApiTemplate = "/api/asf/permission/modifyaction", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_modify_menu", Code = "modify_menu", ParentId = "asf_permission", Name = "修改导航", Description = "修改导航权限", ApiTemplate = "/api/asf/permission/modifymenu", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_modify_sort", Code = "modify_sort", ParentId = "asf_permission", Name = "修改排序", Description = "修改功能/导航权限显示排序", ApiTemplate = "/api/asf/permission/modifysort", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_query_action", Code = "query_action", ParentId = "asf_permission", Name = "功能列表", Description = "查看功能权限列表", ApiTemplate = "/api/asf/permission/getactionlist", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_permission_query_menu", Code = "query_menu", ParentId = "asf_permission", Name = "查看导航", Description = "查看导航权限列表", ApiTemplate = "/api/asf/permission/getmenulist", Type = PermissionType.Action, IsSystem = true, Enable = true });

            list.Add(new PermissionModel() { Id = "asf_publicapi", Code = "publicapi", ParentId = "asf", Name = "公共 API", Description = "管理公共API，只要有登录权限就可以访问，不受角色控制。", Type = PermissionType.Menu, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_publicapi_query", Code = "query", ParentId = "asf_publicapi", Name = "查看", Description = "公共API列表", ApiTemplate = "/api/asf/permission/getopenapilist", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_publicapi_create", Code = "create", ParentId = "asf_publicapi", Name = "新增", Description = "新增公共API", ApiTemplate = "/api/asf/permission/createopenapi", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_publicapi_delete", Code = "delete", ParentId = "asf_publicapi", Name = "删除", Description = "删除公共 API", ApiTemplate = "/api/asf/permission/delete/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_publicapi_modify", Code = "modify", ParentId = "asf_publicapi", Name = "修改", Description = "修改公共API", ApiTemplate = "/api/asf/permission/modifyopenapi", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });

            list.Add(new PermissionModel() { Id = "asf_role", Code = "role", ParentId = "asf", Name = "角色管理", Description = "角色列表", Type = PermissionType.Menu, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_query", Code = "query", ParentId = "asf_role", Name = "查看", Description = "角色列表", ApiTemplate = "/api/asf/role/getlist", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_create", Code = "create", ParentId = "asf_role", Name = "新增", Description = "创建角色", ApiTemplate = "/api/asf/role/create", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_delete", Code = "delete", ParentId = "asf_role", Name = "删除", Description = "删除角色", ApiTemplate = "/api/asf/role/delete/[0-9]{1,9}", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_details", Code = "details", ParentId = "asf_role", Name = "详情", Description = "角色详情", ApiTemplate = "/api/asf/role/getdetails/[0-9]{1,9}", Type = PermissionType.Action, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_modify", Code = "modify", ParentId = "asf_role", Name = "修改", Description = "修改角色", ApiTemplate = "/api/asf/role/modify", Type = PermissionType.Action, IsLogger = true, IsSystem = true, Enable = true });
            list.Add(new PermissionModel() { Id = "asf_role_modify_status", Code = "modify_status", ParentId = "asf_role", Name = "修改状态", Description = "修改角色状态", ApiTemplate = "/api/asf/role/modifystatus", IsLogger = true, Type = PermissionType.Action, IsSystem = true, Enable = true });
            context.Permissions.AddRange(list);
            context.SaveChanges();
        }

        private void AccountMigration()
        {
            if (context.Accounts.Any())
                return;

            AccountModel account = new AccountModel()
            {
                Name = "超级管理员",
                Username = "admin",
                Password = "21232f297a57a5a743894a0e4a801fc3",
                Avatar = "/avatar.jpg",
                Status = Domain.Values.AccountStatus.Normal,
                Roles = "999999999"
            };
            context.Accounts.Add(account);
            context.SaveChanges();
        }

    }
}
