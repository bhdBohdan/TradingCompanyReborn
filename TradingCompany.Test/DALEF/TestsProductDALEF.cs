using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.DALEF.Concrete;

namespace TradingCompany.Test.DALEF
{
    public class TestsProductDALEF
    {
        private readonly string _testConnectionString;
        private readonly IMapper _mapper;
        private readonly ProductDALEF _productDal;

        public TestsProductDALEF()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _testConnectionString = config.GetConnectionString("TestConnection");


            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ProductMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _productDal = new ProductDALEF(_testConnectionString, _mapper);
        }


        [Fact]
        public void GetProductById()
        {
            var products = _productDal.GetAll();
            var product = _productDal.GetById(products[0].Id);
            Assert.NotNull(product);
            Assert.IsType<string>(product.ProductName);
        }

        [Fact]
        public void GetAllProducts()
        {
            var products = _productDal.GetAll();
            Assert.NotNull(products);
            Assert.NotEqual(0, products.Count);
        }

        [Fact]
        public void InsertAndUpdateAndDeleteProduct()
        {
            var productDTO = new TradingCompany.DTO.Product
            {
                UserId = 100001,
                ProductName = "NewProduct123",
                Description = "A new product for testing",
                Category = "Category",
                Price = 19.99m,
      
            };
            var product = _productDal.Create(productDTO);
            Assert.Equal(productDTO.ProductName, product.ProductName);
            Assert.Equal(productDTO.Description, product.Description);
            Assert.Equal(productDTO.Price, product.Price);



            productDTO.ProductName = "UpdatedProduct123";
            var updatedProduct = _productDal.Update(productDTO);
            Assert.Equal(productDTO.ProductName, updatedProduct.ProductName);



            var deleteResult = _productDal.Delete(product.Id);
            Assert.True(deleteResult);
        }


    }
}
