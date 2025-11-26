using Moq;
using Xunit;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using System.Collections.Generic;

namespace TradingCompany.BLL.Test
{
    public class AuthManagerTests
    {
        [Fact]
        public void GetUserById_Calls_UserDal_And_Returns_User()
        {
            var mockUserDal = new Mock<IUserDAL>();
            var mockRoleDal = new Mock<IRoleDAL>();
            var expected = new User { Id = 42, Username = "bob" };
            mockUserDal.Setup(d => d.GetById(42)).Returns(expected);

            var sut = new AuthManager(mockUserDal.Object, mockRoleDal.Object);
            var result = sut.GetUserById(42);

            Assert.Equal(expected, result);
            mockUserDal.Verify(d => d.GetById(42), Times.Once);
        }

        [Fact]
        public void GetUserByLogin_Calls_UserDal_And_Returns_User()
        {
            var mockUserDal = new Mock<IUserDAL>();
            var mockRoleDal = new Mock<IRoleDAL>();
            var expected = new User { Id = 1, Username = "alice" };
            mockUserDal.Setup(d => d.GetUserByLogin("alice")).Returns(expected);

            var sut = new AuthManager(mockUserDal.Object, mockRoleDal.Object);
            var result = sut.GetUserByLogin("alice");

            Assert.Equal(expected, result);
            mockUserDal.Verify(d => d.GetUserByLogin("alice"), Times.Once);
        }

        [Fact]
        public void GetUsers_Calls_UserDal_And_Returns_List()
        {
            var mockUserDal = new Mock<IUserDAL>();
            var mockRoleDal = new Mock<IRoleDAL>();
            var list = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            mockUserDal.Setup(d => d.GetAll()).Returns(list);

            var sut = new AuthManager(mockUserDal.Object, mockRoleDal.Object);
            var result = sut.GetUsers();

            Assert.Equal(2, result.Count);
            mockUserDal.Verify(d => d.GetAll(), Times.Once);
        }

        [Fact]
        public void Login_Calls_UserDal_Login_And_Returns_User()
        {
            var mockUserDal = new Mock<IUserDAL>();
            var mockRoleDal = new Mock<IRoleDAL>();
            var expected = new User { Id = 7, Username = "login" };
            mockUserDal.Setup(d => d.Login("u", "p")).Returns(expected);

            var sut = new AuthManager(mockUserDal.Object, mockRoleDal.Object);
            var result = sut.Login("u", "p");

            Assert.Equal(expected, result);
            mockUserDal.Verify(d => d.Login("u", "p"), Times.Once);
        }

        [Fact]
        public void Register_Calls_RoleDal_GetById_And_UserDal_Register_With_Role()
        {
            var mockUserDal = new Mock<IUserDAL>();
            var mockRoleDal = new Mock<IRoleDAL>();

            var adminRole = new Role { Id = (int)RoleType.ADMIN, RoleName = "ADMIN" };
            mockRoleDal.Setup(r => r.GetById((int)RoleType.ADMIN)).Returns(adminRole);

            User captured = null;
            mockUserDal.Setup(d => d.Register(It.IsAny<User>()))
                       .Callback<User>(u => captured = u)
                       .Returns<User>(u => { u.Id = 999; return u; });

            var sut = new AuthManager(mockUserDal.Object, mockRoleDal.Object);

            var registered = sut.Register("e@mail", "bob", "secret", RoleType.ADMIN);

            // registration returned object
            Assert.NotNull(registered);
            Assert.Equal(999, registered.Id);

            // the DAL Register was called with a User containing the role fetched from roleDal
            Assert.NotNull(captured);
            Assert.Equal("e@mail", captured.Email);
            Assert.Equal("bob", captured.Username);
            Assert.Equal("secret", captured.PasswordHash);
            Assert.NotNull(captured.Roles);
            Assert.Single(captured.Roles);

            mockRoleDal.Verify(r => r.GetById((int)RoleType.ADMIN), Times.Once);
            mockUserDal.Verify(d => d.Register(It.IsAny<User>()), Times.Once);
        }
    }
}