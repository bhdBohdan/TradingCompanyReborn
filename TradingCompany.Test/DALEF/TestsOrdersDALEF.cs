using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DALEF.Automapper;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.DALEF.Concrete;

namespace TradingCompany.Test.DALEF
{
    public class TestsOrdersDALEF
    {
        private readonly string _testConnectionString;
        private readonly IMapper _mapper;
        private readonly OrderDALEF _orderDal;

        public TestsOrdersDALEF()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _testConnectionString = config.GetConnectionString("TestConnection");


            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<OrderMap>();
            configExpression.AddProfile<ProductMap>();
            configExpression.AddProfile<UserMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            _mapper = mapperConfig.CreateMapper();

            _orderDal = new OrderDALEF(_testConnectionString, _mapper);
        }

        [Fact]
        public void GetOrderById()
        {
            var orders = _orderDal.GetAll();
            var order = _orderDal.GetById(orders[0].Id);
            Assert.NotNull(order);
            Assert.IsType<int>(order.BuyerId);
        }

        [Fact]
        public void GetAllOrders()
        {
            var orders = _orderDal.GetAll();
            Assert.NotNull(orders);
            Assert.NotEqual(0, orders.Count);
        }

        [Fact]
        public void InsertAndUpdateAndDeleteOrder()
        {
            var orderDTO = new TradingCompany.DTO.Order
            {
                BuyerId = 100001,
                
                ProductId = 100001,
            };
            // Insert
            var order = _orderDal.Create(orderDTO);
            Assert.Equal(orderDTO.BuyerId, order.BuyerId);
            // Update
            order.BuyerId = 100002;
            var updatedOrder = _orderDal.Update(order);
            Assert.Equal(100002, updatedOrder.BuyerId);
            // Delete
            var res = _orderDal.Delete(order.Id);
            Assert.True(res);
            var deletedOrder = _orderDal.GetById(order.Id);
            Assert.Null(deletedOrder);
        }
    }
}
