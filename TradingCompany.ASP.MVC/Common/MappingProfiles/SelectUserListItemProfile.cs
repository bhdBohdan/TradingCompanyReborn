using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TradingCompany.DTO;

namespace TradingCompany.ASP.MVC.Common.MappingProfiles
{
    public class SelectUserListItemProfile : Profile
    {
        public SelectUserListItemProfile()
        {
            CreateMap<User, SelectListItem>()
                .ForMember(dest => dest.Value, src => src.MapFrom(g => g.Id))
                .ForMember(dest => dest.Text, src => src.MapFrom(g => g.Username));
        }
    }
}
