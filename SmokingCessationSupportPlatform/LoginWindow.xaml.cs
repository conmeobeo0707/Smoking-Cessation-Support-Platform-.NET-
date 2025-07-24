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
    using System.Windows.Shapes;
using BLL.Service;
using DAL.Models;

    namespace SmokingCessationSupportPlatform
    {
        /// <summary>
        /// Interaction logic for LoginWindow.xaml
        /// </summary>
        public partial class LoginWindow : Window
        {
            public LoginWindow()
            {
                InitializeComponent();
            }

            private async void btnButton_Click(object sender, RoutedEventArgs e)
            {
            var email = txtEmail.Text;
            var password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email hoặc mật khẩu không được để trống.");
                return;
            }

            using (HttpClient client = new HttpClient())
                {
                    // API: endpoint
                    string apiUrl = "http://localhost:8080/api/auth/login";
               
                    var loginData = new
                    {
                        login = txtEmail.Text,
                        password = txtPassword.Password
                    };


                    string jsonData = JsonSerializer.Serialize(loginData);
                
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Goi api
                    var response = await client.PostAsync(apiUrl, content);

                    // KIEM TRA KET QUA
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Raw response: " + responseContent);
                    

                        var loginResult = JsonSerializer.Deserialize<UserModel>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if(loginResult != null)
                        {

                            ApiClient.setToken(loginResult.Token);
                            string role = loginResult.Role;

                             if(role == "ADMIN")
                            {
                                AdminWindow adminWindow = new AdminWindow();
                                adminWindow.Account = loginResult;
                                adminWindow.Show();
                                this.Close();
                            }
                            else if (role == "COACH")
                            {
                            CoachWindow coachWindow = new CoachWindow();
                            coachWindow.Account = loginResult;
                            coachWindow.Show();
                            this.Close();
                            }
                            else if (role == "USER")
                            {
                            MemberWindow memberWindow = new MemberWindow();
                            memberWindow.Account = loginResult;
                            memberWindow.Show();
                            this.Close();
                            }
                            else
                            {
                            MessageBox.Show("Invalid Email or Password!!!");
                            }
                        }
                    else
                    {
                        MessageBox.Show("Login failed!!! " + response.StatusCode);
                    }

                }
                    
                }
            }
        }
    }
