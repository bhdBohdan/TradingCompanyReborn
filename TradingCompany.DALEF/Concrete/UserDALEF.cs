using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;
using TradingCompany.Dto;

namespace TradingCompany.DALEF.Concrete
{
    public class UserDALEF : IUserDAL
    {
        private readonly string _connStr;

        public UserDALEF(string connStr)
        {
            _connStr = connStr;

        }
        public bool Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            using(var ctx = new TradingCompContext(_connStr))
            {
                var users = ctx.Users.Select(elem => new User
                {
                    UserId = elem.UserId,
                    Username = elem.Username,
                    RestoreKeyword = elem.RestoreKeyword,
                    Email = elem.Email,
                    PasswordHash = elem.PasswordHash,
                    RegistrationDate = (DateTime)elem.CreatedAt,

                }).ToList();
                return users;
            }

        }

        public User GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public User Register(User user)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                var entity = new DALEF.Models.User
                {
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    RestoreKeyword = user.RestoreKeyword,
                    CreatedAt = new DateTime(),
                };
                ctx.Users.Add(entity);
                ctx.SaveChanges();
                user.UserId = entity.UserId;
                user.RegistrationDate = (DateTime)entity.CreatedAt;
                return user ;
            }
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
