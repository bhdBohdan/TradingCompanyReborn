using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DALEF.AutoMapper
{
    public class ProductMap: Profile
    {
        public ProductMap()
        {
            CreateMap<TradingCompany.DALEF.Models.Product, TradingCompany.DTO.Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));
            CreateMap<TradingCompany.DTO.Product, TradingCompany.DALEF.Models.Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
