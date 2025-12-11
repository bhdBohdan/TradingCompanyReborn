 
using AutoMapper;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<DALEF.Models.User, DTO.User>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
            .ForMember(d => d.RegistrationDate, o => o.MapFrom(s => s.CreatedAt ?? DateTime.MinValue))
            .ForSourceMember(s => s.Products, o => o.DoNotValidate())
            .ForSourceMember(s => s.Orders, o => o.DoNotValidate())
            .ForSourceMember(s => s.UserProfile, o => o.DoNotValidate())
            .ForSourceMember(s => s.Roles, o => o.DoNotValidate());   // ✔ ADD THIS

            CreateMap<DTO.User, DALEF.Models.User>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.Orders, o => o.Ignore())
                .ForMember(d => d.Roles, o => o.Ignore())
                .ForMember(d => d.UserProfile, o => o.Ignore());
        }
    }
}