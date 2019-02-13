using ASF.Application.DTO;
using ASF.Domain.Services;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Application
{
    /// <summary>
    /// 管理员账户验证
    /// </summary>
    public class AuthoriseController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Result<AccessToken>> Index(AuthoriseByUsernameRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return Result<AccessToken>.ReFailure(result);

            //账户登录验证
            var logResult = this._serviceProvider.GetRequiredService<AccountLoginService>().LoginByUsername(dto.Username, dto.Password, HttpContext.Connection.RemoteIpAddress.ToString());
            if (!logResult.Success)
                return Result<AccessToken>.ReFailure(logResult);

            //数据持久化
            await _accountRepository.ModifyAsync(logResult.Data);
            await _unitOfWork.CommitAsync(autoRollback: true);
            return Result<AccessToken>.ReSuccess(logResult.Data.LoginInfo.AccessToken);
        }
    }
}
