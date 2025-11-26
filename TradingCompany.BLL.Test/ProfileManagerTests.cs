using Moq;
using Xunit;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using System;

namespace TradingCompany.BLL.Test
{
    public class ProfileManagerTests
    {
        [Fact]
        public void GetProfileByUserId_Calls_GetById_On_DAL()
        {
            var mockDal = new Mock<IUserProfileDAL>();
            var expected = new UserProfile { Id = 3, UserId = 42, FirstName = "X" };
            mockDal.Setup(d => d.GetById(42)).Returns(expected);

            var sut = new ProfileManager(mockDal.Object);
            var res = sut.GetProfileByUserId(42);

            mockDal.Verify(d => d.GetById(42), Times.Once);
            Assert.Equal(expected, res);
        }

        [Fact]
        public void CreateDefaultProfile_Constructs_Defaults_And_Calls_Create()
        {
            var mockDal = new Mock<IUserProfileDAL>();
            UserProfile captured = null!;
            mockDal.Setup(d => d.Create(It.IsAny<UserProfile>()))
                   .Callback<UserProfile>(u => captured = u)
                   .Returns((UserProfile u) => u);

            var sut = new ProfileManager(mockDal.Object);
            var result = sut.CreateDefaultProfile(77);

            mockDal.Verify(d => d.Create(It.IsAny<UserProfile>()), Times.Once);
            Assert.NotNull(captured);
            Assert.Equal(77, captured.UserId);
            Assert.Equal("New", captured.FirstName);
            Assert.StartsWith("User+", captured.LastName);
            Assert.Equal(result, captured); // ProfileManager returns the created instance it constructed
        }

        [Fact]
        public void UpdateProfile_Calls_Update_On_DAL_And_Returns_Result()
        {
            var mockDal = new Mock<IUserProfileDAL>();
            var input = new UserProfile { Id = 9, UserId = 5, FirstName = "A" };
            var updated = new UserProfile { Id = 9, UserId = 5, FirstName = "B" };
            mockDal.Setup(d => d.Update(It.IsAny<UserProfile>())).Returns(updated);

            var sut = new ProfileManager(mockDal.Object);
            var res = sut.UpdateProfile(input);

            mockDal.Verify(d => d.Update(It.Is<UserProfile>(p => p == input)), Times.Once);
            Assert.Equal("B", res.FirstName);
        }
    }
}