using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TradingCompany.DALEF.Concrete;

namespace TradingCompany.Test.DALEF
{
    using Microsoft.Extensions.Configuration;
    using TradingCompany.DALEF.AutoMapper;
    using Xunit;

    public class TestsRoleDALEF
    {
        private readonly string _testConnectionString;
        private readonly IMapper _mapper;
        private readonly RoleDALEF _RoleDal;

        

        public TestsRoleDALEF()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<RoleMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _RoleDal = new RoleDALEF(_testConnectionString, _mapper);

            

        }

        [Fact]
        public void GetAll()
        {
            var Roles = _RoleDal.GetAll();
            Assert.NotNull(Roles);
            Assert.NotEqual(0, Roles.Count);
        }

        [Fact]
        public void GetById()
        {
            var Roles = _RoleDal.GetAll();

            var Role = _RoleDal.GetById(Roles[0].Id);
            Assert.NotNull(Role);
            Assert.IsType<string>(Role.RoleName);
        }

        [Fact]
        public void Insert()
        {
            var RoleDTO = new TradingCompany.DTO.Role
            {
                RoleName = "newRole111",
               
            };
            var Role = _RoleDal.Create(RoleDTO);
            _RoleDal.Delete(Role.Id);
            Assert.Equal(RoleDTO.RoleName, Role.RoleName);
        }

        [Fact]
        public void Delete()
        {
            var RoleDTO = new TradingCompany.DTO.Role
            {
                RoleName = "newRole111",

            };
            var Role = _RoleDal.Create(RoleDTO);
            var result = _RoleDal.Delete(Role.Id);
            Assert.True(result);
        }

        [Fact]
        public void Update()
        {
            // Arrange: create a Role to update
            var RoleDTO = new TradingCompany.DTO.Role
            {
                RoleName = "Role111",
               
            };

            var createdRole = _RoleDal.Create(RoleDTO);

            // Act: update some fields
            createdRole.RoleName = "updatedRole22";
            
            var updatedRole = _RoleDal.Update(createdRole);
            _RoleDal.Delete(updatedRole.Id);


            // Assert
            Assert.NotNull(updatedRole);
            Assert.Equal("updatedRole22", updatedRole.RoleName);

        }
    }
}