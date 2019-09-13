using AutoMapper;
using System.Linq;
using System.Net.Http;

namespace ASF.EntityFramework.ModelMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<Model.PermissionModel, ASF.Domain.Entities.Permission>()
                .ForMember(m => m.HttpMethods, s => s.MapFrom(f => f.HttpMethods.Split(',').Select(t => new HttpMethod(t)).ToList()));

            base.CreateMap<ASF.Domain.Entities.Permission, Model.PermissionModel>()
               .ForMember(m => m.HttpMethods, s => s.MapFrom(f => string.Join(",", f.HttpMethods)));
        }
    }
}
