using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using DAL.Models;
using SmokingCessationSupportPlatform.Helpers;

namespace SmokingCessationSupportPlatform
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email hoặc mật khẩu không được để trống.");
                return;
            }

            using var client = new HttpClient();
            string apiUrl = "http://localhost:8080/api/auth/login";

            var loginData = new
            {
                login = email,
                password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng nhập thất bại! Vui lòng kiểm tra lại thông tin.");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var loginResult = JsonSerializer.Deserialize<UserModel>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResult == null)
                {
                    MessageBox.Show("Lỗi: Không thể phân tích dữ liệu phản hồi.");
                    return;
                }

                // Lưu user & token
                SessionContext.SetUser(loginResult);

                // Mở cửa sổ theo role
                switch (loginResult.Role?.ToUpper())
                {
                    case "USER":
                        new MemberWindow(loginResult).Show();
                        break;
                    case "ADMIN":
                        new AdminWindow(loginResult).Show();
                        break;
                    case "COACH":
                        new CoachWindow(loginResult).Show();
                        break;
                    default:
                        MessageBox.Show("Vai trò không hợp lệ.");
                        return;
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gọi API: " + ex.Message);
            }
        }
    }
}
