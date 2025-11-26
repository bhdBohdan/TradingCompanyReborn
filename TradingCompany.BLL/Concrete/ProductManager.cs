using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Concrete
{
    public class ProductManager: IProductManager
    {
        private readonly IProductDAL _productDAL;


        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public List<Product> GetProducts()
        {
            return _productDAL.GetAll();
        }
        public Product AddProduct(Product product)
        {
           return _productDAL.Create(product);
        }
        public bool DeleteProduct(int productId)
        {
            return _productDAL.Delete(productId);
        }
        public Product? GetProductById(int productId)
        {
            return _productDAL.GetById(productId);
        }

        public Product UpdateProduct(Product product)
        {
            return _productDAL.Update(product);
        }
    }
}
