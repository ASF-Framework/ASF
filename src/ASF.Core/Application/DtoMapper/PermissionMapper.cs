using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;

namespace ASF.Application.DtoMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<PermissionActionCreateRequestDto, Permission>();
            base.CreateMap<PermissionMenuCreateRequestDto, Permission>();
            base.CreateMap<Permission, PermissionActionInfoDetailsResponseDto>();
            base.CreateMap<Permission, PermissionMenuInfoDetailsResponseDto>();
            base.CreateMap<Permission, PermissionMenuInfoBaseResponseDto>();
        }
    }
}
