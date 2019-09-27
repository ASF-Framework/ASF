using ASF.Domain.Entities;
using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Domain.Services
{
    public interface IAccountLoginService
    {
        /// <summary>
        /// 使用用户名登录
        /// </summary>
        /// <param name="username">账户账号</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        Result<Account> LoginByUsername(string username, string password, string ip);
        /// <summary>
        /// 使用邮箱登录账户
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        Result<Account> LoginByEmail(string email, string password, string ip);
        /// <summary>
        /// 使用手机登录账户
        /// </summary>
        /// <param name="telephone">手机号码</param>
        /// <param name="password">账户密码</param>
        /// <param name="ip">登录IP</param>
        /// <returns></returns>
        Result<Account> LoginByTelephone(PhoneNumber telephone, string password, string ip);
    }
}
