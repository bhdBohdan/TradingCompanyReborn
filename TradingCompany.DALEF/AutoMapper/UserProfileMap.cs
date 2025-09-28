using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DALEF.AutoMapper
{
    public class UserProfileMap: Profile
    {
        public UserProfileMap()
        {
            CreateMap<TradingCompany.DALEF.Models.UserProfile, TradingCompany.DTO.UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProfileId));
            CreateMap<TradingCompany.DTO.UserProfile, TradingCompany.DALEF.Models.UserProfile>()
                .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
