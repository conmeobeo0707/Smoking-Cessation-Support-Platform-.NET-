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
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Page
    {
        public UserManagement()
        {
            InitializeComponent();
            string hardcodeToken = "eyJhbGciOiJIUzM4NCJ9.eyJzdWIiOiI0IiwiaWF0IjoxNzUzMjY0NzUwLCJleHAiOjE3NTMzNTExNTAsInJvbGVzIjpbIlJPTEVfQURNSU4iXX0.xdD8mFawTR8raCus_LFJNN0-HP0JS03QFrP0-iRrp-O8VGMj4Hsh5BsTUUEGVpks";
            ApiClient.setToken(hardcodeToken);
            LoadUsers();
        }

        public async void LoadUsers()
        {
            var client = ApiClient.Client;
            {
                // API: enpoint
                string apiUrl = "http://localhost:8080/api/admin/users";



                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc tên thuộc tính
                });
                
                dgUser.ItemsSource = users;


            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;
            
            if(!int.TryParse(txtSearch.Text.Trim(), out int userId))
            {
                MessageBox.Show("User ID is number. Please enter again!!!");
                return;
            }
                // API: enpoint
                string apiUrl = $"http://localhost:8080/api/admin/users/{userId}";

            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }

                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Not found user with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve
               

                var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính


                dgUser.ItemsSource = new List<User> { user};
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
                


            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();                // Xóa nội dung ô tìm kiếm
            LoadUsers();      // Load lai
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var client  = ApiClient.Client;

            if (dgUser.SelectedItem is not User user)
            {
                MessageBox.Show("Please select a user in the list.");
                return;
            }

            int userId = user.UserId;

            var validRole = new[] {1, 2, 3};
            if (!int.TryParse(txtRoleId.Text.Trim(), out int roleId) && !validRole.Contains(roleId))
            {
                MessageBox.Show("Role ID must be 1(ADMIN), 2(COACH), 3(USER)");
                return;
            }
            if(string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtRoleId.Text))
            {
                MessageBox.Show("All fields are required!!!");
                return;
            }
            string status = txtStatus.Text.ToLower();
            if (status != "active" && status != "inactive")
            {
                MessageBox.Show("Status must be either 'active' or 'inactive'.");
                return;
            }
           

            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/admin/users/{userId}/update-role-status";

            var data = new
            {
                newRoleId = roleId,
                newStatus = status.ToUpper(),
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
                );
            try
            {
                var response = await client.PutAsync(apiUrl, jsonContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("User not found.");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var updateUser = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính

                MessageBox.Show("Update Successfully!!!");
                dgUser.ItemsSource = new List<User> { updateUser };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }


        }
    }
}
