using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;

namespace ASF.Application.DtoMapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            base.CreateMap<AccountCreateRequestDto, Account>();
            base.CreateMap<Account, AccountBaseInfoResponseDto>()
                .ForPath(f => f.Telephone, p => p.MapFrom(s => s.Telephone.ToString()))
                .ForPath(f => f.CreateTime, p => p.MapFrom(s => s.CreateInfo.CreateTime.ToString()))
                .ForPath(f => f.LoginTime, p => p.MapFrom(s => s.LoginInfo.LoginTime.ToString()));
            base.CreateMap<Account, AccountDetailsResponseDto>()
                .IncludeBase<Account, AccountBaseInfoResponseDto>()
                .ForPath(f => f.LoginIp, p => p.MapFrom(s => s.LoginInfo.LoginIp));
        }
    }
}
