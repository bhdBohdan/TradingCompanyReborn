using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DALEF.AutoMapper
{
    public class RoleMap : Profile
    {
        public RoleMap()
        {
            CreateMap<TradingCompany.DALEF.Models.Role, TradingCompany.DTO.Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId));
            CreateMap<TradingCompany.DTO.Role, TradingCompany.DALEF.Models.Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));


        }


    }
}
