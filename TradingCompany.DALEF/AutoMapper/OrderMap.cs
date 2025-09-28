using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DALEF.AutoMapper
{
    public class OrderMap: Profile
    {
        public OrderMap()
        {
            CreateMap<TradingCompany.DALEF.Models.Order, TradingCompany.DTO.Order>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId));
            CreateMap<TradingCompany.DTO.Order, TradingCompany.DALEF.Models.Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
