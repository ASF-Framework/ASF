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
            list.Add(new PermissionModel() { ParentId = "", Code = "asf", Name = "控制面板", Type = PermissionType.Menu, Description = "控制面板", ApiTemplate = "PageView", MenuIcon = "table" });
            list.Add(new PermissionModel() { ParentId = "asf", Code = "audit", Name = "审计管理", Description = "管理日志包含操作日志和权限日志", Type = PermissionType.Menu });
            list.Add(new PermissionModel() { ParentId = "asf", Code = "account", Name = "管理员列表", Description = "管理员列表", Type = PermissionType.Menu });
            list.Add(new PermissionModel() { ParentId = "asf", Code = "menu", Name = "菜单管理", Description = "菜单管理列表", Type = PermissionType.Menu });
            list.Add(new PermissionModel() { ParentId = "asf", Code = "publicapi", Name = "公共 API", Description = "管理公共API，只要有登录权限就可以访问，不受角色控制。", Type = PermissionType.Menu });
            list.Add(new PermissionModel() { ParentId = "asf", Code = "role", Name = "角色管理", Description = "角色列表", Type = PermissionType.Menu });
            list.Add(new PermissionModel() { ParentId = "asf_menu", Code = "list", Name = "菜单列表", Description = "菜单列表", Type = PermissionType.Menu, MenuHidden = true });
            list.Add(new PermissionModel() { ParentId = "asf_menu", Code = "details", Name = "菜单详情", Description = "菜单详情", Type = PermissionType.Menu, MenuHidden = true });


            // 初始账户权限数据
            list.Add(new PermissionModel() { Code = "query", Name = "查看", Description = "查看管理员账户列表", ApiTemplate = "/account/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "create", Name = "新增", Description = "创建管理账户", ApiTemplate = "/account/create", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "delete", Name = "删除", Description = "删除管理账户", ApiTemplate = "/account/delete/[0-9]{1,9}", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "details", Name = "详情", Description = "管理员账户详情", ApiTemplate = "/account/getdetails/[0-9]{1,9}", Type = PermissionType.Action, HttpMethods = "GET" });
            list.Add(new PermissionModel() { Code = "modify", Name = "修改", Description = "修改管理账户信息", ApiTemplate = "/account/midify", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "modify_status", Name = "修改状态", Description = "修改管理账户状态", ApiTemplate = "/account/midifystatus", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "reset_password", Name = "重置密码", Description = "重置管理账号密码", ApiTemplate = "/account/resetpassword", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_account" : f.ParentId);

            //初始角色权限数据
            list.Add(new PermissionModel() { Code = "query", Name = "查看", Description = "角色列表", ApiTemplate = "/role/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "create", Name = "新增", Description = "创建角色", ApiTemplate = "/role/create", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "delete", Name = "删除", Description = "删除角色", ApiTemplate = "/role/delete/[0-9]{1,9}", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "details", Name = "详情", Description = "角色详情", ApiTemplate = "/role/getdetails/[0-9]{1,9}", Type = PermissionType.Action, HttpMethods = "GET" });
            list.Add(new PermissionModel() { Code = "modify", Name = "修改", Description = "修改角色", ApiTemplate = "/role/modify", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "modify_status", Name = "修改状态", Description = "修改角色状态", ApiTemplate = "/role/modifystatus", IsLogger = true, Type = PermissionType.Action, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_role" : f.ParentId);

            // 初始审计权限数据
            list.Add(new PermissionModel() { Code = "query", Name = "查看", Description = "审计日志列表", ApiTemplate = "/logger/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "delete", Name = "批量删除", Description = "批量删除审计日志", ApiTemplate = "/logger/delete", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_audit" : f.ParentId);

            // 初始开放性API权限数据
            list.Add(new PermissionModel() { Code = "query", Name = "查看", Description = "公共API列表", ApiTemplate = "/permission/openapi/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "create", Name = "新增", Description = "新增公共API", ApiTemplate = "/permission/openapi/create", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "modify", Name = "修改", Description = "修改公共API", ApiTemplate = "/permission/openapi/modify", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "import", Name = "导入", Description = "导入公共API权限", ApiTemplate = "/permission/openapi/import", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "delete", Name = "删除", Description = "删除公共 API", ApiTemplate = "/permission/delete/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_publicapi" : f.ParentId);

            // 初始化菜单权限数据
            list.Add(new PermissionModel() { Code = "menu_query", Name = "查看菜单", Description = "查看菜单权限列表", ApiTemplate = "/permission/menu/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
         
            list.Add(new PermissionModel() { Code = "menu_create", Name = "新增导航", Description = "创建导菜单权限", ApiTemplate = "/permission/menu/create", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "menu_details", Name = "菜单详情", Description = "菜单权限详情", ApiTemplate = "/permission/menu/getdetails/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, HttpMethods = "GET" });
            list.Add(new PermissionModel() { Code = "menu_modify", Name = "修改菜单", Description = "修改菜单权限", ApiTemplate = "/permission/menu/modify", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "menu_delete", Name = "删除菜单", Description = "删除菜单权限", ApiTemplate = "/permission/menu/delete/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "menu_modify_sort", Name = "修改排序", Description = "修改菜单权限顺序", ApiTemplate = "/permission/menu/modifysort", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "export", Name = "导出", Description = "导出菜单权限", ApiTemplate = "/permission/menu/export", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "import", Name = "导入", Description = "导入菜单权限", ApiTemplate = "/permission/menu/import", Type = PermissionType.Action, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_menu_list" : f.ParentId);

            list.Add(new PermissionModel() { Code = "action_details", Name = "功能详情", Description = "功能权限详情", ApiTemplate = "/permission/action/getdetails/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, HttpMethods = "GET" });
            list.Add(new PermissionModel() { Code = "action_create", Name = "新增功能", Description = "创建功能权限", ApiTemplate = "/permission/action/create", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "action_query", Name = "功能列表", Description = "查看功能权限列表", ApiTemplate = "/permission/action/getlist", Type = PermissionType.Action, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "action_modify", Name = "修改功能", Description = "修改功能权限", ApiTemplate = "/permission/action/modify", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "action_delete", Name = "删除功能", Description = "删除功能权限", ApiTemplate = "/permission/action/delete/[a-zA-Z0-9_]{1,100}", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.Add(new PermissionModel() { Code = "action_modify_sort", Name = "修改排序", Description = "修改功能权限顺序", ApiTemplate = "/permission/action/modifysort", Type = PermissionType.Action, IsLogger = true, HttpMethods = "POST" });
            list.ForEach(f => f.ParentId = f.ParentId == null ? "asf_menu_details" : f.ParentId);

            //全部设置为系统权限
            list.ForEach(f =>
            {
                f.IsSystem = true;
                f.Enable = true;
                f.Id = (f.ParentId.Any() ? f.ParentId + "_" : "") + f.Code;
                f.ApiTemplate = !string.IsNullOrEmpty(f.ApiTemplate) && f.Type == PermissionType.Action ? "/api/asf" + f.ApiTemplate : f.ApiTemplate;
            });
            context.Permissions.AddRange(list);
            context.SaveChanges();
        }

        private void AccountMigration()
        {
            if (context.Accounts.Any())
                return;

            AccountModel account = new AccountModel()
            {
                Id=1,
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
