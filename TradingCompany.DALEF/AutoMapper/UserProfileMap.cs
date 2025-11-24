using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class UserProfileMap: Profile
    {
        public UserProfileMap()
        {
            CreateMap<DTO.UserProfile, DALEF.Models.UserProfile>().ForMember(d => d.ProfileId, o => o.Ignore());
            CreateMap<DALEF.Models.UserProfile, DTO.UserProfile>().ForMember(d => d.Id, o => o.MapFrom(s => s.ProfileId));
        }
    }
}
