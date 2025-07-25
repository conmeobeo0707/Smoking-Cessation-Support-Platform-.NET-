using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using DAL.Models;
using BLL.Service;
using SmokingCessationSupportPlatform.Helpers; // Giả sử bạn có AuthContext để lấy user ID

namespace SmokingCessationSupportPlatform.Member
{
    public partial class SmokingStatusPage : Page
    {
        public SmokingStatusPage()
        {
            InitializeComponent();
            dpDate.SelectedDate = DateTime.Now;
            LoadHistory();
        }

        public class SmokingStatusItem
        {
            public string Status { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string DateDisplay => Date.ToString("yyyy-MM-dd");
        }

        public async void LoadHistory()
        {
            try
            {
                var client = ApiClient.Client;
                var userId = AuthContext.CurrentUserId; // giả sử bạn đã có user đăng nhập
                var response = await client.GetAsync("http://localhost:8080/api/smoking-status");

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                var items = JsonSerializer.Deserialize<List<SmokingStatusItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                HistoryListBox.ItemsSource = items.OrderByDescending(x => x.Date).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải lịch sử: " + ex.Message);
            }
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            if (cbStatus.SelectedItem is ComboBoxItem selected && dpDate.SelectedDate.HasValue)
            {
                var status = selected.Content.ToString();
                var date = dpDate.SelectedDate.Value;

                var data = new { Status = status, Date = date };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = ApiClient.Client;
                var response = await client.PostAsync("http://localhost:8080/api/smoking-status", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tạo thành công");
                    LoadHistory();
                }
                else
                {
                    MessageBox.Show("Lỗi khi tạo: " + await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            if (cbStatus.SelectedItem is ComboBoxItem selected && dpDate.SelectedDate.HasValue)
            {
                var status = selected.Content.ToString();
                var date = dpDate.SelectedDate.Value;

                var data = new { Status = status, Date = date };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = ApiClient.Client;
                var response = await client.PutAsync("http://localhost:8080/api/smoking-status", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật thành công");
                    LoadHistory();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + await response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}
