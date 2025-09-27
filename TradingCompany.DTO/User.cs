using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.Dto
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string RestoreKeyword { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
