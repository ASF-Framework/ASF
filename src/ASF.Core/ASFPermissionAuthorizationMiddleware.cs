using ASF.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.Authorisation;
using Ocelot.Middleware;
using System;
using System.Threading.Tasks;

namespace ASF
{
    public class ASFPermissionAuthorizationMiddleware
    {
        public static async Task Invoke(DownstreamContext context, Func<Task> next)
        {
            var httpContext = context.HttpContext;
            var _serviceProvider = httpContext.RequestServices;
            var _logger = _serviceProvider.GetRequiredService<ILogger<ASFPermissionAuthorizationMiddleware>>();

            //验证登陆用户是否有权限
            var result = await _serviceProvider.GetRequiredService<AccountAuthorizationService>().Authentication();
            var requestPath = httpContext.Request.PathBase + httpContext.Request.Path;
            if (result.Success)
            {
                _logger.LogInformation($"{requestPath} Permission authorization success");
                httpContext.Items.Add("asf_parmission", result.Data);
                await next.Invoke();
                return;
            }
            else
            {
                context.Errors.Add(new UnauthorisedError($"UnAuthorised"));
                _logger.LogWarning($"{requestPath} Permission authorization failed");
            }
        }
    }
}
