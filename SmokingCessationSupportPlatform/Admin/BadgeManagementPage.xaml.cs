using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for BadgeManagementPage.xaml
    /// </summary>
    public partial class BadgeManagementPage : Page
    {
        public BadgeManagementPage()
        {
            InitializeComponent();
            LoadBadge();
        }

        public async void LoadBadge()
        {
            var client = ApiClient.Client;
            {
                // API: enpoint
                string apiUrl = "http://localhost:8080/api/achievement-badge";



                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                var users = JsonSerializer.Deserialize<List<Badge>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc tên thuộc tính
                });

                dgBadge.ItemsSource = users;


            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (!int.TryParse(txtSearch.Text.Trim(), out int badgeId))
            {
                MessageBox.Show("Badge ID is number. Please enter again!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/achievement-badge/{badgeId}";

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
                    MessageBox.Show("Not found user with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var badge = JsonSerializer.Deserialize<Badge>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính


                dgBadge.ItemsSource = new List<Badge> { badge };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();                // Xóa nội dung ô tìm kiếm
            LoadBadge();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            spInputForm.Visibility = Visibility.Visible;
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if(string.IsNullOrWhiteSpace(txtBadgeName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtCriteria.Text) ||
                string.IsNullOrWhiteSpace(txtBadgeType.Text)
                )
            {
                MessageBox.Show("All fields are required!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/achievement-badge";

            var data = new
            {
                badgeName = txtBadgeName.Text,
                description = txtDescription.Text,
                criteria = txtCriteria.Text,
                badgeType = txtBadgeType.Text,
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
                );
            try
            {
                var response = await client.PostAsync(apiUrl, jsonContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Bad request: " + error);
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var badge = JsonSerializer.Deserialize<Badge>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính

                MessageBox.Show("Create Successfully!!!");
                dgBadge.ItemsSource = new List<Badge> { badge };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }
    }
}
