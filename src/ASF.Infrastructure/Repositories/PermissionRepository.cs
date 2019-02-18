using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var model = Mapper.Map<Model.PermissionModel>(entity);
            await _dbContext.AddAsync(model);
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
            list = list == null ? new List<Model.PermissionModel>() : list;
            return Mapper.Map<List<Domain.Entities.Permission>>(list);
        }

        public Task<IList<Permission>> GetList(PermissionInfoListRequestDto requestDto)
        {
            throw new System.NotImplementedException();
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
        }

        public async Task RemoveAsync(string primaryKey)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == primaryKey);
            _dbContext.Remove(model);
        }

        Task<IList<Permission>> IPermissionRepository.GetList(IList<string> ids)
        {
            throw new System.NotImplementedException();
        }
    }
}
