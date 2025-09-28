using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Product
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: {ProductName} - {Category} - {Price:C} - {CreatedDate?.ToShortDateString()}";
        }
    }
}
