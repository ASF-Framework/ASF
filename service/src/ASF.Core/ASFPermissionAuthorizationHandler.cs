using ASF.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ASF
{
    public class ASFPermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        public ASFPermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor, ILogger<ASFPermissionAuthorizationHandler> logger,
            IServiceProvider serviceProvider)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                //验证请求Action是角色权限
                if (context.Resource is AuthorizationFilterContext authContext)
                {
                    if (authContext.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
                    {
                        var atts = actionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true);
                        if (atts.Count() > 0)
                        {
                            if (atts.Where(f => f.Roles.ToLower() == "self").Count() > 0)
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }
                    }
                }
                //验证登陆用户是否有权限
                var result = await this._serviceProvider.GetRequiredService<AccountAuthorizationService>().Authentication();
                var requestPath = httpContext.Request.PathBase + httpContext.Request.Path;
                if (result.Success)
                {
                    this._logger.LogInformation($"{requestPath} Permission authorization success");
                    httpContext.Items.Add("asf_parmission", result.Data);
                    context.Succeed(requirement);
                    return ;
                }
                else
                {
                    context.Fail();
                    this._logger.LogWarning($"{requestPath} Permission authorization failed");
                }
            }

            return ;
        }
    }
}
