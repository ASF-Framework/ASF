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
   public class RoleRepository: IRoleRepository
    {
        public readonly RepositoryContext _dbContext;
        public RoleRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> AddAsync(Role entity)
        {
            var model = Mapper.Map<Model.Role>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Role>(model);
        }

        public async Task<Role> GetAsync(int id)
        {
            var model = await _dbContext.Roles.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Role>(model);
        }

        public async Task<List<Role>> GetList(IList<int> ids)
        {
            var list = await _dbContext.Roles.Where(w => ids.Contains(w.Id)).ToListAsync();
            return Mapper.Map<List<Role>>(list);
        }

        public async Task<List<Role>> GetList()
        {
            var list = await _dbContext.Roles.ToListAsync();
            return Mapper.Map<List<Role>>(list);
        }

        public Task ModifyAsync(Role role)
        {
            var model = Mapper.Map<Model.Role>(role);
            _dbContext.Roles.Update(model);
            return Task.FromResult(0);
        }

        public async Task ModifyAsync(int roleId, bool enable)
        {
            var model = await _dbContext.Roles.FirstOrDefaultAsync(w => w.Id == roleId);
            model.Enable = enable;
            _dbContext.Roles.Update(model);
           // await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int primaryKey)
        {
            var model = await _dbContext.Roles.FirstOrDefaultAsync(w => w.Id == primaryKey);
            _dbContext.Remove(model);
            // await _dbContext.SaveChangesAsync();
        }
    }
}
