using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;


namespace TradingCompany.DALEF.Concrete
{
    public class UserDALEF : GenericDAL<DTO.User>, IUserDAL
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

        public override List<DTO.User> GetAll()
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var users = ctx.Users.OrderBy(u => u.UserId).ToList();
                    return _mapper.Map<List<DTO.User>>(users);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving all Users: {ex.Message}");
                    return new List<DTO.User>();
                }

            }

        }

        public override DTO.User GetById(int userId)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var user = ctx.Users.Find(userId);
                    if (user == null) return null;
                    //use mapper here too
                    return _mapper.Map<DTO.User>(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving User by ID: {ex.Message}");
                    return null;
                }

            }

        }

        public override DTO.User Create(DTO.User user)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if (ctx.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
                    {
                        throw new Exception("A user with the same username or email already exists.");
                    }
                    var entity = _mapper.Map<DALEF.Models.User>(user);
                    entity.CreatedAt = DateTime.Now;
                    // Set UpdatedAt to null on initial insert to satisfy CHECK constraint (UpdatedAt IS NULL OR UpdatedAt > CreatedAt)
                    entity.UpdatedAt = null;
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

        public override DTO.User Update(DTO.User user) //works as patch as well
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

                    // For updates set UpdatedAt to current time (must be greater than CreatedAt per constraint)
                    entity.UpdatedAt = DateTime.Now;
                    ctx.SaveChanges();
                    return _mapper.Map<DTO.User>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating User: {ex.Message}");
                    return null;
                }
            }
        }

        public DTO.User GetUserByLogin(string username)
        {
         
            using (var context = new TradingCompContext(_connStr))
            {
                try {
                    var user = context.Users.SingleOrDefault(u => u.Username == username);
                    if (user == null) return null;
                    return _mapper.Map<DTO.User>(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving User by login: {ex.Message}");
                    return null;
                }
              

            }
        }

        public DTO.User Register(DTO.User user)
        {
            using(var context = new TradingCompContext(_connStr))
            {
                try
                {
                    if (context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
                        return null;

                    byte[] passwordHash = hash(user.PasswordHash, user.Email.ToString());

                    // Map DTO -> EF but DO NOT map navigation collections
                    var entity = _mapper.Map<DALEF.Models.User>(user);

                    entity.PasswordHash = Convert.ToBase64String(passwordHash);
                    entity.CreatedAt = DateTime.Now;
                    entity.RestoreKeyword = "default_keyword";

                    // Ensure UpdatedAt is null on creation to satisfy constraint
                    entity.UpdatedAt = null;

                    // Attach existing Role entities from DB instead of mapping new ones
                    if (user.Roles != null && user.Roles.Any())
                    {
                        var roleIds = user.Roles.Select(r => r.Id).ToList();
                        entity.Roles = context.Roles.Where(r => roleIds.Contains(r.RoleId)).ToList();
                    }
                    else
                    {
                        // fallback default role (adjust RoleType/Id as needed)
                        var defaultRole = context.Roles.Find((int)RoleType.USER);
                        if (defaultRole != null) entity.Roles = new List<DALEF.Models.Role> { defaultRole };
                    }

                    context.Users.Add(entity);
                    context.SaveChanges();

                    // Create an empty UserProfile if your app expects one
                    if (entity.UserProfile == null)
                    {
                        var profileEntity = new DALEF.Models.UserProfile
                        {
                            UserId = entity.UserId,
                            FirstName = $"User{entity.UserId}",
                            LastName = $"User{entity.UserId}",
                            BankCardNumber = "None",
                            Phone = "None",
                            Address = "Unknown",
                            Gender = "Other",
                            // leave UpdatedAt null so any CHECK constraints are satisfied
                            UpdatedAt = null
                        };
                        context.UserProfiles.Add(profileEntity);
                        context.SaveChanges();
                    }

                    return _mapper.Map<DTO.User>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during registration: {ex.Message}");
                    return null;
                }
               
            }
        }

        public DTO.User Login(string username_or_email, string password)
        {
            using (var context = new TradingCompContext(_connStr))
            {
                try
                {
                    var user = context.Users.SingleOrDefault(u => u.Username == username_or_email || u.Email == username_or_email);
                    if (user == null) return null;

                    byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
                    byte[] inputHash = hash(password, user.Email.ToString());
                    return storedHash.SequenceEqual(inputHash) ? _mapper.Map<DTO.User>(user) : null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during login: {ex.Message}");
                    return null;
                }
            }
        }

        private byte[] hash(string password, string salt)
        {
           
            var alg = SHA256.Create();
            return alg.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        }

      
    }
}
