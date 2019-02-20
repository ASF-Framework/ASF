using ASF.Application.DTO;
using ASF.Core.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace ASF.Core.Test.Application
{
    public class RoleControllerTests : WebHostTest
    {
        [Fact(DisplayName = "创建角色")]
        public void Create()
        {
            var data = new
            {
                Name = "超级管理员",
                Description = "超级管理员",
                Permissions = new List<string>
                {
                    "asf.acount","asf.acount.details","asf.acount.create"
                }
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/create")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "删除角色")]
        public void Delete()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/delete/2")
                .And(r => r.Authorization("Bearer", token))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改角色")]
        public void Modify()
        {
            var data = new
            {
                RoleId = 1,
                Name = "超级超级管理",
                Description = "ss",
                Enable = false,
                Permissions = new List<string>
                {
                    "asf.acount","asf.acount.details","asf.acount.create"
                }
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/Modify")
                 .And(r => r.Authorization("Bearer", token).ContextJson(data))
                 .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改角色状态")]
        public void ModifyStatus()
        {
            var data = new
            {
                RoleId = 1,
                Enable = true
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/ModifyStatus")
                 .And(r => r.Authorization("Bearer", token).ContextJson(data))
                 .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "获取所有角色基本信息")]
        public void GetListAll()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/GetListAll")
                .And(r => r.Authorization("Bearer", token))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultList<RoleInfoSimpleResponseDto>>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "获取角色集合")]
        public void GetList()
        {
            var data = new
            {
                PagedCount = 10,
                SkipPage = 1
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/GetList")
                 .And(r => r.Authorization("Bearer", token).ContextJson(data))
                 .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultPagedList<RoleInfoBaseResponseDto>>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "获取角色详情")]
        public void GetDetails()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/role/GetDetails/1")
                 .And(r => r.Authorization("Bearer", token))
                 .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result<RoleInfoDetailsResponseDto>>();
            Assert.True(result.Success);
        }
    }
}
