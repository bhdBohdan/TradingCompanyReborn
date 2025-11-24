using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IProfileManager
    {

        UserProfile GetProfileByUserId(int userId);

        UserProfile CreateDefaultProfile(int userId);

        UserProfile UpdateProfile(UserProfile profile);

    }
}
