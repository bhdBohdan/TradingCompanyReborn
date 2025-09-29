using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.Test.DALEF
{
    using AutoMapper;
    using Microsoft.Extensions.Configuration;
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

            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            _testConnectionString = config.GetConnectionString("TestConnection");


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
        public void InsertUpdateAndDeleteUserProfile()
        {
            // Arrange: create a new profile
            var userProfileDTO = new TradingCompany.DTO.UserProfile
            {
                UserId = 100001, //
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Phone = "555-1234",
            };
            var createdProfile = _userProfileDal.Create(userProfileDTO);

            Assert.NotNull(createdProfile);
            Assert.Equal(userProfileDTO.FirstName, createdProfile.FirstName);

            var userUpdateProfileDTO = new TradingCompany.DTO.UserProfile
            {
                Id = createdProfile.Id,
                UserId = createdProfile.UserId,
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                Phone = "555-5678",
                Gender = null,

            };
            var updatedProfile = _userProfileDal.Update(userUpdateProfileDTO);

            Assert.Equal(userUpdateProfileDTO.FirstName, updatedProfile.FirstName);
            Assert.Equal(userUpdateProfileDTO.LastName, updatedProfile.LastName);


            // Assert insert
        

            // Act & Assert: delete
            var deleteResult = _userProfileDal.Delete(updatedProfile.Id);
            Assert.True(deleteResult);
        }

      
    }
}
