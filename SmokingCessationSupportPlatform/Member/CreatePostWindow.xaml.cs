using BLL.Service;
using DAL.Models;
using SmokingCessationSupportPlatform.Helpers;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class CreatePostWindow : Window
    {
        public CreatePostWindow()
        {
            InitializeComponent();
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string content = txtContent.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề và nội dung.");
                return;
            }

            try
            {
                if (!SessionContext.IsLoggedIn)
                {
                    MessageBox.Show("Chưa đăng nhập. Vui lòng đăng nhập lại.");
                    return;
                }

                var client = ApiClient.Client;
                var postData = new
                {
                    title = title,
                    content = content
                };

                var json = JsonSerializer.Serialize(postData);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:8080/api/posts", httpContent);

                response.EnsureSuccessStatusCode();
                MessageBox.Show("✅ Bài viết đã được đăng!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi đăng bài viết: " + ex.Message);
            }
        }
    }
}
