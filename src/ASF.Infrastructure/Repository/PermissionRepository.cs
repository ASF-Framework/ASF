using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        public readonly RepositoryContext _dbContext;
        public PermissionRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Permission> AddAsync(Permission entity)
        {
            var model = Mapper.Map<Model.Permission>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Permission>(model);
        }

        public async Task<Permission> GetAsync(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Permission>(model);
        }

        public async Task<Permission> GetByApiAddress(string apiAddress)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.ApiAddress == apiAddress);
            return Mapper.Map<Permission>(model);
        }

        public async Task<List<Permission>> GetList(IList<string> ids)
        {
            var list = await _dbContext.Permissions.Where(w => ids.Contains(w.Id)).ToListAsync();
            return Mapper.Map<List<Permission>>(list);
        }

        public async Task<bool> HasById(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return model == null ? false : true;
        }

        public Task ModifyAsync(Permission permission)
        {
            var model = Mapper.Map<Model.Permission>(permission);
            _dbContext.Permissions.Update(model);
            // await _dbContext.SaveChangesAsync();
            return Task.FromResult(0);
        }

        public async Task RemoveAsync(string primaryKey)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == primaryKey);
            _dbContext.Remove(model);
            // await _dbContext.SaveChangesAsync();
        }
    }
}
