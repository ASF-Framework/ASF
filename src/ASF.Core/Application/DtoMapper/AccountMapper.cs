using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;

namespace ASF.Application.DtoMapper
{
    public  class AccountMapper : Profile
    {
        public AccountMapper()
        {
            base.CreateMap<AccountCreateRequestDto, Account>();
            base.CreateMap<Account, AccountInfoResponseDto>()
                .ForMember(f => f.Telephone, p => p.MapFrom(s => s.Telephone.ToString()))
                .ForMember(f => f.CreateTime, p => p.MapFrom(s => s.CreateInfo.CreateTime.ToString()))
                .ForMember(f => f.LoginTime, p => p.MapFrom(s => s.LoginInfo.LoginTime.ToString()));  

        }
    }
}
