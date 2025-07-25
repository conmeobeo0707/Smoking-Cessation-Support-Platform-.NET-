<<<<<<< HEAD
﻿using System.Windows;
using DAL.Models;
using SmokingCessationSupportPlatform.Helpers;
=======
﻿using System;
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
>>>>>>> ab0fb5ae47ced4f733cdeaed9ed8ac429ac048c0

namespace SmokingCessationSupportPlatform
{
    public partial class AdminWindow : Window
    {
        public UserModel Account { get; set; }

        public AdminWindow(UserModel account)
        {
            InitializeComponent();
<<<<<<< HEAD
            Account = account;

            // Gán dữ liệu vào context nếu muốn dùng sau
            AuthContext.UserId = account.UserId;
            AuthContext.Role = account.Role;
            AuthContext.FullName = account.FullName;
            AuthContext.Email = account.Email;

            this.Title = $"Admin - {account.FullName}";
=======
            
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
>>>>>>> ab0fb5ae47ced4f733cdeaed9ed8ac429ac048c0
        }
    }
}
