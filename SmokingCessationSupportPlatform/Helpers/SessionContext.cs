using DAL.Models;
using BLL.Service;

namespace SmokingCessationSupportPlatform.Helpers
{
    public static class SessionContext
    {
        public static UserModel? CurrentUser { get; private set; }

        public static void SetUser(UserModel user)
        {
            CurrentUser = user;
            ApiClient.setToken(user.Token);
        }

        public static void Logout()
        {
            CurrentUser = null;
            ApiClient.ClearToken();
        }

        public static string? GetToken()
        {
            return CurrentUser?.Token;
        }

        public static bool IsLoggedIn => CurrentUser != null;
    }
}
