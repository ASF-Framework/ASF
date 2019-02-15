using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;

namespace ASF.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        static List<Account> Accounts = new List<Account>();
        static AccountRepository()
        {
            Account account = new Account("admin", "21232f297a57a5a743894a0e4a801fc3", null);
            account.SetRoles(new List<int>() { 0 });
            Accounts.Add(account);

        }
        public Task<Account> AddAsync(Account entity)
        {
            return Task.FromResult(entity);
        }

        public Task<Account> GetAsync(PhoneNumber telephone)
        {
            return this.GetAsync(0);
        }

        public Task<Account> GetAsync(int id)
        {
            Account account = new Account("admin", "21232f297a57a5a743894a0e4a801fc3", null);
            return Task.FromResult(account);
        }

        public Task<Account> GetByEmailAsync(string email)
        {
            return this.GetAsync(0);
        }

        public Task<Account> GetByUsernameAsync(string username)
        {
            return this.GetAsync(0);
        }

        public Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountInfoListPagedRequestDto requestDto)
        {
            var a = new ValueTuple<IList<Account>, int>(AccountRepository.Accounts, AccountRepository.Accounts.Count);
            return Task.FromResult(a);
        }

        public Task<bool> HasByEmail(string email)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasByTelephone(PhoneNumber telephone)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasByUsername(string username)
        {
            return Task.FromResult(false);
        }

        public Task ModifyAsync(Account account)
        {

            return Task.CompletedTask;
        }

        public Task RemoveAsync(int primaryKey)
        {
            return Task.CompletedTask;
        }
    }
}
