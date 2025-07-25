using System.Windows;
using System.Windows.Controls;
using DAL.Models;
using SmokingCessationSupportPlatform.Member;

namespace SmokingCessationSupportPlatform
{
    public partial class MemberWindow : Window
    {
        public UserModel Account { get; set; } = new();

        public MemberWindow(UserModel account)
        {
            InitializeComponent();
            Account = account;

            txtGreeting.Text = $"Xin chào, {Account.FullName}!";
            MainContent.Navigate(new DashboardPage(Account));
        }


        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new DashboardPage(Account));
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new PostManagementPage());
        }

        private void Achievement_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new AchievementPage());
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new NotificationPage());
        }

        private void SmokingStatus_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new SmokingStatusPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
