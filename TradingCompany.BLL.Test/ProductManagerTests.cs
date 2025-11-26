using Moq;
using Xunit;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using System.Collections.Generic;

namespace TradingCompany.BLL.Test
{
    public class ProductManagerTests
    {
        [Fact]
        public void AddProduct_Calls_Create_On_DAL()
        {
            var mockDal = new Mock<IProductDAL>();
            var dto = new Product { Id = 1, ProductName = "Test", Price = 1m };
            mockDal.Setup(d => d.Create(It.IsAny<Product>())).Returns((Product p) => p);

            var sut = new ProductManager(mockDal.Object);
            var result = sut.AddProduct(dto);

            mockDal.Verify(d => d.Create(It.Is<Product>(x => x == dto)), Times.Once);
            Assert.Equal("Test", result.ProductName);
        }

        [Fact]
        public void GetProducts_Calls_GetAll_On_DAL()
        {
            var mockDal = new Mock<IProductDAL>();
            var list = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            mockDal.Setup(d => d.GetAll()).Returns(list);

            var sut = new ProductManager(mockDal.Object);
            var result = sut.GetProducts();

            mockDal.Verify(d => d.GetAll(), Times.Once);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void DeleteProduct_Calls_Delete_On_DAL_Returns_Result()
        {
            var mockDal = new Mock<IProductDAL>();
            mockDal.Setup(d => d.Delete(5)).Returns(true);

            var sut = new ProductManager(mockDal.Object);
            var ok = sut.DeleteProduct(5);

            mockDal.Verify(d => d.Delete(5), Times.Once);
            Assert.True(ok);
        }

        [Fact]
        public void UpdateProduct_Calls_Update_On_DAL_And_Returns_Updated()
        {
            var mockDal = new Mock<IProductDAL>();
            var input = new Product { Id = 10, ProductName = "Before" };
            var updated = new Product { Id = 10, ProductName = "After" };
            mockDal.Setup(d => d.Update(It.IsAny<Product>())).Returns(updated);

            var sut = new ProductManager(mockDal.Object);
            var result = sut.UpdateProduct(input);

            mockDal.Verify(d => d.Update(It.Is<Product>(p => p == input)), Times.Once);
            Assert.Equal("After", result.ProductName);
        }
    }
}