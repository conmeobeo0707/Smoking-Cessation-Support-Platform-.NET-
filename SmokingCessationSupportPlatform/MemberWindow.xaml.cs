using DAL.Models;
using SmokingCessationSupportPlatform.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class MemberWindow : Window
    {
        private int _userId = 4;
        public UserModel Account { get; set; }
        public MemberWindow()
        {
            InitializeComponent();
        }

        private void Btn_QuitPlan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new QuitPlanPage());
        }

        private void Btn_Badge_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BadgePage(_userId));
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
