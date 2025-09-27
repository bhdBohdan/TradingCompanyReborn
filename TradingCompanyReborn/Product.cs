using System;
using System.Collections.Generic;

namespace TradingCompany.Console;

public partial class Product
{
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
