using DAL.Models;
using System.Windows;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class MemberWindow : Window
    {
        public UserModel Account { get; set; }

        public MemberWindow()
        {
            InitializeComponent();
        }

        private void Btn_QuitPlan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new QuitPlanPage(Account));
        }

        private void Btn_Badge_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BadgePage(Account));
        }

        private void Btn_Progress_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProgressPage());
        }

        private void Btn_Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage());
        }

        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
