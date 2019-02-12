using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户手机号码设置、修改服务
    /// </summary>
    public class AccountTelephoneChangeService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountTelephoneChangeService(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }
        /// <summary>
        /// 设置手机号码
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="telephone">手机号码</param>
        /// <returns></returns>
        public Result<Account> Modify(int accountId, PhoneNumber telephone)
        {
            if (accountId <= 0)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //验证电话号码是否合法
            var result = ValidationHelper.ValidResult(telephone);
            if (!result.Success)
                return Result<Account>.ReFailure(result);

            //存储获取数据
            var accountTask = _accountRepository.GetAsync(accountId);
            var hasTelephoneTask = _accountRepository.HasByTelephone(telephone);
            Task.WaitAll(accountTask, hasTelephoneTask);

            //获取用户信息
            var account = accountTask.Result;
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //验证手机号码是否已经被使用
            if (hasTelephoneTask.Result)
                return Result<Account>.ReFailure(ResultCodes.AccountOldPasswordAndNewPasswordSame);

            account.Telephone = telephone;
            return Result<Account>.ReSuccess(account);
        }
    }
}
