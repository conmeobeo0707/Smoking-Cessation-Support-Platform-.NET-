using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public static class ApiClient
    {
        public static HttpClient Client { get; private set; } = new HttpClient();

        public static void setToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void ClearToken()
        {
            Client.DefaultRequestHeaders.Authorization=null;
        }

    }
}
