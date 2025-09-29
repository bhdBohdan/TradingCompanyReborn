using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TradingCompany.DALEF.Automapper;
using TradingCompany.DALEF.Concrete;

namespace TradingCompany.Test.DALEF
{
    using Microsoft.Extensions.Configuration;
    using Xunit;

    public class TestsUserDALEF
    {
        private readonly string _testConnectionString;
        private readonly IMapper _mapper;
        private readonly UserDALEF _userDal;

        public TestsUserDALEF()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _userDal = new UserDALEF(_testConnectionString, _mapper);
        }

        [Fact]
        public void GetAll()
        {
            var users = _userDal.GetAll();
            Assert.NotNull(users);
            Assert.NotEqual(0, users.Count);
        }

        [Fact]
        public void GetById()
        {
            var user = _userDal.GetById(100001);
            Assert.NotNull(user);
            Assert.IsType<string>(user.Username);
        }

        [Fact]
        public void Insert()
        {
            var userDTO = new TradingCompany.DTO.User
            {
                Username = "newuser111",
                RestoreKeyword = "keyword",
                PasswordHash = "hashedpassword",
                Email = "email111",
            };
            var user = _userDal.Create(userDTO);
            _userDal.Delete(user.Id);
            Assert.Equal(userDTO.Username, user.Username);
        }

        [Fact]
        public void Delete()
        {
            var userDTO = new TradingCompany.DTO.User
            {
                Username = "delete",
                RestoreKeyword = "me",
                PasswordHash = "plz",
                Email = "email232323",
            };
            var user = _userDal.Create(userDTO);
            var result = _userDal.Delete(user.Id);
            Assert.True(result);
        }

        [Fact]
        public void Update()
        {
            // Arrange: create a user to update
            var userDTO = new TradingCompany.DTO.User
            {
                Username = "user111",
                RestoreKeyword = "keyword111",
                PasswordHash = "hashedpassword111",
                Email = "updateuser@email.com111",
            };

            var createdUser = _userDal.Create(userDTO);

            // Act: update some fields
            createdUser.Username = "updateduser228";
            createdUser.Email = "updateduser@email.com228";
            var updatedUser = _userDal.Update(createdUser);
            _userDal.Delete(updatedUser.Id);


            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("updateduser228", updatedUser.Username);
            Assert.Equal("updateduser@email.com228", updatedUser.Email);
            Assert.True(updatedUser.UpdatedAt.HasValue);

        }
    }
}