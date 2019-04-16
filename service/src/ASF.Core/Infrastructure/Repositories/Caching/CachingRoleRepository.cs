using ASF.Application.DTO;
using ASF.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public class CachingRoleRepository<TImplRepository> : IRoleRepository where TImplRepository : IRoleRepository
    {
        private static readonly ConcurrentDictionary<int, Role> cache = new ConcurrentDictionary<int, Role>();
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public CachingRoleRepository(IServiceProvider serviceProvider, ILogger<CachingRoleRepository<TImplRepository>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        public async Task<Role> GetAsync(int id)
        {
            var list = await this.GetList();
            return list.Where(f => f.Id == id).FirstOrDefault();
        }
        public async Task<IList<Role>> GetList(IList<int> ids)
        {
            var list = await this.GetList();
            return list.Where(w => ids.Contains(w.Id)).ToList();
        }
        public async Task<(IList<Role> Roles, int TotalCount)> GetList(RoleListPagedRequestDto requestDto)
        {
            var list = await this.GetList();
            var queryable = list.Where(w => w.Id > 0);
            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || w.Name.Contains(requestDto.Vague));
            }
            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            return (queryable.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToList(), queryable.Count());
        }

        public Task<IList<Role>> GetList()
        {
            IList<Role> list = cache.Values.ToList();
            if (list.Count > 0)
                return Task.FromResult(list);
            else
            {
                return this.Do(repository =>
                 {
                     var _list = repository.GetList().GetAwaiter().GetResult();
                     foreach (var role in _list)
                     {
                         cache.GetOrAdd(role.Id, role);
                     }
                     return Task.FromResult(_list);
                 });
            }
        }


        public Task<Role> AddAsync(Role entity)
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
        public Task ModifyAsync(Role role)
        {
            return this.Do(repository =>
            {
                repository.ModifyAsync(role).GetAwaiter().GetResult();
                cache.AddOrUpdate(role.Id, role, (key, r) =>
                {
                    return role;
                });
                return Task.CompletedTask;
            });
        }

        public Task ModifyAsync(int roleId, bool enable)
        {
            return this.Do(repository =>
            {
                repository.ModifyAsync(roleId, enable).GetAwaiter().GetResult();
                var role = this.GetAsync(roleId).GetAwaiter().GetResult();
                cache.AddOrUpdate(role.Id, role, (key, r) =>
                {
                    r.Enable = enable;
                    return r;
                });
                return Task.CompletedTask;
            });
           
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
