using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete.ctx;
using TradingCompany.DALEF;
using TradingCompany.DTO;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.DALEF.Concrete
{
    public class ProductDALEF: GenericDAL<Product>, IProductDAL
    {
        public ProductDALEF(string connStr, IMapper mapper) : base(connStr, mapper)
        {
        }

        public override Product Create(Product entity)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var dalEntity = _mapper.Map<DALEF.Models.Product>(entity);
                    ctx.Products.Add(dalEntity);
                    ctx.SaveChanges();
                    entity.Id = dalEntity.ProductId;
                    return entity;
                }
                catch (Exception ex)
                {               
                    Console.WriteLine($"Error creating Product: {ex.Message}");
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
                    var entity = ctx.Products.Find(id);
                    if (entity == null) return false;
                    ctx.Products.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting Product: {ex.Message}");
                    return false;
                }
            }
        }

        public override List<Product> GetAll()
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entities = ctx.Products.OrderBy(r => r.ProductId).ToList();
                    return _mapper.Map<List<Product>>(entities);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving all Products: {ex.Message}");
                    return new List<Product>();
                }
            }
        }

        public override Product GetById(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.Products.Include(p => p.User).FirstOrDefault(p => p.ProductId == id);
                    return _mapper.Map<Product>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving Product by Id: {ex.Message}");
                    return null;
                }
            }
        }

        public override Product Update(Product entity)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if (entity.Id <= 0)
                        throw new ArgumentException("Product ID must be provided for update.");

                    var existingEntity = ctx.Products.Find(entity.Id);
                    if (existingEntity == null) throw new Exception("Non existing id");
                

                    if (!string.IsNullOrEmpty(entity.ProductName))
                        existingEntity.ProductName = entity.ProductName;

                    if (!string.IsNullOrEmpty(entity.Description))
                        existingEntity.Description = entity.Description;

                    if ((entity.Price) > 0)
                        existingEntity.Price = entity.Price;

                    if (!string.IsNullOrEmpty(entity.Category))
                       existingEntity.Category = entity.Category;

                    

                   // _mapper.Map(entity, existingEntity);
                    ctx.SaveChanges();
                    return _mapper.Map<Product>(existingEntity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating Product: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
