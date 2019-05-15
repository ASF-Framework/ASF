using AutoMapper;

namespace ASF.EntityFramework.ModelMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<Model.PermissionModel, ASF.Domain.Entities.Permission>()
                .ReverseMap();
        }
    }
}
