using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF;
using TradingCompany.DALEF.Concrete.ctx;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.Concrete
{
    public class RoleDALEF : GenericDAL<Role>, IRoleDAL
    {
        public RoleDALEF(string connStr, IMapper mapper) : base(connStr, mapper)
        {

        }

        public override Role Create(Role role)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = _mapper.Map<DALEF.Models.Role>(role);
                    ctx.Roles.Add(entity);
                    ctx.SaveChanges();
                    role.Id = entity.RoleId;
                    return role;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating Role: {ex.Message}");
                    return null;
                }
            }
        }

        public override bool Delete(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try {
                    var entity = ctx.Roles.Find(id);
                    if (entity == null) return false;

                    ctx.Roles.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting Role: {ex.Message}");
                    return false;
                }

            }
        }

        public override List<Role> GetAll()
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entities = ctx.Roles.OrderBy(r => r.RoleId).ToList();
                    return _mapper.Map<List<Role>>(entities);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving all Roles: {ex.Message}");
                    return new List<Role>();
                }

            }
        }

        public override Role GetById(int id)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    var entity = ctx.Roles.Find(id);
                    return _mapper.Map<Role>(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving Role by Id: {ex.Message}");
                    return null;
                }

            }
        }

        public override Role Update(Role entity)
        {
            using (var ctx = new TradingCompContext(_connStr))
            {
                try
                {
                    if(entity.Id <= 0) throw new Exception("Invalid id");

                    var existingEntity = ctx.Roles.Find(entity.Id);
                    if (existingEntity == null) throw new Exception("Non existing id");


                    existingEntity.RoleName = string.IsNullOrEmpty(entity.RoleName) 
                        ? existingEntity.RoleName : entity.RoleName;


                    //_mapper.Map(entity, existingEntity);
                    ctx.SaveChanges();
                    return _mapper.Map<Role>(existingEntity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating Role: {ex.Message}");
                    return null;
                }

            }
        }

    }
}
