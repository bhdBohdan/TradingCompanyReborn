using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int BuyerId { get; set; }

        public string? Status { get; set; }

        public int Quantity { get; set; }

        public virtual User? Buyer { get; set; }

        public virtual Product? Product { get; set; }

        public DateTime? OrderedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: Product {(Product?.ProductName ?? ProductId.ToString())} " +
                $"ordered by User {(Buyer?.Username ?? BuyerId.ToString())} at {OrderedAt?.ToShortDateString() ?? "N/A"} \n" +
                 $"\t Status: {Status}, Quantity bought {Quantity}";

        }
    }
}
