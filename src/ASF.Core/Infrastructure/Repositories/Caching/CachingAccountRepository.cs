using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public class CachingAccountRepository<TImplRepository> : IAccountRepository where TImplRepository : IAccountRepository
    {
        private static readonly ConcurrentDictionary<int, Account> cache = new ConcurrentDictionary<int, Account>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        public CachingAccountRepository(IServiceProvider serviceProvider, ILogger<CachingAccountRepository<TImplRepository>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<Account> GetAsync(PhoneNumber telephone)
        {
            var list = await this.GetList();
            return list.Where(f => f.Telephone == telephone).FirstOrDefault();
        }

        public async Task<Account> GetAsync(int id)
        {
            var list = await this.GetList();
            return list.FirstOrDefault(w => w.Id == id);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var list = await this.GetList();
            return list.FirstOrDefault(w => w.Email == email);
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var list = await this.GetList();
            return list.FirstOrDefault(w => w.Username == username);
        }

        public async Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountListPagedRequestDto requestDto)
        {
            var list = await this.GetList();
            var queryable = list.Where(w => w.IsDeleted == requestDto.IsDeleted);
            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || w.Name.Contains(requestDto.Vague)
                    || w.Username.Contains(requestDto.Vague)
                    );
            }
            if (requestDto.Status == 1)
                queryable = queryable.Where(w => w.Status == AccountStatus.Normal);
            if (requestDto.Status == 2)
                queryable = queryable.Where(w => w.Status == AccountStatus.NotAllowedLogin);

            var result = queryable.OrderByDescending(p => p.CreateInfo.CreateTime);
            return (result.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToList(), result.Count());
        }
        public Task<IList<Account>> GetList()
        {
            IList<Account> list = cache.Values.ToList();
            if (list.Count > 0)
                return Task.FromResult(list);
            else
            {
                return this.Do(repository =>
                {
                    var _list = repository.GetList().GetAwaiter().GetResult();
                    foreach (var data in _list)
                    {
                        cache.GetOrAdd(data.Id, data);
                    }
                    return Task.FromResult(_list);
                });
            }
        }
        public Task<bool> HasByEmail(string email)
        {
            return this.Do(repository =>
            {
                return repository.HasByEmail(email);
            });
        }
        public Task<bool> HasByTelephone(PhoneNumber telephone)
        {
            return this.Do(repository =>
            {
                return repository.HasByTelephone(telephone);
            });
        }

        public Task<bool> HasByUsername(string username)
        {
            return this.Do(repository =>
            {
                return repository.HasByUsername(username);
            });
        }

        public async Task ModifyAsync(Account account)
        {
            var repository = this._serviceProvider.GetRequiredService<TImplRepository>();
            await repository.ModifyAsync(account);
            cache.AddOrUpdate(account.Id, account, (key, r) =>
            {
                return account;
            });
        }

        public Task<Account> AddAsync(Account entity)
        {
            entity = cache.GetOrAdd(entity.Id, key =>
            {
                return this.Do(repository =>
                {
                    return repository.AddAsync(entity).GetAwaiter().GetResult();
                });
            });
            return Task.FromResult(entity);
        }
        public Task RemoveAsync(int primaryKey)
        {
            return this.Do(repository =>
            {
                repository.RemoveAsync(primaryKey).GetAwaiter().GetResult();
                cache.TryRemove(primaryKey, out var role);
                return Task.CompletedTask;
            });
        }

        public TRes Do<TRes>(Func<TImplRepository, TRes> action)
        {
            var repository = this._serviceProvider.GetRequiredService<TImplRepository>();
            return action.Invoke(repository);
        }
    }
}
