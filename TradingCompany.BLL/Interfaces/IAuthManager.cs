using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IAuthManager
    {
        User Login(string username, string password);
        User Register(string email, string username, string password, RoleType privilegeType = RoleType.USER);
        User GetUserByLogin(string username);
        User GetUserById(int id);
        List<User> GetUsers();
    }
}
