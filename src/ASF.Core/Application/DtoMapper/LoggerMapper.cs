using ASF.Application.DTO;
using ASF.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DtoMapper
{
    public class LoggerMapper : Profile
    {  
        public LoggerMapper()
        {
            base.CreateMap<Logging, LoggerInfoDetailsResponseDto>();
        }
    }
}
