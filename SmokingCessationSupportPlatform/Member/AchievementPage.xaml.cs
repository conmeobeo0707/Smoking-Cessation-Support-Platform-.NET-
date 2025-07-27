using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DAL.Models;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Member
{
    public partial class AchievementPage : Page
    {
        private List<Badge> allBadges = new();

        public AchievementPage()
        {
            InitializeComponent();
            LoadBadges();
        }

        public async void LoadBadges()
        {
            var client = ApiClient.Client;
            string apiUrl = "http://localhost:8080/api/achievement-badge";

            try
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                allBadges = JsonSerializer.Deserialize<List<Badge>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Badge>();

                UpdateBadgePanel(allBadges);

                // Thêm mock nếu ít quá
                if (allBadges.Count < 3)
                {
                    BadgePanel.Children.Add(CreateMockBadge("🔥 7 ngày không thuốc", "Bạn đã vượt mốc 7 ngày!", "Motivation"));
                    BadgePanel.Children.Add(CreateMockBadge("💪 14 ngày kiên trì", "Tuyệt vời!", "Health"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải huy hiệu: " + ex.Message);
            }
        }

        private void UpdateBadgePanel(List<Badge> badges)
        {
            BadgePanel.Children.Clear();
            foreach (var badge in badges)
            {
                BadgePanel.Children.Add(CreateBadgeCard(badge));
            }
        }

        private Border CreateBadgeCard(Badge badge)
        {
            return new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Width = 200,
                Height = 150,
                Background = Brushes.White,
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = $"🏅 {badge.BadgeName}", FontWeight = FontWeights.Bold, FontSize = 16, Margin = new Thickness(0,0,0,5) },
                        new TextBlock { Text = badge.Description, FontSize = 12, TextWrapping = TextWrapping.Wrap },
                        new TextBlock { Text = $"🎯 {badge.Criteria}", FontStyle = FontStyles.Italic, FontSize = 11, Foreground = Brushes.DarkSlateGray }
                    }
                }
            };
        }

        private Border CreateMockBadge(string name, string desc, string type)
        {
            return new Border
            {
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Width = 200,
                Height = 150,
                Background = Brushes.Beige,
                Child = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = name, FontWeight = FontWeights.Bold, FontSize = 16, Margin = new Thickness(0,0,0,5) },
                        new TextBlock { Text = desc, FontSize = 12, TextWrapping = TextWrapping.Wrap },
                        new TextBlock { Text = $"🔖 {type}", FontSize = 11, FontStyle = FontStyles.Italic, Foreground = Brushes.DarkGray }
                    }
                }
            };
        }

        private void TypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allBadges == null || allBadges.Count == 0) return;

            string selected = (TypeFilter.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tất cả";
            var filtered = selected == "Tất cả"
                ? allBadges
                : allBadges.Where(b => b.BadgeType?.ToUpper() == selected.ToUpper()).ToList();

            UpdateBadgePanel(filtered);

            if (filtered.Count == 0)
            {
                BadgePanel.Children.Add(CreateMockBadge("❔ Không tìm thấy huy hiệu", "Không có huy hiệu thuộc loại này.", selected));
            }
        }
    }
}