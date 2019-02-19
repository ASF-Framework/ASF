using ASF.Application.DTO;
using ASF.Core.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace ASF.Core.Test.Application
{
    public class AccountControllerTests : WebHostTest
    {
        [Fact(DisplayName = "修改登录密码")]
        public void ModifyPassword()
        {
            var data = new
            {
                OldPassword = "e10adc3949ba59abbe56e057f20f883e",
                Password = "123456"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/ModifyPassword")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);

            //恢复原密码
            var data1 = new
            {
                OldPassword = "123456",
                Password = "e10adc3949ba59abbe56e057f20f883e"
            };
            this.Server.CreateRequest("api/asf/account/ModifyPassword")
                .And(r => r.Authorization("Bearer", token).ContextJson(data1))
                .PostAsync().GetAwaiter().GetResult();
        }
        [Fact(DisplayName = "修改账户信息")]
        public void Midify()
        {
            var data = new
            {
                Status = 1,
                Name = "超级管理员1"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/Midify")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改邮箱地址")]
        public void ModifyEmail()
        {
            var data = new
            {
                Email = "510415@qq.com",
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/ModifyEmail")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改手机号码")]
        public void ModifyTelephone()
        {
            var data = new
            {
                Number = "18163676912",
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/ModifyTelephone")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "获取账户信息")]
        public void Info()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/Info")
                .And(r => r.Authorization("Bearer", token))
                .GetAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "创建管理账户")]
        public void Create()
        {
            string username = DateTime.Now.ToString("MMDDHHmmss");
            var data = new
            {
                Name = "测试用户",
                Username = "aqatest",
                Password = "123456",
                Roles = new List<int>()
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/Create")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "删除管理账户")]
        public void Delete()
        {
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/Delete/2")
             .And(r => r.Authorization("Bearer", token))
             .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);
        }
        [Fact(DisplayName = "修改账号状态")]
        public void MidifyStatus()
        {
            var data = new
            {
                Id = 1,
                Status = 2
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/MidifyStatus")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);

            //还原配置
            var data1 = new
            {
                Id = 1,
                Status = 1
            };
            this.Server.CreateRequest("api/asf/account/MidifyStatus")
                .And(r => r.Authorization("Bearer", token).ContextJson(data1))
                .PostAsync().GetAwaiter().GetResult();
        }
        [Fact(DisplayName = "设置登录密码")]
        public void ResetPassword()
        {
            var data = new
            {
                Id = 1,
                Password = "123456"
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/ResetPassword")
                .And(r => r.Authorization("Bearer", token).ContextJson(data))
                .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result>();
            Assert.True(result.Success);

            //恢复原密码
            var data1 = new
            {
                Id = 1,
                Password = "e10adc3949ba59abbe56e057f20f883e"
            };
            this.Server.CreateRequest("api/asf/account/ResetPassword")
                .And(r => r.Authorization("Bearer", token).ContextJson(data1))
                .PostAsync().GetAwaiter().GetResult();
        }
        [Fact(DisplayName = "账户列表")]
        public void List()
        {
            var data = new
            {
                Status = -1
            };
            var token = this.AccessToken();
            var response = this.Server.CreateRequest("api/asf/account/List")
              .And(r => r.Authorization("Bearer", token).ContextJson(data))
              .PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<ResultPagedList<AccountInfoResponseDto>>();
            Assert.True(result.Success);
        }
    }
}
