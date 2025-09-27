using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.Dto;

namespace TradingCompany.DAL.Interfaces
{
    public interface IUserDAL
    {
        User Register(User user);
        List<User> GetAll();
        User GetById(int userId);
        User Update(User user);
        bool Delete(int userId);

    }
}
