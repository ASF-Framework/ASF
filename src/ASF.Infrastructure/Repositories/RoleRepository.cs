using ASF.Application.DTO;
using ASF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
       static List<Role> Roles = new List<Role>();
        static RoleRepository()
        {
            Role role = new Role("admin", "admin", 1);
            role.SetPermissions(new List<string>()
            {
                "dashboard","exception","result","permission","role","user"
            });
            Roles.Add(role);
        }
        public Task<Role> AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> GetList(IList<int> ids)
        {
            return Task.FromResult(Roles);
        }

        public Task<List<Role>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<(IList<Role> Roles, int TotalCount)> GetList(RoleInfoListPagedRequestDto requestDto)
        {
            var a = new ValueTuple<IList<Role>, int>(RoleRepository.Roles, RoleRepository.Roles.Count);
            return Task.FromResult(a);
        }

        public Task ModifyAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(int roleId, bool enable)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int primaryKey)
        {
            throw new NotImplementedException();
        }
    }
}
