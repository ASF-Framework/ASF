using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 管理员账户角色分配服务
    /// </summary>
    public class AccountRoleAssignationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        public AccountRoleAssignationService(IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            this._accountRepository = accountRepository;
            this._roleRepository = roleRepository;
        }
        public async Task<Result<Account>> Assignation(int accountId, List<int> rids)
        {
            //获取用户信息
            var account = await _accountRepository.GetAsync(accountId);
            //分配角色
            return await this.Assignation(account, rids);
        }


        public async Task<Result<Account>> Assignation(Account account, List<int> rids)
        {
            if (account == null)
                return Result<Account>.ReFailure(ResultCodes.AccountNotExist);
            if (account.IsSuperAdministrator())
                return Result<Account>.ReFailure(ResultCodes.AccountSuperAdminNoAllowedModify);

            //获取角色信息
            var roles = await _roleRepository.GetList(rids);
            if (rids.Count != roles.Count)
                return Result<Account>.ReFailure(ResultCodes.AccountRoleAssignationFailed);
            //验证角色是否可用
            foreach (var role in roles)
            {
                if (!role.IsNormal())
                {
                    return Result<Account>.ReFailure(ResultCodes.RoleUnavailable.ToFormat(role.Name));
                }
            }
            account.SetRoles(rids);
            return Result<Account>.ReSuccess(account);
        }
    }
}
