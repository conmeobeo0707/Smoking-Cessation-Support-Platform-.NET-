using System.Windows;
using DAL.Models;
using SmokingCessationSupportPlatform.Helpers;

namespace SmokingCessationSupportPlatform
{
    public partial class AdminWindow : Window
    {
        public UserModel Account { get; set; }

        public AdminWindow(UserModel account)
        {
            InitializeComponent();
            Account = account;

            // Gán dữ liệu vào context nếu muốn dùng sau
            AuthContext.UserId = account.UserId;
            AuthContext.Role = account.Role;
            AuthContext.FullName = account.FullName;
            AuthContext.Email = account.Email;

            this.Title = $"Admin - {account.FullName}";
        }
    }
}
