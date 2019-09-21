using ASF.Domain.Entities;
using ASF.Domain.Values;
using ASF.EntityFramework.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.EntityFramework.ModelMapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            base.CreateMap<AccountModel, Account>(MemberList.None)
                .ForPath(a => a.CreateInfo.CreateId, model => model.MapFrom(d => d.CreateId))
                .ForPath(a => a.CreateInfo.CreateTime, model => model.MapFrom(d => d.CreateTime))
                .ForPath(a => a.LoginInfo.LoginIp, model => model.MapFrom(d => d.LoginIp))
                .ForPath(a => a.LoginInfo.LoginTime, model => model.MapFrom(d => d.LoginTime))
                .ForPath(a => a.LoginInfo.AccessToken, model => model.MapFrom(d => new AccessToken(d.Token, d.RefreshToken, d.Expired)))
                .ForPath(a => a.Telephone, model => model.MapFrom(d => new PhoneNumber(d.Telephone)))
                .ForMember(a => a.Roles, model => model.MapFrom(d => d.Roles.Split(',').Select<string, int>(q =>  string.IsNullOrEmpty(q) ? 0 : Convert.ToInt32(q)).ToList()));

            base.CreateMap<Account, AccountModel>(MemberList.None)
                .ForPath(a => a.CreateId, model => model.MapFrom(d => d.CreateInfo.CreateId))
                .ForPath(a => a.CreateTime, model => model.MapFrom(d => d.CreateInfo.CreateTime))
                .ForPath(a => a.LoginIp, model => model.MapFrom(d => d.LoginInfo.LoginIp))
                .ForPath(a => a.LoginTime, model => model.MapFrom(d => d.LoginInfo.LoginTime))
                .ForPath(a => a.Token, model => model.MapFrom(d => d.LoginInfo.AccessToken.Token))
                .ForPath(a => a.RefreshToken, model => model.MapFrom(d => d.LoginInfo.AccessToken.RefreshToken))
                .ForPath(a => a.Expired, model => model.MapFrom(d => d.LoginInfo.AccessToken.Expired))
                .ForPath(a => a.Telephone, model => model.MapFrom(d => d.Telephone.ToString()))
                .ForMember(a => a.Roles, model => model.MapFrom(d => string.Join(",", d.Roles)));
        }
    }
}
