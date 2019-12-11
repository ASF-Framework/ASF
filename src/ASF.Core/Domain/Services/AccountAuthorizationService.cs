using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账号权限认证服务
    /// </summary>
    public class AccountAuthorizationService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public AccountAuthorizationService(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, ILogger<AccountAuthorizationService> logger)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<Result<Permission>> Authentication()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            HttpRequest request = context.Request;
            var requestPath = (request.PathBase + request.Path).ToString().ToLower();


            //根据请求地址获取权限
            var parmission = await this.MatchPermission(requestPath);
            if (parmission == null)
            {
                this._logger.LogWarning($"Did not find the corresponding permissions of {requestPath}");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            this._logger.LogDebug($"[{ request.Method}]{requestPath} matches to Permission with {parmission.Id}");
            // 判断请求方法
            if (!parmission.HttpMethods.Select(f => f.Method.ToString().ToUpper()).ToList().Contains(request.Method.ToUpper()))
            {
                this._logger.LogWarning($"Request method is incorrect : [{ request.Method}]{requestPath}");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            if (!parmission.IsNormal())
            {
                this._logger.LogWarning($"{parmission.Id} permissions are not available");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            //如果是开放性权限，只要登录就可以访问
            if (parmission.Type == Values.PermissionType.OpenApi)
            {
                this._logger.LogDebug($"{parmission.Id} Open API authorization succeeded");
                return Result<Permission>.ReSuccess(parmission);
            }

            //获取超级管理员账号
            int uid = context.User.UserId();
            var account = await this._serviceProvider.GetRequiredService<IAccountRepository>().GetAsync(uid);
            if (account == null)
            {
                this._logger.LogWarning($"{uid} Super administrator does not exist");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            //判断是否为超级管理员
            if (account.IsSuperAdministrator())
            {
                return Result<Permission>.ReSuccess(parmission);
            }
            if (account.Roles.Count == 0)
            {
                this._logger.LogDebug("Access to Tokan needs to include roles");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }

            //根据ID获取角色
            var roleList = await this._roleRepository.GetList(account.Roles.ToList());
            if (roleList == null)
            {
                this._logger.LogWarning($"No authorized roles found");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            if (roleList.Where(f => f.IsNormal() && f.ContainPermission(parmission.Id)).Count() == 0)
            {
                this._logger.LogWarning($"Authorized users are not assigned {parmission.Name} permissions ");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            else
            {
                this._logger.LogDebug($"authorization successful");
                return Result<Permission>.ReSuccess(parmission);
            }
        }

        /// <summary>
        /// 匹配权限
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        private async Task<Permission> MatchPermission(string requestPath)
        {
            var parmissions = await _permissionRepository.GetList();
            return parmissions.FirstOrDefault(f => f.MatchApiTemplate(requestPath));
        }

    }
}
