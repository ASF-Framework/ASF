using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户信息修改服务
    /// </summary>
    public class AccountInfoChangeService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountInfoChangeService(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="status">账户状态</param>
        /// <param name="avatar">账户头像</param>
        /// <returns></returns>
        public async Task<Result<Account>> Modify(int accountId, string name, string avatar, AccountStatus status)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //验证角色是否可用
            account.Status = status;
            account.Avatar = avatar;
            account.Name = name;
            return account.Valid<Account>();
        }

        /// <summary>
        /// 修改账户状态
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="status">账户状态</param>
        /// <returns></returns>
        public async Task<Result<Account>> ModifyStatus(int accountId, AccountStatus status)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            account.Status = status;
            return Result<Account>.ReSuccess(account);
        }


    }
}
