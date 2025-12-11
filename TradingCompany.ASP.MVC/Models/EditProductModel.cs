using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TradingCompany.ASP.MVC.Common.Validation;

namespace TradingCompany.ASP.MVC.Models
{
    public class EditProductModel
    {
        public EditProductModel()
        {
            ProductName = string.Empty;

            Users = new List<SelectListItem>();

        }

        public List<SelectListItem> Users { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "User is required!")]
        [DisplayName("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Product Name is required!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title length should be between 5 and 100!")]
        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Category is required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category length should be between 3 and 50!")]
        public string Category { get; set; }

        
    }
}
