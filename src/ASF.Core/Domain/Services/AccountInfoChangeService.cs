using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户信息修改服务
    /// </summary>
    public class AccountInfoChangeService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IServiceProvider _serviceProvider;

        public AccountInfoChangeService(IAccountRepository accountRepository, IServiceProvider serviceProvider)
        {
            _accountRepository = accountRepository;
            _serviceProvider = serviceProvider;
        }



        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="name">昵称</param>
        /// <param name="status">状态</param>
        /// <param name="roles">角色</param>
        /// <returns></returns>
        public async Task<Result<Account>> Modify(int accountId, string name,  AccountStatus status,List<int> roles)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);
            if (account.IsSuperAdministrator())
                //如果分配了角色需要验证角色
                if (roles.Count > 0)
            {
                var result = await this._serviceProvider.GetRequiredService<AccountRoleAssignationService>()
                    .Assignation(account, roles);
                if (!result.Success)
                    return result;
            }
            account.Status = status;
            account.Name = name;

            //验证角色是否可用
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
        /// <summary>
        /// 修改昵称和头像
        /// </summary>
        /// <param name="accountId">账户标识</param>
        /// <param name="name">昵称</param>
        /// <param name="avatar">头像</param>
        /// <returns></returns>
        public async Task<Result<Account>> ModifyNameOrAvatar(int accountId,string name,string avatar)
        {
            //获取账户信息
            var account = await _accountRepository.GetAsync(accountId);
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);

            if (!string.IsNullOrEmpty(name))
                account.Name = name;

            if (!string.IsNullOrEmpty(avatar))
                account.Avatar = avatar;
            //验证角色是否可用
            return account.Valid<Account>();
        }
    }
}
