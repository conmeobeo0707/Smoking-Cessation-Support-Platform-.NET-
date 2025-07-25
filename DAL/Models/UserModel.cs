using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserPublicId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
    }
}
