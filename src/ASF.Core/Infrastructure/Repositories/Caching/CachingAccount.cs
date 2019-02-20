using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASF.Infrastructure.Repositories
{
    public class CachingAccount<T> : IAccountRepository where T : IAccountRepository
    {
        private readonly T _repository;
        private readonly ICache<Account> _accountCache;
        private readonly string _cacheKey = "GetList";
        private readonly ILogger<CachingAccount<T>> _logger;
        private TimeSpan _duration = new TimeSpan(0, 5, 0);
        public CachingAccount(T repository, ICache<Account> accountCache, ILogger<CachingAccount<T>> logger)
        {
            _repository = repository;
            _accountCache = accountCache;
            _logger = logger;
        }


        public async Task<Account> AddAsync(Account entity)
        {
            var account = await _repository.AddAsync(entity);
            //更新缓存
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            list.Add(account);
            await _accountCache.SetAsync(_cacheKey, list, _duration);
            return account;
        }

        public async Task<Account> GetAsync(PhoneNumber telephone)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Telephone.ToString() == telephone.ToString());
        }

        public async Task<Account> GetAsync(int id)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Id == id);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Email == email);
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Username == username);
        }

        public async Task<(IList<Account> Accounts, int TotalCount)> GetList(AccountListPagedRequestDto requestDto)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var queryable = list.Where(w => w.IsDeleted == requestDto.IsDeleted);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString().Contains(requestDto.Vague)
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

        public async Task<IList<Account>> GetList()
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list;
        }

        public async Task<bool> HasByEmail(string email)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var model = list.FirstOrDefault(w => w.Email == email);
            return model == null ? false : true;
        }

        public async Task<bool> HasByTelephone(PhoneNumber telephone)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var model = list.FirstOrDefault(w => w.Telephone.ToString() == telephone.ToString());
            return model == null ? false : true;
        }

        public async Task<bool> HasByUsername(string username)
        {
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var model = list.FirstOrDefault(w => w.Username == username);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Account account)
        {
            await _repository.ModifyAsync(account);

            //更新缓存
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == account.Id);
            if (entity != null)
            {
                list.Remove(entity);
                var model = await _repository.GetAsync(account.Id);
                if (model == null) return;
                list.Add(model);
                await _accountCache.SetAsync(_cacheKey, list, _duration);
            }
        }

        public async Task RemoveAsync(int primaryKey)
        {
            await _repository.RemoveAsync(primaryKey);

            //更新缓存
            var list = await _accountCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == primaryKey);
            if (entity != null)
            {
                list.Remove(entity);
                var model = await _repository.GetAsync(primaryKey);
                if (model == null) return;
                list.Add(model);
                await _accountCache.SetAsync(_cacheKey, list, _duration);
            }
        }
    }
}
