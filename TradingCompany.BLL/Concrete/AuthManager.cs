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
    public class AuthManager : IAuthManager
    {
        private readonly IUserDAL _userDal;
        private readonly IRoleDAL _userRoleDal;

        public AuthManager(IUserDAL userDal, IRoleDAL userRoleDal)
        {
            _userDal = userDal;
            _userRoleDal = userRoleDal;
        }

        public User GetUserById(int id)
        {
           return _userDal.GetById(id);
        }

        public User GetUserByLogin(string username)
        {
            return _userDal.GetUserByLogin(username);
        }

        public List<User> GetUsers()
        {
            return _userDal.GetAll();
        }

        public User Login(string username, string password)
        {
            return _userDal.Login(username, password);
        }

        public User Register(string email, string username, string password, RoleType privilegeType = RoleType.USER)
        {
            User _user = new User
            {
                Email = email,
                Username = username,
                PasswordHash = password,
                Roles = new List<Role>
                {
                    _userRoleDal.GetById((int)privilegeType)
                },
                RegistrationDate = DateTime.Now,
                UpdatedAt = DateTime.Now
                
            };
            return _userDal.Register(_user);
        }
    }
}
