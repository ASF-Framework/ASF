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
    public class CachingPermissionRepository<TImplRepository> : IPermissionRepository where TImplRepository : IPermissionRepository
    {
        private static readonly ConcurrentDictionary<string, Permission> cache = new ConcurrentDictionary<string, Permission>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        public CachingPermissionRepository(IServiceProvider serviceProvider, ILogger<CachingPermissionRepository<TImplRepository>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<Permission> GetAsync(string id)
        {
            var list = await this.GetList();
            return list.Where(f => f.Id == id).FirstOrDefault();
        }

        public async Task<IList<Permission>> GetList(IList<string> ids)
        {
            var list = await this.GetList();
            return list.Where(w => ids.Contains(w.Id)).ToList();
        }

        public Task<IList<Permission>> GetList()
        {
            IList<Permission> list = cache.Values.ToList();
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

        public async Task<IList<Permission>> GetList(PermissionListRequestDto requestDto)
        {
            var list = await this.GetList();
            var queryable = list.Where(w => w.Id != "");
            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id == requestDto.Vague
                    || w.Name.Contains(requestDto.Vague));
            }
            if (!string.IsNullOrEmpty(requestDto.ParamId))
                queryable= queryable.Where(w => w.ParentId == requestDto.ParamId);

            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            return queryable.ToList();
        }

        public async Task<IList<Permission>> GetListByParentId(string parentId)
        {
            var list = await this.GetList();
            return list.Where(f => f.ParentId == parentId).ToList();
        }

        public async Task<IList<Permission>> GetActionListByParentId(string parentId)
        {
            var list = await this.GetList();
            return list.Where(f => f.ParentId == parentId && f.Type== PermissionType.Action).ToList();
        }

        public Task<bool> HasById(string id)
        {
            return this.Do(repository =>
             {
                 return repository.HasById(id);
             });
        }

        public Task<Permission> AddAsync(Permission entity)
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


        public Task ModifyAsync(Permission permission)
        {
            return this.Do(repository =>
            {
                repository.ModifyAsync(permission).GetAwaiter().GetResult();
                cache.AddOrUpdate(permission.Id, permission, (key, r) =>
                {
                    return permission;
                });
                return Task.CompletedTask;
            });

        }

        public Task RemoveAsync(string primaryKey)
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
