using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class UserProfileDALEF: GenericDAL<UserProfile>, IUserProfileDAL
    {
        public UserProfileDALEF(string connStr, IMapper mapper) : base(connStr, mapper)
        {

        }
        public override UserProfile Create(UserProfile profile)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var dalEntity = _mapper.Map<DALEF.Models.UserProfile>(profile);
                    ctx.UserProfiles.Add(dalEntity);
                    ctx.SaveChanges();
                    profile.Id = dalEntity.ProfileId;
                    return profile;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    // For example: log details, rethrow, or return null
                    // throw; // or return null;
                    Console.WriteLine($"Error creating UserProfile: {ex.Message}");
                    return null;
                }
            }
        }
        public override bool Delete(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.UserProfiles.Find(id);
                if (entity == null) return false;

                    ctx.UserProfiles.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting UserProfile: {ex.Message}");
                    return false;
                }
            }
        }
        public override List<UserProfile> GetAll()
        {

            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entities = ctx.UserProfiles.OrderBy(r => r.ProfileId).ToList();
                    return _mapper.Map<List<UserProfile>>(entities);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving UserProfiles: {ex.Message}");
                    return new List<UserProfile>();
                }
            }

        }
        public override UserProfile GetById(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.UserProfiles.Where(e => e.UserId == id).FirstOrDefault();
                    return _mapper.Map<UserProfile>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving UserProfile by ID: {ex.Message}");
                    return null;
                }
            }
        }
        public override UserProfile Update(UserProfile profile)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if (profile == null || profile.Id <= 0)
                        throw new ArgumentException("Invalid UserProfile entity or ID.");

                    var existingEntity = ctx.UserProfiles.Find(profile.Id);
                    if (existingEntity == null) throw new Exception("Non existing id");

                    existingEntity.FirstName = string.IsNullOrEmpty(profile.FirstName) ? existingEntity.FirstName : profile.FirstName;
                    existingEntity.LastName = string.IsNullOrEmpty(profile.LastName) ? existingEntity.LastName : profile.LastName;
                    existingEntity.Address = string.IsNullOrEmpty(profile.Address) ? existingEntity.Address : profile.Address;
                    existingEntity.Phone = string.IsNullOrEmpty(profile.Phone) ? existingEntity.Phone : profile.Phone;
                    existingEntity.Gender = string.IsNullOrEmpty(profile.Gender) ? existingEntity.Gender : profile.Gender;

                    // Update BankCardNumber if provided (preserve existing when null/empty)
                    // (uncomment / adjust if your DTO has this property)
                    existingEntity.BankCardNumber = string.IsNullOrEmpty(profile.BankCardNumber) ? existingEntity.BankCardNumber : profile.BankCardNumber;

                    // Update profile picture only when non-null and non-empty to avoid clearing existing image
                    if (profile.ProfilePicture != null && profile.ProfilePicture.Length > 0)
                    {
                        existingEntity.ProfilePicture = profile.ProfilePicture;
                    }

                    existingEntity.UpdatedAt = DateTime.UtcNow;

                    //_mapper.Map(profile, existingEntity);
                    ctx.SaveChanges();
                    return _mapper.Map<UserProfile>(existingEntity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating UserProfile: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
