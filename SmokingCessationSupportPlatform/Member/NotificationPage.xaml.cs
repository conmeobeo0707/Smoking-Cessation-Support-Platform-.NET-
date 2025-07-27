using BLL.Service;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class NotificationPage : Page
    {
        public NotificationPage()
        {
            InitializeComponent();
            LoadNotifications();
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

                NotificationList.ItemsSource = list ?? new List<Notification>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông báo: " + ex.Message);
            }
        }
    }
}