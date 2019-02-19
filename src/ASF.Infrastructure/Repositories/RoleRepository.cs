﻿using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Domain.Entities.Role> AddAsync(Domain.Entities.Role entity)
        {
            var model = Mapper.Map<Model.RoleModel>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Domain.Entities.Role>(model);
        }

        public async Task<Domain.Entities.Role> GetAsync(int id)
        {
            var model = await _dbContext.Roles.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Domain.Entities.Role>(model);
        }

        public async Task<List<Domain.Entities.Role>> GetList(IList<int> ids)
        {
            var list = await _dbContext.Roles.Where(w => ids.Contains(w.Id)).ToListAsync();
            list = list == null ? new List<Model.RoleModel>() : list;
            return Mapper.Map<List<Domain.Entities.Role>>(list);
        }

        public async Task<List<Domain.Entities.Role>> GetList()
        {
            var list = await _dbContext.Roles.ToListAsync();
            list = list == null ? new List<Model.RoleModel>() : list;
            return Mapper.Map<List<Domain.Entities.Role>>(list);
        }

        public async Task<(IList<Role> Roles, int TotalCount)> GetList(RoleInfoListPagedRequestDto requestDto)
        {
            var queryable = _dbContext.Roles
               .Where(w => w.Id > 0);

            if (!string.IsNullOrEmpty(requestDto.Vague))
            {
                queryable = queryable
                    .Where(w => EF.Functions.Like(w.Id.ToString(), "%" + requestDto.Vague + "%")
                    || EF.Functions.Like(w.Name, "%" + requestDto.Vague + "%"));
            }
            if (requestDto.Enable == 1)
                queryable = queryable.Where(w => w.Enable == true);
            if (requestDto.Enable == 0)
                queryable = queryable.Where(w => w.Enable == false);

            var result = queryable.OrderByDescending(p => p.CreateTime);
            var list = await result.Skip((requestDto.SkipPage - 1) * requestDto.PagedCount).Take(requestDto.PagedCount).ToListAsync();

            return (Mapper.Map<List<Domain.Entities.Role>>(list), result.Count());
        }

        public Task ModifyAsync(Domain.Entities.Role role)
        {
            var model = Mapper.Map<Model.RoleModel>(role);
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