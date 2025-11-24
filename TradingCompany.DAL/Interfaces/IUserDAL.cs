using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.DAL.Interfaces
{
    public interface IUserDAL: IGenericDAL<DTO.User>
    {
        DTO.User GetUserByLogin(string username_or_email);
        DTO.User Register(DTO.User user);
        DTO.User Login(string username_or_email, string password);

    }
}
