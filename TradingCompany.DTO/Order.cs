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

        public DateTime? OrderedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: Product {ProductId} ordered by User {BuyerId} at {OrderedAt?.ToShortDateString()} \n" +
                $"\t Status: {Status}, Quantity bought {Quantity}";
        }
    }
}
