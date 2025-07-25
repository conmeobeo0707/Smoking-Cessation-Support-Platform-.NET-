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
    public partial class PostDetailPage : Window
    {
        private readonly int? postId;
        private readonly Action? onSuccessCallback;

        public string WindowTitle => postId.HasValue ? "📝 Chi tiết bài viết" : "➕ Tạo bài viết mới";

        public PostDetailPage(int? id = null, Action? onSuccess = null)
        {
            InitializeComponent();
            DataContext = this;
            postId = id;
            onSuccessCallback = onSuccess;

            if (postId.HasValue)
            {
                LoadPost(postId.Value);
            }
            else
            {
                txtCreatedAt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                txtAuthor.Text = SessionContext.CurrentUser?.FullName ?? "Không xác định";
            }
        }

        private async void LoadPost(int id)
        {
            try
            {
                var response = await ApiClient.Client.GetAsync($"http://localhost:8080/api/posts/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Không tìm thấy bài viết.");
                    Close();
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<PostModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                txtTitle.Text = post.Title;
                txtContent.Text = post.Content;
                txtAuthor.Text = post.UserName;
                txtCreatedAt.Text = post.PostDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bài viết: " + ex.Message);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var title = txtTitle.Text.Trim();
            var content = txtContent.Text.Trim();
            var author = txtAuthor.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tiêu đề và nội dung.");
                return;
            }

            var postData = new
            {
                title,
                content
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(postData),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                HttpResponseMessage response;
                if (postId.HasValue)
                {
                    // PUT: cập nhật
                    response = await ApiClient.Client.PutAsync($"http://localhost:8080/api/posts/{postId.Value}", jsonContent);
                }
                else
                {
                    // POST: tạo mới
                    response = await ApiClient.Client.PostAsync("http://localhost:8080/api/posts", jsonContent);
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Lưu thành công!");
                    onSuccessCallback?.Invoke(); // 👉 Gọi reload
                    Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lưu thất bại: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
