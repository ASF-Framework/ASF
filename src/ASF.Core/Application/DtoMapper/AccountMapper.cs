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
        }
    }
}
