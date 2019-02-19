using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;

namespace ASF.Application.DtoMapper
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            base.CreateMap<Role, RoleInfoSimpleResponseDto>();
            base.CreateMap<Role, RoleInfoBaseResponseDto>()
                .ForPath(f => f.CreateTime, m => m.MapFrom(s => s.CreateInfo.CreateTime));

            
        }
    }
}
