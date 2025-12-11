using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TradingCompany.ASP.MVC.Controllers;
using TradingCompany.ASP.MVC.Models;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using Xunit;

namespace TradingCompany.ASP.MVC.Test
{
    public class AccountControllerTests
    {
        private readonly Mock<IAuthManager> _mockAuthManager;

        public AccountControllerTests()
        {
            _mockAuthManager = new Mock<IAuthManager>();
        }

        private AccountController CreateControllerWithAuthService(Mock<IAuthenticationService> authServiceMock)
        {
            var controller = new AccountController(_mockAuthManager.Object);

            var services = new ServiceCollection();
            services.AddSingleton<IAuthenticationService>(authServiceMock.Object);

            // Provide TempData services required by Controller.TempData
            var tempDataProviderMock = new Mock<ITempDataProvider>();
            services.AddSingleton<ITempDataProvider>(tempDataProviderMock.Object);
            services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

            var provider = services.BuildServiceProvider();

            var context = new DefaultHttpContext
            {
                RequestServices = provider
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            // Create and assign a TempDataDictionary so controller.TempData is available
            controller.TempData = new TempDataDictionary(context, provider.GetRequiredService<ITempDataProvider>());

            return controller;
        }

        [Fact]
        public void Get_Login_ReturnsViewWithModelAndReturnUrl()
        {
            // Arrange
            var controller = new AccountController(_mockAuthManager.Object);

            // Act
            var result = controller.Login("/return/here") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<LoginModel>(result!.Model);
            Assert.Equal("/return/here", controller.ViewData["ReturnUrl"]);
        }

        [Fact]
        public async System.Threading.Tasks.Task Post_Login_ValidCredentials()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "user1", Password = "pass" };

            var user = new User
            {
                Id = 42,
                Username = "user1",
                Roles = new System.Collections.Generic.List<Role> { new Role { Id = (int)RoleType.USER } }
            };

            _mockAuthManager.Setup(m => m.Login(loginModel.Username, loginModel.Password)).Returns(user);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(s => s.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            var controller = CreateControllerWithAuthService(authServiceMock);

            // Act - no returnUrl -> should redirect to Home/Index
            var result = await controller.Login(loginModel, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            authServiceMock.Verify(s => s.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
        }
    

    [Fact]
    public async System.Threading.Tasks.Task Post_Login_InvalidCredentials_ReturnsViewWithModelErrorAsync()
    {
        // Arrange
        var loginModel = new LoginModel { Username = "bad", Password = "wrong" };

            _mockAuthManager.Setup(m => m.Login(loginModel.Username, loginModel.Password)).Returns((User?)null);

            // Auth service should not be called in this case, but still provide one in DI
            var authServiceMock = new Mock<IAuthenticationService>();
            var controller = CreateControllerWithAuthService(authServiceMock);

            // Act
            var result = await controller.Login(loginModel, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Same(loginModel, result!.Model);
            Assert.False(controller.ModelState.IsValid);
            Assert.True(controller.ModelState[string.Empty].Errors.Count > 0);
            authServiceMock.Verify(s => s.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Never);
        }
    }
}