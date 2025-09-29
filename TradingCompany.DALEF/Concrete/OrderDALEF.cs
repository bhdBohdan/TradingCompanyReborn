using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class OrderDALEF: GenericDAL<Order>, IOrderDAL
    {
        public OrderDALEF(string connStr, IMapper mapper) : base(connStr, mapper)
        {
        }
        public override Order Create(Order entity)
        {
           using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var dalEntity = _mapper.Map<DALEF.Models.Order>(entity);
                    ctx.Orders.Add(dalEntity);
                    ctx.SaveChanges();
                    entity.Id = dalEntity.OrderId;
                    return entity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating Order: {ex.Message}");
                    return null;
                }
            }
        }
        public override bool Delete(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.Orders.Find(id);
                    if (entity == null) return false;
                    ctx.Orders.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting Order: {ex.Message}");
                    return false;
                }
            }
        }
        public override List<Order> GetAll()
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entities = ctx.Orders.OrderBy(r => r.OrderId).ToList();
                    return _mapper.Map<List<Order>>(entities);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving all Orders: {ex.Message}");
                    return new List<Order>();
                }
            }
        }
        public override Order GetById(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.Orders.Include(p=> p.Product).Include(p => p.Buyer).FirstOrDefault(p => p.OrderId == id);
                    return _mapper.Map<Order>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving Order by Id: {ex.Message}");
                    return null;
                }
            }
        }
        public override Order Update(Order entity)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {

                    if (entity.Id <= 0)
                        throw new ArgumentException("Order ID must be provided for update.");

                    var existingEntity = ctx.Orders.Find(entity.Id);
                    if (existingEntity == null) throw new Exception("Non existing id");


                    if ((entity.ProductId) > 0)
                        existingEntity.ProductId = entity.ProductId;

                    if ((entity.BuyerId) > 0)
                        existingEntity.BuyerId = entity.BuyerId;

                    if ((entity.Quantity) > 0)
                        existingEntity.Quantity = entity.Quantity;

                    if (!string.IsNullOrEmpty(entity.Status)) //'CANCELLED', 'ACTIVE', 'DONE'
                        existingEntity.Status = entity.Status;


                        //_mapper.Map(entity, existingEntity);
                        ctx.SaveChanges();
                    return _mapper.Map<Order>(existingEntity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating Order: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
