using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Infrastructure.ModelMapper
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            base.CreateMap<ASF.Infrastructure.Model.Role, ASF.Domain.Entities.Role>()
              .ForMember(o => o.Permissions, model => model.MapFrom(d => new List<string>(d.Permissions.Split(','))));

            base.CreateMap<ASF.Domain.Entities.Role,ASF.Infrastructure.Model.Role > ()
              .ForMember(o => o.Permissions, e => e.MapFrom(d => string.Join(",", d.Permissions)));
        }
    }
}
