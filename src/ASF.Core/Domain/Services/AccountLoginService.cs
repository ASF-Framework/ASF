using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户登录服务
    /// </summary>
    public class AccountLoginService : IAccountLoginService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccessTokenGenerate _tokenGenerate;
        private readonly LogLoginRecordService _logLoginRecordService;
        private readonly IMemoryCache _memoryCache;
        private string loginType = string.Empty;
        private int maxLoginFailedCount = 5;

        public AccountLoginService(
            IAccountRepository accountRepository,
            IAccessTokenGenerate tokenGenerate,
            LogLoginRecordService logLoginRecordService,
            IMemoryCache memoryCache)
        {
            _accountRepository = accountRepository;
            _tokenGenerate = tokenGenerate;
            _logLoginRecordService = logLoginRecordService;
            _memoryCache = memoryCache;
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
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame);

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
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame);

            //登录验证
            loginType = "EmailAndPassword";
            return this.Login(account, password, ip);
        }
        /// <summary>
        /// 使用手机登录账户
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
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame);

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
            {
                //获取是否有登录失败信息
                LoginFailed loginFailed = this.GetLoginFailedInfo(account.Username);
                loginFailed.Accumulative();
                if (loginFailed.FailedCount >= this.maxLoginFailedCount)
                {
                    return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSameOverrun);
                }
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame2.ToFormat((this.maxLoginFailedCount - loginFailed.FailedCount).ToString()));
            }

            //生成访问Token
            List<Claim> claims = new List<Claim>();
            claims.AddRange(new[] {
                new Claim(ClaimTypes.Role, "self"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim("name", account.Username),
                new Claim("nickname", account.Name),
                new Claim("sub", account.Id.ToString()),
                new Claim("auth_mode", loginType)
            });


            var accessToken = _tokenGenerate.Generate(claims);
            //生成访问Token
            account.SetLoginInfo(new LoginInfo(ip, accessToken));


            //记录登录日志
            this._logLoginRecordService.Record(account, loginType);
            //移除登录失败记录
            this.RemoveLoginFailedInfo(account.Username);
            return Result<Account>.ReSuccess(account);
        }


        /// <summary>
        /// 获取登录失败信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private LoginFailed GetLoginFailedInfo(string username)
        {
            string key = "LOGIN_FILED_" + username;
            if (_memoryCache.TryGetValue<LoginFailed>(key, out LoginFailed loginFailed))
            {
                return loginFailed;
            }
            return _memoryCache.Set<LoginFailed>(key, new LoginFailed(username), TimeSpan.FromMinutes(30));
        }


        private void RemoveLoginFailedInfo(string username)
        {
            string key = "LOGIN_FILED_" + username;
            _memoryCache.Remove(key);
        }
    }
}
