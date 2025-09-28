using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.DALEF.Models;

[Index("OrderedAt", Name = "IX_Orders_OrderedAt")]
[Index("ProductId", Name = "UQ__Orders__B40CC6ECBBBAEC3B", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column("BuyerID")]
    public int BuyerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderedAt { get; set; }

    [ForeignKey("BuyerId")]
    [InverseProperty("Orders")]
    public virtual User Buyer { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Order")]
    public virtual Product Product { get; set; } = null!;
}
