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
            configExpression.AddProfile<UserProfileMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _userProfileDal = new UserProfileDALEF(_testConnectionString, _mapper);
        }
        [Fact]
        public void GetUserProfileByUserId()
        {
            var userProfiles = _userProfileDal.GetAll();
            var userProfile = _userProfileDal.GetById(userProfiles[0].Id);
            Assert.NotNull(userProfile);
            Assert.IsType<string>(userProfile.FirstName);
        }
       
        

        [Fact]
        public void GetAllUserProfiles()
        {
            var userProfiles = _userProfileDal.GetAll();
            Assert.NotNull(userProfiles);
            Assert.NotEqual(0, userProfiles.Count);
        }

        [Fact]
        public void InsertAndDeleteUserProfile()
        {
            // Arrange: create a new profile
            var userProfileDTO = new TradingCompany.DTO.UserProfile
            {
                UserId = 100001, // Make sure this user exists in your test DB!
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Phone = "555-1234",
            };
            var createdProfile = _userProfileDal.Create(userProfileDTO);

            // Assert insert
            Assert.NotNull(createdProfile);
            Assert.Equal(userProfileDTO.FirstName, createdProfile.FirstName);

            // Act & Assert: delete
            var deleteResult = _userProfileDal.Delete(createdProfile.Id);
            Assert.True(deleteResult);
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
                Gender = null,
                
            };
            var updatedProfile = _userProfileDal.Update(userProfileDTO);
            Assert.Equal(userProfileDTO.FirstName, updatedProfile.FirstName);
            Assert.Equal(userProfileDTO.LastName, updatedProfile.LastName);
        }
    }
}
