
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using AutoMapper;
using System.Data;
using TradingCompany.DALEF.Concrete.ctx;


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
                try
                {
                    var entity = ctx.Users.Find(userId);
                    if (entity == null) return false;

                    ctx.Users.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting User: {ex.Message}");
                    return false;
                }
            }
        }

        public override List<User> GetAll()
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var users = ctx.Users.OrderBy(u => u.UserId).ToList();
                    return _mapper.Map<List<User>>(users);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving all Users: {ex.Message}");
                    return new List<User>();
                }

            }

        }

        public override User GetById(int userId)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var user = ctx.Users.Find(userId);
                    if (user == null) return null;
                    //use mapper here too
                    return _mapper.Map<User>(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving User by ID: {ex.Message}");
                    return null;
                }

            }

        }

        public override User Create(User user)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if (ctx.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
                    {
                        // User with the same username or email already exists
                        throw new Exception("A user with the same username or email already exists.");
                    }
                    var entity = _mapper.Map<DALEF.Models.User>(user);
                    entity.CreatedAt = DateTime.Now;
                    // entity.UpdatedAt = DateTime.Now;
                    ctx.Users.Add(entity);
                    ctx.SaveChanges();
                    user.Id = entity.UserId;
                    user.RegistrationDate = (DateTime)entity.CreatedAt;
                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking existing User: {ex.Message}");
                    return null;
                }

            }
        }

        public override User Update(User user) //works as patch as well
        {

            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if (user.Id <= 0)
                        throw new ArgumentException("User ID must be provided for update.");

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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating User: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
