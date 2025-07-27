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
    public partial class PostManagement : Page
    {
        public PostManagement()
        {
            InitializeComponent();
            LoadPosts();
        }

        public async void LoadPosts()
        {
            try
            {
                var response = await ApiClient.Client.GetAsync("http://localhost:8080/api/posts");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<PostModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                dgPosts.ItemsSource = posts ?? new List<PostModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bài viết: " + ex.Message);
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSearch.Text.Trim(), out int id))
            {
                MessageBox.Show("ID không hợp lệ!");
                return;
            }

            try
            {
                var response = await ApiClient.Client.GetAsync($"http://localhost:8080/api/posts/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Không tìm thấy bài viết.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<PostModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                dgPosts.ItemsSource = post != null ? new List<PostModel> { post } : new List<PostModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            tbPlaceholder.Visibility = Visibility.Visible;
            LoadPosts();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtSearch.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((Button)sender).Tag?.ToString(), out int postId))
            {
                var detailWindow = new PostDetailPage(postId, LoadPosts); // 👈 truyền callback
                detailWindow.ShowDialog();
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(((Button)sender).Tag?.ToString(), out int postId)) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xoá bài viết này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var response = await ApiClient.Client.DeleteAsync($"http://localhost:8080/api/posts/{postId}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xoá thành công!");
                    LoadPosts();
                }
                else
                {
                    MessageBox.Show("Xoá thất bại!");
                }
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new PostDetailPage(null, LoadPosts); // 👈 truyền callback
            createWindow.ShowDialog();
        }
    }
}