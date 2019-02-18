using AutoMapper;

namespace ASF.Infrastructure.ModelMapper
{
    public class LogInfoMapper : Profile
    {
        public LogInfoMapper()
        {
            base.CreateMap<ASF.Infrastructure.Model.LogInfoModel, ASF.Domain.Entities.Logging>().ReverseMap();
        }
    }
}
