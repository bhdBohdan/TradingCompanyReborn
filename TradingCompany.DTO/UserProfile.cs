using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompany.DTO
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? BankCardNumber { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id}: {UserId} {FirstName}  {LastName} \n" +
                $"{Gender} - {Address}\n" +
                $" {Phone} - {BankCardNumber} \n" +
                $" {UpdatedAt?.ToShortDateString()}\n\n";
        }
    }
}
