using System.Configuration;
using System.Data;
using System.Windows;

namespace SmokingCessationSupportPlatform
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Chỉ mở login window
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Gán MainWindow để đảm bảo chỉ có 1 cửa sổ chính
            MainWindow = loginWindow;
        }
    }

}
