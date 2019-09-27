using ASF.Application.DTO;
using ASF.Domain.Services;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.Application
{
    /// <summary>
    /// 管理员账户验证
    /// </summary>
    [Route("[controller]/[action]")]
    public class AuthoriseController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public AuthoriseController(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// 账户登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<AccessToken>> Login([FromBody]AuthoriseByUsernameRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Result<AccessToken>.ReFailure(result);
            }

            //账户登录验证
            var service = this._serviceProvider.GetRequiredService<IAccountLoginService>();
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip))
                ip = "127.0.0.1";
            var logResult = service.LoginByUsername(dto.Username, dto.Password, ip);
            if (!logResult.Success)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Result<AccessToken>.ReFailure(logResult);
            }

            //数据持久化
            await _accountRepository.ModifyAsync(logResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result<AccessToken>.ReSuccess(logResult.Data.LoginInfo.AccessToken);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result Logout()
        {
            return Result.ReSuccess();
        }
    }
}
