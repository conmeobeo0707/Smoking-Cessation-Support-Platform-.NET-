using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL.Service;
using DAL.Models;

namespace SmokingCessationSupportPlatform.Admin
{
    /// <summary>
    /// Interaction logic for PostManagementPage.xaml
    /// </summary>
    public partial class PostManagementPage : Page
    {
        public PostManagementPage()
        {
            InitializeComponent();
            LoadPost();
        }

        public async void LoadPost()
        {
            var client = ApiClient.Client;
            {
                // API: enpoint
                string apiUrl = "http://localhost:8080/api/posts";



                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                var posts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc tên thuộc tính
                });

                dgPost.ItemsSource = posts;


            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (!int.TryParse(txtSearch.Text.Trim(), out int userId))
            {
                MessageBox.Show("Post ID is number. Please enter again!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/posts/user/{userId}";

            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Not found any post with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var posts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính


                dgPost.ItemsSource = posts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();                // Xóa nội dung ô tìm kiếm
            LoadPost();
        }
    }
}
