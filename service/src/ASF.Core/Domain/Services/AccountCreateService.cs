using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 账户创建服务
    /// </summary>
    public class AccountCreateService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountRoleAssignationService _roleAssignationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountCreateService(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor, AccountRoleAssignationService roleAssignationService)
        {
            this._accountRepository = accountRepository;
            this._roleAssignationService = roleAssignationService;
            this._httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 创建账户信息
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public async Task<Result> Create(Account account)
        {
            //验证用户名是否已经被使用
            if (await _accountRepository.HasByUsername(account.Username))
                return Result.ReFailure(ResultCodes.AccountUsernameExist);

            //获取创建账户的用户
            int uid = this._httpContextAccessor.HttpContext.User.UserId();
            account.SetCreateOfAccount(uid);
            account.Id = DateTime.Now.ToUnixTime32();
            //如果分配了角色需要验证角色
            var roles = (List<int>)account.Roles;
            if (roles.Count > 0)
            {
                var result = await this._roleAssignationService.Assignation(account, roles);
                if (!result.Success)
                    return result;
            }
            //验证聚合数据是否合法
            return account.Valid();
        }
    }
}
