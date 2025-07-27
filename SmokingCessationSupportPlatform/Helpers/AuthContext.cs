using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokingCessationSupportPlatform.Helpers
{
    public static class AuthContext
    {
        public static int UserId { get; set; }
        public static int CurrentUserId => UserId; // thêm nếu project đang gọi chỗ khác
        public static string? Role { get; set; }
        public static string? FullName { get; set; }
        public static string? Email { get; set; }
    }
}