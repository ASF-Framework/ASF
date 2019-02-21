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
    public class CachingPermissionRepository<T> : IPermissionRepository where T : IPermissionRepository
    {
        private readonly T _repository;
        private readonly ICache<Permission> _permissionCache;
        private readonly string _cacheKey = "GetList";
        private readonly ILogger<CachingPermissionRepository<T>> _logger;
        private TimeSpan _duration = new TimeSpan(1000, 0, 0, 0);
        public CachingPermissionRepository(T repository, ICache<Permission> permissionCache, ILogger<CachingPermissionRepository<T>> logger)
        {
            _repository = repository;
            _permissionCache = permissionCache;
            _logger = logger;
        }
        public async Task<Permission> AddAsync(Permission entity)
        {
            var permission= await _repository.AddAsync(entity);

            //更新缓存
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            list.Add(permission);
            await _permissionCache.SetAsync(_cacheKey, list, _duration);
            return permission;
        }

        public async Task<Permission> GetAsync(string id)
        {
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.FirstOrDefault(w => w.Id == id);
        }

        public async Task<IList<Permission>> GetList(IList<string> ids)
        {
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            return list.Where(w => ids.Contains(w.Id)).ToList();
        }

        public async Task<IList<Permission>> GetList()
        {
            return await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
        }

        public async Task<IList<Permission>> GetList(PermissionListRequestDto requestDto)
        {
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);

            var queryable= list.Where(w => w.Id != "");
            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.Contains(requestDto.Vague)
                    || w.Name.Contains(requestDto.Vague));
            }
            if (!string.IsNullOrEmpty(requestDto.ParamId))
                queryable.Where(w => w.ParentId == requestDto.ParamId);

            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            return queryable.ToList();
        }

        public Task<IList<Permission>> GetListByParentId(string parentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasById(string id)
        {
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var model = list.FirstOrDefault(w => w.Id == id);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Permission permission)
        {
            await _repository.ModifyAsync(permission);

            //更新缓存
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == permission.Id);
            if (entity != null)
            {
                list.Remove(entity);
                var model = await _repository.GetAsync(permission.Id);
                if (model != null)
                    list.Add(model);
                await _permissionCache.SetAsync(_cacheKey, list, _duration);
            }
        }

        public async Task RemoveAsync(string primaryKey)
        {
            await _repository.RemoveAsync(primaryKey);

            //更新缓存
            var list = await _permissionCache.GetAsync(_cacheKey, _duration, async () => await _repository.GetList(), _logger);
            var entity = list.FirstOrDefault(f => f.Id == primaryKey);
            if (entity != null)
            {
                list.Remove(entity);
                await _permissionCache.SetAsync(_cacheKey, list, _duration);
            }
        }
    }
}
