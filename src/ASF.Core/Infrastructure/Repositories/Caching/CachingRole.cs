using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASF.Application.DTO;
using ASF.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASF.Infrastructure.Repositories
{
    public class CachingRole<T> : IRoleRepository where T : IRoleRepository
    {
        private readonly T _repository;
        private readonly ICache<Role> _roleCache;
        private readonly string _cacheKey = "GetList";
        private readonly ILogger<CachingRole<T>> _logger;
        private TimeSpan _duration = new TimeSpan(0, 5, 0);
        public CachingRole(T repository, ICache<Role> roleCache, ILogger<CachingRole<T>> logger)
        {
            _repository = repository;
            _roleCache = roleCache;
            _logger = logger;
        }
        public async Task<Role> AddAsync(Role entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<Role> GetAsync(int id)
        {
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Id == id);
        }

        public async Task<IList<Role>> GetList(IList<int> ids)
        {
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.Where(w => ids.Contains(w.Id)).ToList();
        }

        public async Task<IList<Role>> GetList()
        {
            return await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
        }

        public async Task<(IList<Role> Roles, int TotalCount)> GetList(RoleListPagedRequestDto requestDto)
        {
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var queryable = list.Where(w => w.Id > 0);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString().Contains(requestDto.Vague)
                    || w.Name.Contains(requestDto.Vague));
            }
            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);
            
            return (queryable.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToList(), queryable.Count());
        }

        public async Task ModifyAsync(Role role)
        {
            await _repository.ModifyAsync(role);

            //更新缓存
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == role.Id);
            if (entity != null)
            {
                list.Remove(entity);
                var model = await _repository.GetAsync(role.Id);
                list.Add(model);
                await _roleCache.SetAsync(_cacheKey, list, _duration);
            }
        }

        public async Task ModifyAsync(int roleId, bool enable)
        {
            await _repository.ModifyAsync(roleId, enable);

            //更新缓存
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == roleId);
            if (entity != null)
            {
                list.Remove(entity);
                var model = await _repository.GetAsync(roleId);
                list.Add(model);
                await _roleCache.SetAsync(_cacheKey, list, _duration);
            }
        }

        public async Task RemoveAsync(int primaryKey)
        {
            await _repository.RemoveAsync(primaryKey);

            //更新缓存
            var list = await _roleCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == primaryKey);
            if (entity != null)
            {
                list.Remove(entity);
                await _roleCache.SetAsync(_cacheKey, list, _duration);
            }
        }
    }
}
