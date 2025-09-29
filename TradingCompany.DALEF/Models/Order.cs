using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.DALEF.Models;

[Index("OrderedAt", Name = "IX_Orders_OrderedAt")]
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

    [StringLength(12)]
    public string? Status { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("BuyerId")]
    [InverseProperty("Orders")]
    public virtual User Buyer { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Orders")]
    public virtual Product Product { get; set; } = null!;
}
