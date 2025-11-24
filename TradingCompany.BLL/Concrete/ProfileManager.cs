using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class ProfileManager: IProfileManager
    {
        private readonly IUserProfileDAL _userProfileDAL;

        public ProfileManager(IUserProfileDAL userProfileDAL)
        {
            _userProfileDAL = userProfileDAL;
        }

        public UserProfile GetProfileByUserId(int userId)
        {
            return _userProfileDAL.GetById(userId);
        }

        public UserProfile CreateDefaultProfile(int userId)
        {
            var profile = new UserProfile
            {
                UserId = userId,
                FirstName = "New",
                LastName = $"User+{userId}",
                Gender = "unknown",
                Phone = "None",
                Address = "Unknown",
                ProfilePicture = null,
                UpdatedAt = DateTime.UtcNow 

            };
            _userProfileDAL.Create(profile);
            return profile;
        }

        public UserProfile UpdateProfile(UserProfile profile)
        {
            return _userProfileDAL.Update(profile);
        }
    }
}
