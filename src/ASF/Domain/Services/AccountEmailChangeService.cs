using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    public class AccountEmailChangeService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountEmailChangeService(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }
        public Result<Account> Modify(int accountId, string email)
        {
            //前往存储获取数据
            var accountTask = _accountRepository.GetAsync(accountId);
            var hasEmailTask = _accountRepository.HasByEmail(email);
            Task.WaitAll(accountTask, hasEmailTask);

            //获取用户数据
            var account = accountTask.Result;
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //验证邮箱地址是否已经被使用
            if (hasEmailTask.Result)
                return Result<Account>.ReFailure(ResultCodes.AccountEmailExist);

            account.Email = email;

            //验证聚合数据是否合法
            return account.Valid<Account>();
        }
    }
}
