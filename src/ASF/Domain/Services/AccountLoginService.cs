using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Anticorrsives;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户登录服务
    /// </summary>
    public class AccountLoginService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccessTokenGenerate _tokenGenerate;
        private readonly LogLoginRecordService _logLoginRecordService;
        private string loginType = string.Empty;

        public AccountLoginService(IAccountRepository accountRepository, IAccessTokenGenerate tokenGenerate, LogLoginRecordService logLoginRecordService)
        {
            _accountRepository = accountRepository;
            _tokenGenerate = tokenGenerate;
            _logLoginRecordService = logLoginRecordService;
        }

        /// <summary>
        /// 使用用户名登录
        /// </summary>
        /// <param name="username">账户账号</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        public Result<Account> LoginByUsername(string username, string password, string ip)
        {
            //获取用户数据
            var account = _accountRepository.GetByUsernameAsync(username).GetAwaiter().GetResult();
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //登录验证
            loginType = "UsernameAndPassword";
            return this.Login(account, password, ip);
        }
        /// <summary>
        /// 使用邮箱登录账户
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        public Result<Account> LoginByEmail(string email, string password, string ip)
        {
            //获取用户数据
            var account = _accountRepository.GetByEmailAsync(email).GetAwaiter().GetResult();
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //登录验证
            loginType = "EmailAndPassword";
            return this.Login(account, password, ip);
        }
        /// <summary>
        /// 使用邮箱登录账户
        /// </summary>
        /// <param name="telephone">手机号码</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        public Result<Account> LoginByTelephone(PhoneNumber telephone, string password, string ip)
        {
            //获取用户数据
            var account = _accountRepository.GetAsync(telephone).GetAwaiter().GetResult();
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //登录验证
            loginType = "TelephoneAndPassword";
            return this.Login(account, password, ip);
        }
        /// <summary>
        /// 登录用户验证
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">账户密码</param>
        /// <param name="loginInfo">登录信息</param>
        /// <returns></returns>
        private Result<Account> Login(Account account, string password, string ip)
        {
            //判断用户是否禁止登陆和密码匹配
            if (!account.IsAllowLogin())
                return Result<Account>.ReFailure(ResultCodes.AccountNotAllowedLogin);
            if (!account.HasPassword(password))
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame);

            //生成访问Token
            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.Role, "self");
            claims.Add("name", account.Name);
            claims.Add("sub", account.Id.ToString());
            claims.Add("auth_mode", loginType);
            claims.Add("roles", string.Join(",", account.Roles.ToArray()));
            var accessToken = _tokenGenerate.Generate(claims);

            //生成访问Token
            account.SetLoginInfo(new LoginInfo(ip, accessToken));

            //记录登录日志
            this._logLoginRecordService.Record(account, loginType);
            return Result<Account>.ReSuccess(account);
        }
    }
}
