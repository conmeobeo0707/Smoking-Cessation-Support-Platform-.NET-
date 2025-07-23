using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using DAL.Models;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class BadgePage : Page
    {
        private int _userId;

        public BadgePage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadBadges();
        }

        private async void LoadBadges()
        {
            string url = $"http://localhost:8080/api/user_badge/user/{_userId}";
            var response = await ApiClient.Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var badges = JsonSerializer.Deserialize<List<Badge>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                lvBadges.ItemsSource = badges;
            }
        }
    }
}
