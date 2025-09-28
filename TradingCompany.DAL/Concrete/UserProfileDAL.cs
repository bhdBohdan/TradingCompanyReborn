using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using AutoMapper;


namespace TradingCompany.DAL.Concrete
{
    public class UserProfileDAL : GenericDAL<UserProfile>, IUserProfileDAL
    {
        public UserProfileDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }

        public override UserProfile Create(UserProfile entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override List<UserProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public override UserProfile GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override UserProfile Update(UserProfile entity)
        {
            throw new NotImplementedException();
        }
    }
}
