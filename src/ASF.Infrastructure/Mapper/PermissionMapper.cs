using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Infrastructure.Mapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<ASF.Infrastructure.Model.Permission, ASF.Domain.Entities.Permission>()
                .ReverseMap();
        }
    }
}
