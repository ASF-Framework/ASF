using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;

namespace ASF.Application.DtoMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<Permission, PermissionActionInfoDetailsResponseDto>();
            base.CreateMap<Permission, PermissionMenuInfoDetailsResponseDto>()
                .ForPath(f => f.Icon, p => p.MapFrom(s => s.MenuIcon))
                .ForPath(f => f.Hidden, p => p.MapFrom(s => s.MenuHidden))
                .ForPath(f => f.Redirect, p => p.MapFrom(s => s.MenuRedirect))
                .ForPath(f => f.Template, p => p.MapFrom(s => s.ApiTemplate));
            base.CreateMap<Permission, PermissionMenuInfoBaseResponseDto>();
            base.CreateMap<Permission, PermissionOpenApiInfoDetailsResponseDto>();

        }
    }
}
