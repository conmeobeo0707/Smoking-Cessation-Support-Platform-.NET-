using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserPublicId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? Status { get; set; }
        public string? RoleName { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? AuthProvider { get; set; }
    }
}
