using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RestoreKeyword { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public DateTime RegistrationDate { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Username} - {Email} - {RegistrationDate.ToShortDateString()}";
        }
    }
}
