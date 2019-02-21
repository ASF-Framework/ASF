using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Infrastructure.Model;
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
        public async Task<Permission> AddAsync(Permission entity)
        {
            var model = Mapper.Map<PermissionModel>(entity);
            await _dbContext.AddAsync(model);
            return Mapper.Map<Permission>(model);
        }

        public async Task<Permission> GetAsync(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Permission>(model);
        }

        public async Task<IList<Permission>> GetList(IList<string> ids)
        {
            var list = await _dbContext.Permissions.Where(w => ids.Contains(w.Id)).ToListAsync();
            list = list == null ? new List<PermissionModel>() : list;
            return Mapper.Map<List<Permission>>(list);
        }

        public async Task<IList<Permission>> GetList(PermissionListRequestDto requestDto)
        {
            var queryable = _dbContext.Permissions
                .Where(w => w.Id != "");

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => w.Id.ToString() == requestDto.Vague
                    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%"));
            }
            if (!string.IsNullOrEmpty(requestDto.ParamId))
                queryable = queryable.Where(w => w.ParentId == requestDto.ParamId);

            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            var list = await queryable.ToListAsync();

            return Mapper.Map<IList<Permission>>(list);
        }

        public async Task<IList<Permission>> GetList()
        {
            var list = await _dbContext.Permissions.ToListAsync();
            return Mapper.Map<IList<Permission>>(list);
        }

        public async Task<IList<Permission>> GetListByParentId(string parentId)
        {
            var list = await _dbContext.Permissions.Where(f => f.ParentId == parentId).ToListAsync();
            return Mapper.Map<IList<Permission>>(list);
        }

        public async Task<bool> HasById(string id)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == id);
            return model == null ? false : true;
        }

        public async Task ModifyAsync(Permission permission)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == permission.Id);
            if (model == null) return;
            Mapper.Map(permission, model);
            _dbContext.Permissions.Update(model);
        }

        public async Task RemoveAsync(string primaryKey)
        {
            var model = await _dbContext.Permissions.FirstOrDefaultAsync(w => w.Id == primaryKey);
            if (model == null) return;
            _dbContext.Remove(model);
        }
    }
}
