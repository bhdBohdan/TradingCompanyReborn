using AutoMapper;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Automapper
{

    public class UserMap : Profile
    {
        public UserMap()
        {
           
            CreateMap<DTO.User, DALEF.Models.User>()
                .ForMember(d => d.Roles, opt => opt.Ignore())
                .ForMember(d => d.UserProfile, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id)); // if needed

            CreateMap<DALEF.Models.User, DTO.User>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles))
                .ForMember(d => d.RegistrationDate, opt => opt.MapFrom(s => s.CreatedAt));


        }
    }
}