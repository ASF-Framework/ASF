using ASF.Core.Test.Infrastructure;
using ASF.Domain.Values;
using System;
using System.Net.Http;
using Xunit;

namespace ASF.Core.Test.Application
{
    public class AuthoriseControllerTests : WebHostTest
    {
        [Fact]
        public void Login()
        {
            var data = new
            {
                Username,
                Password
            };
            var request = this.Server.CreateRequest("api/asf/authorise/login").And(r => r.ContextJson(data));
            var response = request.PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result<AccessToken>>();

            Assert.True(result.Success);
            Assert.NotEmpty(result.Data.Token);
        }
    }
}
