using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IRoleManager
    {
        bool AddRoleToUser(int userId, RoleType roleType);
        bool RemoveRoleFromUser(int userId, RoleType roleType);

       
    }
}
