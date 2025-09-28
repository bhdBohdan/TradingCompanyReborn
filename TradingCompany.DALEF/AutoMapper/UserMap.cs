using AutoMapper;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Automapper
{

    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<TradingCompany.DALEF.Models.User, TradingCompany.DTO.User>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            CreateMap<TradingCompany.DTO.User, TradingCompany.DALEF.Models.User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}