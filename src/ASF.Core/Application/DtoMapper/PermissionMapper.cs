using ASF.Application.DTO;
using ASF.Domain.Entities;
using ASF.Domain.Values;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DtoMapper
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            base.CreateMap<PermissionActionCreateRequestDto, Permission>();
            base.CreateMap<PermissionMenuCreateRequestDto, Permission>();
        }
    }
}
