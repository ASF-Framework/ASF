using ASF.EntityFramework.Model;
using AutoMapper;

namespace ASF.EntityFramework.ModelMapper
{
    public class LogInfoMapper : Profile
    {
        public LogInfoMapper()
        {
            base.CreateMap<LogInfoModel, ASF.Domain.Entities.Logging>().ReverseMap();
        }
    }
}
