using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using DAL.Models;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class DashboardPage : Page
    {
        private readonly UserModel user;

        public DashboardPage(UserModel userModel)
        {
            InitializeComponent();
            user = userModel;

            // Hiển thị tên và email
            txtName.Text = $"👤 Xin chào, {user.FullName}";
            txtEmail.Text = $"Email: {user.Email}";

            // Ngày không hút thuốc (dữ liệu mẫu)
            txtSmokeFreeDays.Text = "12 ngày";

            LoadBadges();
            LoadNotifications();
        }

        // Bắt buộc constructor mặc định để load XAML (ví dụ khi preview hoặc chưa có data)
        public DashboardPage()
        {
            InitializeComponent();
        }

        private async void LoadBadges()
        {
            try
            {
                var response = await ApiClient.Client.GetAsync("http://localhost:8080/api/achievement-badge");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<Badge>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                txtBadgeCount.Text = $"{list?.Count ?? 0} huy hiệu";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải huy hiệu: " + ex.Message);
                txtBadgeCount.Text = "--";
            }
        }

        private async void LoadNotifications()
        {
            try
            {
                var response = await ApiClient.Client.GetAsync("http://localhost:8080/api/notifications/me");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                RecentNotifications.ItemsSource = list ?? new List<Notification>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông báo: " + ex.Message);
            }
        }
    }
}