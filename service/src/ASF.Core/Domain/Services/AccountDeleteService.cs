using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    public class AccountDeleteService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountDeleteService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<Result<Account>> Delete(int uid)
        {
            //获取权限数据
            var account = await _accountRepository.GetAsync(uid);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            //超级管理员不能删除
            if (account.IsSuperAdministrator())
                return Result<Account>.ReFailure(ResultCodes.AccountSuperAdminNoAllowedDelete);

            if (!account.IsDeleted)
                account.Delete();

            return Result<Account>.ReSuccess(account);
        }
    }
}
