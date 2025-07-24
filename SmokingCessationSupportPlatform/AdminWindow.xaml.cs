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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL.Service;
using DAL.Models;
using SmokingCessationSupportPlatform.Admin;

namespace SmokingCessationSupportPlatform
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public UserModel Account { get; set; }
        public AdminWindow()
        {
            InitializeComponent();
            
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new UserManagement());
        }

        private void btnBadge_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new BadgeManagementPage());
        }

        private void btnMemberPackage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new MemberPackagePage());
        }

        private void btnCigarette_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new CigarettePackagePage());
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new PostManagementPage());
        }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ApiClient.ClearToken();

            LoginWindow loginWindow = new LoginWindow();  
            loginWindow.Show();

            this.Close();
        }
    }
}
