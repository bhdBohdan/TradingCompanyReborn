using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using TradingCompany.ASP.MVC.Controllers;
using TradingCompany.ASP.MVC.Models;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.ASP.MVC.Test
{
    public class ProductControllerTests
    {

        private Mock<IProductManager> _mockManager;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<ProductController>> _mockLogger;
        private ProductController _controller;



        public ProductControllerTests()
        {
            _mockManager = new Mock<IProductManager>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_mockManager.Object, _mockMapper.Object, _mockLogger.Object);
        }


        [Fact]
        public void Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<DTO.Product>
            {
                new DTO.Product { Id = 1, ProductName = "Product1" },
                new DTO.Product { Id = 2, ProductName = "Product2" }
            };
            _mockManager.Setup(m => m.GetProducts()).Returns(products);
            // Act
            var result = _controller.Index();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<DTO.Product>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            Assert.Contains(model, p => p.ProductName == "Product1");
            Assert.Contains(model, p => p.ProductName == "Product2");
        }

        [Fact]
        public void Details_ExistingId_ReturnsViewWithProduct()
        {
            // Arrange
            var product = new DTO.Product { Id = 1, ProductName = "Product1" };
            _mockManager.Setup(m => m.GetProductById(1)).Returns(product);
            // Act
            var result = _controller.Details(1);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DTO.Product>(viewResult.ViewData.Model);
            Assert.Equal("Product1", model.ProductName);
        }

        [Fact]
        public void Details_NonExistingId_ReturnsViewWithNullModel()
        {
            // Arrange
            _mockManager.Setup(m => m.GetProductById(99)).Returns((DTO.Product?)null);
            // Act
            var result = _controller.Details(99);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = viewResult.ViewData.Model;
            Assert.Null(model);
        }

        [Fact]
        public void Edit_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new EditProductModel { Id = 1, ProductName = "", UserId = 1 };
            _controller.ModelState.AddModelError("ProductName", "Product Name is required!");

            // Act
            var result = _controller.Edit(1, model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Same(model, result.Model);
        }
    }

}
