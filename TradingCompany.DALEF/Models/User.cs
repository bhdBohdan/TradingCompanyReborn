using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingCompany.DALEF.Models;

[Index("CreatedAt", Name = "IX_Users_CreatedAt")]
[Index("Username", Name = "UQ__Users__536C85E4A7D0B23D", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D10534568A099B", IsUnique = true)]
[Index("Email", Name = "UX_Users_Email", IsUnique = true)]
[Index("Username", Name = "UX_Users_Username", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(50)]
    public string RestoreKeyword { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; } 

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; } 
    [InverseProperty("Buyer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("User")]
    public virtual UserProfile? UserProfile { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
