using DAL.Models;
using SmokingCessationSupportPlatform.Admin;
using System.Windows;
using System.Windows.Interop;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class MemberWindow : Window
    {
        public UserModel Account { get; set; }

        public MemberWindow(UserModel account)
        {
            InitializeComponent();
            Account = account;
        }

        private void Btn_QuitPlan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new QuitPlanPage(Account));
        }

        private void Btn_Badge_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BadgePage(Account));
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage(Account));
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PostManagementPage());
        }

        private void Achievement_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AchievementPage());
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NotificationPage());
        }

        private void SmokingStatus_Click(object sender, RoutedEventArgs e)
        {
            
            MainFrame.Navigate(new SmokingStatusPage(Account));
        }



        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}