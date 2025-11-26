using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DTO;

namespace TradingCompany.BLL.Interfaces
{
    public interface IProductManager
    {
        List<Product> GetProducts();
        Product? GetProductById(int productId);
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        bool DeleteProduct(int productId);

    }
}
