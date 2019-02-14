using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASF.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public readonly RepositoryContext _dbContext;
        public AccountRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Domain.Entities.Account> AddAsync(Domain.Entities.Account entity)
        {
            var model = Mapper.Map<Model.Account>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Domain.Entities.Account>(model);
        }

        public async Task<Domain.Entities.Account> GetAsync(PhoneNumber telephone)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Telephone == telephone.ToString());
            return Mapper.Map<Domain.Entities.Account>(model);
        }

        public async Task<Domain.Entities.Account> GetAsync(int id)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Domain.Entities.Account>(model);
        }

        public async Task<Domain.Entities.Account> GetByEmailAsync(string email)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Email == email);
            return Mapper.Map<Domain.Entities.Account>(model);
        }

        public async Task<Domain.Entities.Account> GetByUsernameAsync(string username)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Username == username);
            return Mapper.Map<Domain.Entities.Account>(model);
        }

        public async Task<bool> HasByEmail(string email)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Email == email);
            return model == null ? false : true;
        }

        public async Task<bool> HasByTelephone(PhoneNumber telephone)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Telephone == telephone.ToString());
            return model == null ? false : true;
        }

        public async Task<bool> HasByUsername(string username)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Username == username);
            return model == null ? false : true;
        }

        public Task ModifyAsync(Domain.Entities.Account account)
        {
            var model = Mapper.Map<Model.Account>(account);
            _dbContext.Accounts.Update(model);
            // await _dbContext.SaveChangesAsync();
            return Task.FromResult(0);
        }

        public async Task RemoveAsync(int primaryKey)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Id == primaryKey);
            model.Delete();
            _dbContext.Accounts.Update(model);
            // await _dbContext.SaveChangesAsync();
        }
    }
}
