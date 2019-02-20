using ASF.Application.DTO;
using ASF.Core.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace ASF.Core.Test.Application
{
    /// <summary>
    /// 权限范围测试
    /// </summary>
    public class PermissionControllerTests : WebHostTest
    {
        [Fact(DisplayName = "创建操作权限")]
        public void CreateAction()
        {
            var data = new
            {
                Code = "test",
                ParentId = "asf.account",
                Name = "测试",
                ApiTemplate = "/api/asf/test/get",
                IsLogger = false,
                Description = "测试测试"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/CreateAction")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "创建导航权限")]
        public void CreateMenu()
        {
            var data = new
            {
                Code = "test2",
                ParentId = "asf.account",
                Name = "测试",
                Description = "测试测试"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/CreateMenu")
             .And(r => r.Authorization("Bearer", token).ContextJson(data))
             .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改操作权限")]
        public void ModifyAction()
        {
            var data = new
            {
                Id = "asf.account.test",
                ParentId = "asf.account",
                Name = "测试2",
                ApiTemplate = "/api/asf/test/getlist",
                IsLogger = true,
                Description = "测试测试List",
                Enable = true
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/ModifyAction")
             .And(r => r.Authorization("Bearer", token).ContextJson(data))
             .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改导航权限")]
        public void ModifyMenu()
        {
            var data = new
            {
                Id = "asf.account.test2",
                ParentId = "asf.account",
                Name = "测试4",
                ApiTemplate = "/api/asf/test/getlist",
                Sort = 2,
                Description = "测试测试List",
                Enable = true
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/ModifyMenu")
             .And(r => r.Authorization("Bearer", token).ContextJson(data))
             .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "删除权限")]
        public void Delete()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/Delete/asf.account.test")
             .And(r => r.Authorization("Bearer", token))
             .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "获取导航权限集合")]
        public void GetMenuList()
        {
            var data = new
            {
                Vague= "asf.account",
                Enable=-1
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/GetMenuList")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultList<PermissionMenuInfoDetailsResponseDto>>();
            Assert.True(result.Success);
            Assert.True(result.Data.Count>0);
        }
        [Fact(DisplayName = "获取所有导航权限集合")]
        public void GetMenuAllList()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/GetMenuAllList")
                .And(r => r.Authorization("Bearer", token))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultList<PermissionMenuInfoBaseResponseDto>>();
            Assert.True(result.Success);
            Assert.True(result.Data.Count > 0);
        }
        [Fact(DisplayName = "根据ID获取导航权限详情")]
        public void GetMenuDetails()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/GetMenuDetails/asf.account")
                .And(r => r.Authorization("Bearer", token))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result<PermissionMenuInfoDetailsResponseDto>>();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
        [Fact(DisplayName = "获取操作权限集合")]
        public void GetActionList()
        {
            var data = new
            {
                Enable = -1,
                ParamId= "asf.account"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/GetActionList")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultList<PermissionActionInfoDetailsResponseDto>>();
            Assert.True(result.Success);
            Assert.True(result.Data.Count > 0);
        }
        [Fact(DisplayName = "根据ID获取操作权限详情")]
        public void GetActionDetails()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/permission/GetActionDetails/asf.account.query")
                .And(r => r.Authorization("Bearer", token))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result<PermissionActionInfoDetailsResponseDto>>();
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
