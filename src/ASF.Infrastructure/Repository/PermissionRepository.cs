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
        public async Task<Domain.Entities.Permission> AddAsync(Domain.Entities.Permission entity)
        {
            var model = Mapper.Map<Model.Permission>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Domain.Entities.Permission>(model);
        }

        public async Task<Domain.Entities.Permission> GetAsync(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Domain.Entities.Permission>(model);
        }

        public async Task<Domain.Entities.Permission> GetByApiAddress(string apiAddress)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.ApiAddress == apiAddress);
            return Mapper.Map<Domain.Entities.Permission>(model);
        }

        public async Task<List<Domain.Entities.Permission>> GetList(IList<string> ids)
        {
            var list = await _dbContext.Permissions.Where(w => ids.Contains(w.Id)).ToListAsync();
            list = list == null ? new List<Model.Permission>() : list;
            return Mapper.Map<List<Domain.Entities.Permission>>(list);
        }

        public async Task<bool> HasById(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Domain.Entities.Permission permission)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == permission.Id);
            Mapper.Map(permission, model);
            _dbContext.Permissions.Update(model);
            // await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(string primaryKey)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == primaryKey);
            _dbContext.Remove(model);
            // await _dbContext.SaveChangesAsync();
        }
    }
}
