using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.Infrastructure.Model;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ASF.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public readonly RepositoryContext _dbContext;
        public AccountRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Account> AddAsync(Account entity)
        {
            var model = Mapper.Map<AccountModel>(entity);
            await _dbContext.AddAsync(model);
            return Mapper.Map<Account>(model);
        }

        public async Task<Account> GetAsync(PhoneNumber telephone)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Telephone == telephone.ToString());
            return Mapper.Map<Account>(model);
        }

        public async Task<Account> GetAsync(int id)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Account>(model);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Email == email);
            return Mapper.Map<Account>(model);
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Username == username);
            return Mapper.Map<Account>(model);
        }

        public async Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountListPagedRequestDto requestDto)
        {
            var queryable = _dbContext.Accounts
                .Where(w => w.IsDeleted == requestDto.IsDeleted);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%")
                    || EF.Functions.Like(w.Username, "%" + requestDto.Vague + "%")
                    );
            }
            if (requestDto.Status == 1)
                queryable = queryable.Where(w => w.Status == AccountStatus.Normal);
            if (requestDto.Status == 2)
                queryable = queryable.Where(w => w.Status == AccountStatus.NotAllowedLogin);

            var result = queryable.OrderByDescending(p => p.CreateTime);
            var list = await result.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToListAsync();

            return (Mapper.Map<List<Account>>(list), result.Count());
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

        public async Task ModifyAsync(Account account)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Id == account.Id);
            if (model == null) return;
            Mapper.Map(account, model);
            _dbContext.Accounts.Update(model);
        }

        public async Task RemoveAsync(int primaryKey)
        {
            var model = await _dbContext.Accounts.FirstOrDefaultAsync(w => w.Id == primaryKey);
            if (model == null) return;
            model.Delete();
            _dbContext.Accounts.Update(model);
        }

        public async Task<IList<Account>> GetList()
        {
            var list = await _dbContext.Accounts.ToListAsync();
            return Mapper.Map<IList<Account>>(list);
        }


    }
}
