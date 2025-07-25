// QuitPlanPage.xaml.cs
using BLL.Service;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class QuitPlanPage : Page
    {
        private readonly UserModel _account;
        private QuitPlan selectedPlan;

        public QuitPlanPage(UserModel account)
        {
            InitializeComponent();
            _account = account;
        }

        private async Task<List<QuitPlan>> LoadPlansAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Failed to load plans: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<QuitPlan>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async void BtnLoadMyPlans_Click(object sender, RoutedEventArgs e)
        {
            var plans = await LoadPlansAsync($"http://localhost:8080/api/quit-plan/user/{_account.UserId}");
            dgQuitPlans.ItemsSource = plans;
        }

        private async void BtnLoadCurrentPlan_Click(object sender, RoutedEventArgs e)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:8080/api/quit-plan/user/{_account.UserId}/current");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to load current plan.");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var plan = JsonSerializer.Deserialize<QuitPlan>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            dgQuitPlans.ItemsSource = new List<QuitPlan> { plan };
        }

        private async void BtnLoadFreePlans_Click(object sender, RoutedEventArgs e)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:8080/api/quit-plan/free");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to load free plans.");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var plans = JsonSerializer.Deserialize<List<QuitPlan>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            dgQuitPlans.ItemsSource = plans;
        }

        private async void BtnAddPlan_Click(object sender, RoutedEventArgs e)
        {
            var newPlan = new
            {
                title = txtTitle.Text,
                startDate = dpStartDate.SelectedDate?.ToString("yyyy-MM-dd"),
                expectedEndDate = dpEndDate.SelectedDate?.ToString("yyyy-MM-dd"),
                status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                reason = txtReason.Text,
                stagesDescription = txtStagesDescription.Text,
                customNotes = txtCustomNotes.Text,
                userId = _account.UserId,
                coachId = long.TryParse(txtCoachId.Text, out long coachId) ? coachId : (long?)null,
                recommendedPackageId = long.TryParse(txtPackageId.Text, out long packageId) ? packageId : (long?)null
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(newPlan), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:8080/api/quit-plan")
            {
                Content = jsonContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Plan added successfully!");
                BtnLoadMyPlans_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Failed to add plan: {response.StatusCode}");
            }
        }

        private void dgQuitPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPlan = dgQuitPlans.SelectedItem as QuitPlan;
            if (selectedPlan != null)
            {
                txtTitle.Text = selectedPlan.PlanName;
                dpStartDate.SelectedDate = selectedPlan.StartDate;
                dpEndDate.SelectedDate = selectedPlan.EndDate;
                cbStatus.SelectedItem = cbStatus.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == selectedPlan.Status);
                txtReason.Text = selectedPlan.StagesDescription;
                _ = LoadRatings(selectedPlan.Id);
            }
        }

        private async void BtnSubmitRating_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlan == null)
            {
                MessageBox.Show("Please select a plan to rate.");
                return;
            }

            if (string.IsNullOrEmpty(txtRatingValue.Text) || !int.TryParse(txtRatingValue.Text, out int ratingValue))
            {
                MessageBox.Show("Please enter a valid rating value (1-5).");
                return;
            }

            var ratingRequest = new RatingRequest
            {
                RatingValue = ratingValue,
                FeedbackText = txtFeedback.Text,
                MemberId = _account.UserId,
                PlanId = selectedPlan.Id
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(ratingRequest), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/api/rating")
            {
                Content = jsonContent
            };

            // Thêm Authorization Header với Bearer Token
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            try
            {
                var response = await ApiClient.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Rating submitted successfully!");
                    await LoadRatings(selectedPlan.Id);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to submit rating. Error: {error}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }


        private async Task LoadRatings(int planId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:8080/api/rating/plan/{planId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var ratings = JsonSerializer.Deserialize<List<Rating>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                lvRatings.ItemsSource = ratings;
            }
        }
        private async void BtnUpdatePlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlan == null) { MessageBox.Show("Please select a plan to update."); return; }

            selectedPlan.PlanName = txtTitle.Text;
            selectedPlan.StartDate = dpStartDate.SelectedDate;
            selectedPlan.EndDate = dpEndDate.SelectedDate;
            selectedPlan.Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            selectedPlan.StagesDescription = txtReason.Text;

            var jsonContent = new StringContent(JsonSerializer.Serialize(selectedPlan), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:8080/api/quit-plan/{selectedPlan.Id}/user")
            {
                Content = jsonContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Plan updated successfully!");
                BtnLoadMyPlans_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Failed to update plan: {response.StatusCode}");
            }
        }

        private async void BtnCancelPlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlan == null) { MessageBox.Show("Please select a plan to cancel."); return; }

            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:8080/api/quit-plan/{selectedPlan.Id}/cancel");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Plan cancelled.");
                BtnLoadMyPlans_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Failed to cancel plan: {response.StatusCode}");
            }
        }

        private async void BtnCompletePlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlan == null) { MessageBox.Show("Please select a plan to complete."); return; }

            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:8080/api/quit-plan/{selectedPlan.Id}/complete");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Plan completed.");
                BtnLoadMyPlans_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Failed to complete plan: {response.StatusCode}");
            }
        }

        private async void BtnDeletePlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlan == null) { MessageBox.Show("Please select a plan to delete."); return; }

            var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:8080/api/quit-plan/{selectedPlan.Id}/user/{_account.UserId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Plan deleted.");
                BtnLoadMyPlans_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Failed to delete plan: {response.StatusCode}");
            }
        }

    }
}
