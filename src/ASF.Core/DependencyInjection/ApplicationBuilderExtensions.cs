using Microsoft.Extensions.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static Task<IApplicationBuilder> UseASF(this IApplicationBuilder app)
        {
            // Nginx 代理时获取真实 IP
            app.Use((context, next) =>
            {
                var headers = context.Request.Headers;
                try
                {
                    if (headers.ContainsKey("X-Forwarded-For"))
                    {
                        context.Connection.RemoteIpAddress = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',')[0]);
                    }
                }
                finally
                {
                    next().Wait();
                }
                return Task.CompletedTask;
            });

            return app.UseOcelot(opt =>
            {
                opt.UseASF();
            });
        }
    }
}
