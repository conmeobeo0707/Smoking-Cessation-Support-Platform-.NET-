using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class QuitPlanPage : Page
    {
        private int userId = 4; // Sửa lại nếu cần lấy từ context đăng nhập

        public QuitPlanPage()
        {
            InitializeComponent();
        }

        private async void btnLoadPlans_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string apiUrl = $"http://localhost:8080/api/quit-plan/user/{userId}";
                var response = await ApiClient.Client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Failed to load plans: {response.StatusCode}");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var plans = JsonSerializer.Deserialize<List<QuitPlan>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                dgQuitPlans.ItemsSource = plans;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnLoadMyPlans_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}