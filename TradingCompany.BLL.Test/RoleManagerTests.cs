using Moq;
using Xunit;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Test
{
    public class RoleManagerTests
    {
        [Fact]
        public void AddRoleToUser_Delegates_To_DAL()
        {
            var mockDal = new Mock<IRoleDAL>();
            mockDal.Setup(d => d.AddRoleToUser(7, RoleType.ADMIN)).Returns(true);

            var sut = new RoleManager(mockDal.Object);
            var ok = sut.AddRoleToUser(7, RoleType.ADMIN);

            mockDal.Verify(d => d.AddRoleToUser(7, RoleType.ADMIN), Times.Once);
            Assert.True(ok);
        }

        [Fact]
        public void RemoveRoleFromUser_Delegates_To_DAL()
        {
            var mockDal = new Mock<IRoleDAL>();
            mockDal.Setup(d => d.RemoveRoleFromUser(8, RoleType.ADMIN)).Returns(true);

            var sut = new RoleManager(mockDal.Object);
            var ok = sut.RemoveRoleFromUser(8, RoleType.ADMIN);

            mockDal.Verify(d => d.RemoveRoleFromUser(8, RoleType.ADMIN), Times.Once);
            Assert.True(ok);
        }
    }
}