using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.Test.DALEF
{
    using AutoMapper;
    using Microsoft.Extensions.Logging.Abstractions;
    using TradingCompany.DALEF.Automapper;
    using TradingCompany.DALEF.AutoMapper;
    using TradingCompany.DALEF.Concrete;
    using Xunit;
    public class TestsUserProfileDALEF
    {
        private readonly string _testConnectionString;
        private readonly IMapper _mapper;
        private readonly UserProfileDALEF _userProfileDal;

        public TestsUserProfileDALEF()
        {
            _testConnectionString = "Data Source=localhost,1433;Database=TestTradingCompany;User ID=sa;Password=MyStr0ng!Pass123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _userProfileDal = new UserProfileDALEF(_testConnectionString, _mapper);
        }
        [Fact]
        public void GetUserProfileByUserId()
        {
            var userProfile = _userProfileDal.GetById(100001);
            Assert.NotNull(userProfile);
            Assert.IsType<string>(userProfile.FirstName);
        }

        [Fact]
        public void InsertUserProfile()
        {
            _userProfileDal.Delete(100001);
            var userProfileDTO = new TradingCompany.DTO.UserProfile
            {
                UserId = 100001,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Phone = "555-1234",
                
                
            };
            var userProfile = _userProfileDal.Create(userProfileDTO);
            //_userProfileDal.Delete(userProfile.Id);
            Assert.Equal(userProfileDTO.FirstName, userProfile.FirstName);
            Assert.False(string.IsNullOrEmpty(userProfile.FirstName));
        }

        [Fact]
        public void GetAllUserProfiles()
        {
            var userProfiles = _userProfileDal.GetAll();
            Assert.NotNull(userProfiles);
            Assert.NotEqual(0, userProfiles.Count);
        }

        [Fact]
        public void DeleteUserProfile()
        {        
            
            var result = _userProfileDal.Delete(100001);
            Assert.True(result);
        }

        [Fact]
        public void UpdateUserProfile()
        {
            var userProfileDTO = new TradingCompany.DTO.UserProfile
            {
                Id = 100007,
                UserId = 100007,
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                Phone = "555-5678",
                
            };
            var updatedProfile = _userProfileDal.Update(userProfileDTO);
            Assert.Equal(userProfileDTO.FirstName, updatedProfile.FirstName);
            Assert.Equal(userProfileDTO.LastName, updatedProfile.LastName);
        }
    }
}
