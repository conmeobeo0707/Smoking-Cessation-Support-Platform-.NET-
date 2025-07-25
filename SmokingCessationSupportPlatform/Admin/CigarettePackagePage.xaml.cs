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
    /// Interaction logic for CigarettePackagePage.xaml
    /// </summary>
    public partial class CigarettePackagePage : Page
    {
        public CigarettePackagePage()
        {
            InitializeComponent();

            
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dgCigarette.ItemsSource = await LoadCigarette();
        }

        public async Task<List<CigarettePackage>> LoadCigarette()
        {
            var client = ApiClient.Client;
            {
                // API: enpoint
                string apiUrl = "http://localhost:8080/api/cigarette-packages";



                var response = await client.GetAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return new List<CigarettePackage>();
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                var cigarettes = JsonSerializer.Deserialize<List<CigarettePackage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc tên thuộc tính
                });

                return cigarettes ?? new List<CigarettePackage>();


            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (!int.TryParse(txtSearch.Text.Trim(), out int cigaretteId))
            {
                MessageBox.Show("Badge ID is number. Please enter again!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/cigarette-packages/{cigaretteId}";

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
                    MessageBox.Show("Not found Cigarette with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var cigarette = JsonSerializer.Deserialize<CigarettePackage>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính


                dgCigarette.ItemsSource = new List<CigarettePackage> { cigarette };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();                // Xóa nội dung ô tìm kiếm
            dgCigarette.ItemsSource =  await LoadCigarette();
        }
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtCigaretteName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtBrand.Text) ||
                string.IsNullOrWhiteSpace(txtStrength.Text) ||
                string.IsNullOrWhiteSpace(txtFlavor.Text) ||
                string.IsNullOrWhiteSpace(txtSticks.Text) ||
                string.IsNullOrWhiteSpace(txtNicotineMg.Text)
                )
            {
                MessageBox.Show("All fields are required!!!");
                return false;
            }
            if (!double.TryParse(txtPrice.Text, out double price))
            {
                MessageBox.Show("Price must be number!!");
                return false;
            }
            if (!double.TryParse(txtNicotineMg.Text, out double nicotine))
            {
                MessageBox.Show("Nicotine mg must be number!!");
                return false;
            }
            if (!int.TryParse(txtSticks.Text, out int stick))
            {
                MessageBox.Show("SticksPerPack must be number!!");
                return false;
            }
            return true;
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if(! Validate())
            {
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/cigarette-packages";

            var data = new
            {
                cigaretteName = txtCigaretteName.Text,
                price = txtPrice.Text,
                brand = txtBrand.Text,
                nicoteneStrength = txtStrength.Text,
                flavor = txtFlavor.Text,
                sticksPerPack = txtSticks.Text,
                nicotineMg = txtNicotineMg.Text,
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


                var cigarette = JsonSerializer.Deserialize<CigarettePackage>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính

                MessageBox.Show("Create Successfully!!!");
                dgCigarette.ItemsSource = new List<CigarettePackage> { cigarette };
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

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (dgCigarette.SelectedItem is not CigarettePackage cigarette)
            {
                MessageBox.Show("Please select a Cigarette Package in the list.");
                return;
            }

            int cigaretteId = cigarette.CigaretteId ;

            if(!Validate())
            {
                return;
            }

            string name = txtCigaretteName.Text.Trim() ;
            string priceInput = txtPrice.Text.Trim() ;
            string brandInput = txtBrand.Text.Trim() ;
            string nicoteneStrengthInput = txtStrength.Text.Trim() ;
            string flavorInput = txtFlavor.Text.Trim() ;
            string sticksPerPackInput = txtSticks.Text.Trim() ;
            string nicotineMgInpput = txtNicotineMg.Text.Trim() ;
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/cigarette-packages/{cigaretteId}";

            var data = new
            {
                cigaretteName = name,
                price = priceInput,
                brand = brandInput,
                nicoteneStrength = nicoteneStrengthInput,
                flavor = flavorInput,
                sticksPerPack = sticksPerPackInput,
                nicotineMg = nicotineMgInpput,
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
                    MessageBox.Show("Cigarette not found.");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve


                var updateCigarrete = JsonSerializer.Deserialize<CigarettePackage>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });// Không phân biệt hoa thường khi đọc tên thuộc tính

                MessageBox.Show("Update Successfully!!!");
                dgCigarette.ItemsSource = new List<CigarettePackage> { updateCigarrete };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (dgCigarette.SelectedItem is not CigarettePackage cigarette)
            {
                MessageBox.Show("Please select a Cigarette Package in the list.");
                return;
            }

            var confirmResult = MessageBox.Show(
        $"Are you sure you want to delete '{cigarette.CigaretteName}'?",
        "Confirm Delete",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning
    );

            if (confirmResult != MessageBoxResult.Yes)
            {
                return;

            }

            int cigaretteId = cigarette.CigaretteId;

            string apiUrl = $"http://localhost:8080/api/cigarette-packages/{cigaretteId}";
            
            try
            {
                var response = await client.DeleteAsync(apiUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("You have not logged in or invalid token(401 Unauthorized) ");
                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Cigarette not found.");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();


                MessageBox.Show("Delete Successfully!!!");

                dgCigarette.ItemsSource =  await LoadCigarette();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }
        }

        
    }
}
