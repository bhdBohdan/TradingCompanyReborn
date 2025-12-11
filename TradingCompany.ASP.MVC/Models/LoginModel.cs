
using System.ComponentModel.DataAnnotations;

namespace TradingCompany.ASP.MVC.Models
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
    /// https://medium.com/@madu.sharadika/validation-in-net-8-a250c4d278d2
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter a valid username.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Username must be between 4 and 40 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid password.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}