using AutoMapper;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class ProductMap : Profile
    {
        public ProductMap()
        {
            CreateMap<Models.Product, DTO.Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)) // relies on UserMap
                                                                                   // Avoid mapping Orders collection on Product DTO (not present)
                .ForSourceMember(src => src.Orders, opt => opt.DoNotValidate());

            CreateMap<DTO.Product, Models.Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                // when mapping DTO -> EF model, ignore EF navigation collections you don't set:
                .ForMember(dest => dest.Orders, opt => opt.Ignore());
        }
    }
}