using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository<Account, int>
    {
        /// <summary>
        /// 这用户名是否被使用
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        Task<bool> HasByUsername(string username);
        /// <summary>
        /// 这电话号码是否被使用
        /// </summary>
        /// <param name="telephone">电话号码</param>
        /// <returns></returns>
        Task<bool> HasByTelephone(PhoneNumber telephone);
        /// <summary>
        /// 这邮箱地址是否被使用
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns></returns>
        Task<bool> HasByEmail(string email);


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">账户</param>
        /// <returns></returns>
        Task<Account> GetByUsernameAsync(string username);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">电子邮箱</param>
        /// <returns></returns>
        Task<Account> GetByEmailAsync(string email);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">账户</param>
        /// <returns></returns>
        Task<Account> GetAsync(PhoneNumber telephone);
        /// <summary>
        /// 分页获取用户集合
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountInfoListPagedRequestDto requestDto);
        /// <summary>
        /// 修改账户
        /// </summary>
        /// <param name="account">账户信息</param>
        /// <returns></returns>
        Task ModifyAsync(Account account);
    }
}
