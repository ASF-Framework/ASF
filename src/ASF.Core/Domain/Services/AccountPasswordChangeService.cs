using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户登录密码修改、设置服务
    /// </summary>
    public class AccountPasswordChangeService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountPasswordChangeService(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        /// <summary>
        /// 设置登录密码
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="password">需要设置的密码</param>
        /// <returns></returns>
        public async Task<Result<Account>> Reset(int accountId, string password, int adminAccountId,string adminPassword)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //获取管理员信息,需要验证管理员密码
            var admin =  await _accountRepository.GetAsync(adminAccountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountResetPasswordPasswordNotSame);
            if (!account.HasPassword(adminPassword))
                return Result<Account>.ReFailure(ResultCodes.AccountResetPasswordPasswordNotSame);

            account.SetPassword(password);
            //验证聚合数据是否合法
            return account.Valid<Account>();
        }
        /// <summary>
        /// 修改登录密码(需要验证原密码是否相符)
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="newPassword">需要设置的密码</param>
        /// <param name="oldPassword">原密码</param>
        /// <returns></returns>
        public async Task<Result<Account>> Modify(int accountId, string newPassword, string oldPassword)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //验证和当前密码是否一致
            if (!account.HasPassword(oldPassword))
                return Result<Account>.ReFailure(ResultCodes.AccountPasswordNotSame);

            account.SetPassword(newPassword);

            //验证聚合数据是否合法
            return account.Valid<Account>();
        }
       
    }
}
