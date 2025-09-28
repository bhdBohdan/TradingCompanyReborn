
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using AutoMapper;


namespace TradingCompany.DALEF.Concrete
{
    public class UserDALEF : GenericDAL<User>, IUserDAL
    {    
        public UserDALEF(string connStr, IMapper mapper) : base(connStr, mapper)
        {       
        }
        public override bool Delete(int userId)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                var entity = ctx.Users.Find(userId);
                if (entity == null) return false;

                ctx.Users.Remove(entity);
                ctx.SaveChanges();
                return true;
            }
        }

        public override List<User> GetAll()
        {
            using(var ctx = new TradingCompContext(_connStr))
            {
                var users = ctx.Users.Take(50);
                return _mapper.Map<List<User>>(users);
            }

        }

        public override User GetById(int userId)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                var user = ctx.Users.Find(userId);
                if (user == null) return null;
                  
                //use mapper here too
                return _mapper.Map<User>(user);
            }

        }

        public override User Create(User user)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                var entity = _mapper.Map<DALEF.Models.User>(user);
                entity.CreatedAt = DateTime.Now;
               // entity.UpdatedAt = DateTime.Now;
                ctx.Users.Add(entity);
                ctx.SaveChanges();
                user.Id = entity.UserId;
                user.RegistrationDate = (DateTime)entity.CreatedAt;
                return user;
            }
        }

        public override User Update(User user) //works as patch as well
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                var entity = ctx.Users.Find(user.Id);
                if (entity == null) return null;

                if (!string.IsNullOrEmpty(user.Username))
                    entity.Username = user.Username;

                if (!string.IsNullOrEmpty(user.Email))
                    entity.Email = user.Email;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                    entity.PasswordHash = user.PasswordHash;

                if (!string.IsNullOrEmpty(user.RestoreKeyword))
                    entity.RestoreKeyword = user.RestoreKeyword;

                entity.UpdatedAt = DateTime.Now;
                ctx.SaveChanges();
                return _mapper.Map<User>(entity);
            }
        }

    }
}
