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
    /// Interaction logic for MemberPackagePage.xaml
    /// </summary>
    public partial class MemberPackagePage : Page
    {
        public MemberPackagePage()
        {
            InitializeComponent();
            LoadMemberPackage();
        }
        public async void LoadMemberPackage()
        {
            var client = ApiClient.Client;
            {
                // API: enpoint
                string apiUrl = "http://localhost:8080/api/member-packages";



                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                var memberPackages = JsonSerializer.Deserialize<List<MemberPackage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc tên thuộc tính
                });

                dgMemberPackage.ItemsSource = memberPackages;


            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (!int.TryParse(txtSearch.Text.Trim(), out int memberPackageId))
            {
                MessageBox.Show("Member Package ID is number. Please enter again!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/member-packages/{memberPackageId}";

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
                    MessageBox.Show("Not found member package with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var memberPackage = JsonSerializer.Deserialize<MemberPackage>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính


                dgMemberPackage.ItemsSource = new List<MemberPackage> { memberPackage };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();                // Xóa nội dung ô tìm kiếm
            LoadMemberPackage();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (string.IsNullOrWhiteSpace(txtPackageName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtDuration.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text)
                )
            {
                MessageBox.Show("All fields are required!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/member-packages";

            var data = new
            {
                packageName = txtPackageName.Text,
                price = txtPrice.Text,
                duration = txtDuration.Text,
                featuresDescription = txtDescription.Text,
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


                var memberPackage = JsonSerializer.Deserialize<MemberPackage>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính

                MessageBox.Show("Create Successfully!!!");
                dgMemberPackage.ItemsSource = new List<MemberPackage> { memberPackage };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            spInputForm.Visibility = Visibility.Visible;
        }
    }
}
