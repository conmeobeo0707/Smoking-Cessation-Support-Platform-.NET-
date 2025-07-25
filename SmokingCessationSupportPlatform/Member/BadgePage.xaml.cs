using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DAL.Models;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class BadgePage : Page
    {
        private readonly UserModel _account;
        private UserBadge selectedBadge;

        public BadgePage(UserModel account)
        {
            InitializeComponent();
            _account = account;
            LoadBadges();
        }

        private async void LoadBadges()
        {
            string url = $"http://localhost:8080/api/user_badge/user/{_account.UserId}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var badges = JsonSerializer.Deserialize<List<UserBadge>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                lvBadges.ItemsSource = badges;
            }
            else
            {
                MessageBox.Show("Failed to load badges.");
            }
        }

        private async void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBadge == null)
            {
                MessageBox.Show("Please select a badge to view details.");
                return;
            }

            string url = $"http://localhost:8080/api/user_badge/{selectedBadge.BadgeId}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var badgeDetail = JsonSerializer.Deserialize<UserBadge>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                MessageBox.Show($"Badge Name: {badgeDetail.BadgeName}\nDescription: {badgeDetail.Description}\nAchieved: {badgeDetail.AchievedDate}");
            }
            else
            {
                MessageBox.Show("Failed to get badge details.");
            }
        }

        private async void BtnShareBadge_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBadge == null)
            {
                MessageBox.Show("Please select a badge to share.");
                return;
            }

            string url = $"http://localhost:8080/api/user_badge/{selectedBadge.BadgeId}/share";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _account.Token);

            var response = await ApiClient.Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Badge shared successfully!");
                LoadBadges();
            }
            else
            {
                MessageBox.Show("Failed to share badge.");
            }
        }

        private void lvBadges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBadge = lvBadges.SelectedItem as UserBadge;
        }
    }
}
