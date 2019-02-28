using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

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

        public Result<Permission> Authentication()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            HttpRequest request = context.Request;
            var requestPath = (request.PathBase + request.Path).ToString().ToLower();


            //根据请求地址获取权限
            var parmission = this.MatchPermission(requestPath);
            if (parmission == null)
            {
                this._logger.LogWarning($"Did not find the corresponding permissions of {requestPath}");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            if (!parmission.IsNormal())
            {
                this._logger.LogWarning($"{parmission.Name} permissions are not available");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }

            //获取超级管理员账号
            int uid = context.User.UserId();
            var account = this._serviceProvider.GetRequiredService<IAccountRepository>().GetAsync(uid).GetAwaiter().GetResult();
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
            if (account.Roles.Count==0)
            {
                this._logger.LogDebug("Access to Tokan needs to include roles");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }

            //根据ID获取角色
            var roleList = this._roleRepository.GetList(account.Roles.ToList()).GetAwaiter().GetResult();
            if (roleList == null)
            {
                this._logger.LogWarning($"No authorized roles found");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            if(roleList.Where(f=>f.IsNormal() && f.ContainPermission(parmission.Id)).Count() == 0)
            {
                this._logger.LogWarning($"Authorized users are not assigned {parmission.Name} permissions ");
                return Result<Permission>.ReFailure(ResultCodes.NotAcceptable);
            }
            else
            {
                return Result<Permission>.ReSuccess(parmission);
            }
        }

        /// <summary>
        /// 匹配权限
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        private Permission MatchPermission(string requestPath)
        {
            var parmissions = _permissionRepository.GetList().GetAwaiter().GetResult();
            return parmissions.FirstOrDefault(f => f.MatchApiTemplate(requestPath));
        }

    }
}
