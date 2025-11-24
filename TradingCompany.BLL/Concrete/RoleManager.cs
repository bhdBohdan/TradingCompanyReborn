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
    public class RoleManager : IRoleManager
    {
      
        private readonly IRoleDAL _userRoleDal;

        public RoleManager(IRoleDAL userRoleDal) {
            _userRoleDal = userRoleDal;
        }

        public bool AddRoleToUser(int userId, RoleType roleType)
        {
            return _userRoleDal.AddRoleToUser(userId, roleType);
        }

        public bool RemoveRoleFromUser(int userId, RoleType roleType)
        {
           return _userRoleDal.RemoveRoleFromUser(userId, roleType);
        }
    }
}
