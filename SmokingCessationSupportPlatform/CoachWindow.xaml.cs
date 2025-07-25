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
using System.Windows.Shapes;
using BLL.Service;
using DAL.Models;

namespace SmokingCessationSupportPlatform
{
    /// <summary>
    /// Interaction logic for CoachWindow.xaml
    /// </summary>
    public partial class CoachWindow : Window
    {
        public UserModel Account { get; set; }
        public CoachWindow()
        {
            InitializeComponent();
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = ApiClient.Client;

            if (!int.TryParse(txtUserId.Text.Trim(), out int userId))
            {
                MessageBox.Show("User ID is number. Please enter again!!!");
                return;
            }
            // API: enpoint
            string apiUrl = $"http://localhost:8080/api/quit-plan/user/{userId}";

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
                    MessageBox.Show("Not found plan with this ID");
                    return;
                }
                // kiem tra ket qua
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();// doc noi dung tra  ve

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var plans = new List<QuitPlanBasicModel>();

                foreach (var element in root.EnumerateArray())
                {
                    plans.Add(new QuitPlanBasicModel
                    {
                        PlanId = element.GetProperty("planId").GetInt32(),
                        Title = element.GetProperty("title").GetString(),
                        StartDate = element.GetProperty("startDate").GetDateTime(),
                        ExpectedEndDate = element.GetProperty("expectedEndDate").GetDateTime(),
                        Status = element.GetProperty("status").GetString(),
                        UserId = element.GetProperty("userId").GetInt32(),
                        CoachId = element.TryGetProperty("coachId", out var coach) && coach.ValueKind != JsonValueKind.Null
                                    ? coach.GetInt32()
                                    : null
                    });
                }

                dgQuitPlans.ItemsSource = plans;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when call API: " + ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dgQuitPlans.ItemsSource = null;
            txtUserId.Clear();
        }
    }
}
