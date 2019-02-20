using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    public class AccountPermissionService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPermissionRepository _permissionRepository;

        public AccountPermissionService(IRoleRepository roleRepository, IAccountRepository accountRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<(Account Account, IList<Permission> Permissions)> GetPermissions(int uid)
        {
            var result = new ValueTuple<Account, IList<Permission>>(null, new List<Permission>());
            Account account = await _accountRepository.GetAsync(uid);
            if (account == null)
                return result;

            result.Item1 = account;

            //超级管理员 (九九归一)
            if (account.IsSuperAdministrator())
            {
                result.Item2 = await _permissionRepository.GetList();
                return result;
            }
            else
            {
                //获取账户角色
                var roles = await _roleRepository.GetList(account.Roles.ToList());
                if (roles == null || roles.Count == 0)
                    return result;

                //获取角色权限
                var pids = new List<string>();
                roles
                    .Where(f => f.IsNormal())
                    .Select(f => f.Permissions.ToList())
                    .ToList()
                    .ForEach(p =>
                    {
                        pids = pids.Zip(p, (first, second) => first + second).ToList();
                    });
                result.Item2 = await _permissionRepository.GetList(pids);
                result.Item2 = result.Item2.OrderBy(f => f.Sort).ToList();
                return result;
            }
           
        }
    }
}
