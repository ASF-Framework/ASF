using AutoMapper;

namespace ASF.Infrastructure.ModelMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<ASF.Infrastructure.Model.PermissionModel, ASF.Domain.Entities.Permission>()
                .ReverseMap();
        }
    }
}
