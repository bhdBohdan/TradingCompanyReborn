using System;
using System.Collections.Generic;

namespace TradingCompany.Console;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string RestoreKeyword { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual UserProfile? UserProfile { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
