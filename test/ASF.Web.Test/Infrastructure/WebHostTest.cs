using ASF.Domain.Values;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.Core.Test.Infrastructure
{
    public class WebHostTest
    {
        public static string Username = "admin";
        public static string Password = "e10adc3949ba59abbe56e057f20f883e";
        public WebHostTest()
        {
            try
            {
                this.Server =  new TestServer(WebHost
                    .CreateDefaultBuilder()
                    .UseStartup<Startup>());
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        public TestServer Server { get; }

        /// <summary>
        /// 访问Token
        /// </summary>
        /// <returns></returns>
        public string AccessToken()
        {
            var data = new
            {
                Username = Username,
                Password = Password
            };
            var request = this.Server.CreateRequest("api/asf/authorise/login").And(r => r.ContextJson(data));
            var response = request.PostAsync().GetAwaiter().GetResult();
            var result = response.IsSuccess().GetMessage<Result<AccessToken>>();
            return result.Data.Token;
        }
    }
}
