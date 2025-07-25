using System.Windows;
using DAL.Models;
using SmokingCessationSupportPlatform.Helpers;

namespace SmokingCessationSupportPlatform
{
    public partial class CoachWindow : Window
    {
        public UserModel Account { get; set; }

        public CoachWindow(UserModel account)
        {
            InitializeComponent();
            Account = account;

            // Lưu vào context nếu cần
            AuthContext.UserId = account.UserId;
            AuthContext.Role = account.Role;
            AuthContext.FullName = account.FullName;
            AuthContext.Email = account.Email;

            this.Title = $"Coach - {account.FullName}";
        }
    }
}
