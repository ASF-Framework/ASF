using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASF.Domain.Entities;
using ASF.Domain.Values;

namespace ASF.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
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
