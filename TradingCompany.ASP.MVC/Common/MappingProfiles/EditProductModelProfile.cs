using AutoMapper;

namespace TradingCompany.ASP.MVC.Common.MappingProfiles
{
    public class EditProductModelProfile: Profile
    {
        public EditProductModelProfile()
        {
            CreateMap<TradingCompany.DTO.Product, TradingCompany.ASP.MVC.Models.EditProductModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
