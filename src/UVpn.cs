using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace UVpnApi
{
    public class UVpn
    {
        private readonly HttpClient httpClient;
        private readonly string apiUrl = "https://api.uvpn.me";
        public string? authToken, deviceToken, proxyToken, urlToken;
        public UVpn()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
        }

        public async Task<string> Login()
        {
            var data = JsonContent.Create(new
            {
                version = "7.1.4",
                platform = "Windows",
                browser = "chrome",
                browser_lang = "ru-RU",
                type = "browser-extension",
                user_version = "UVPNv3"
            });
            var response = await httpClient.PostAsync($"{apiUrl}/v2/user/create", data);
            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);
            var documentData = document.RootElement.GetProperty("data");
            string? Get(string name) => documentData.TryGetProperty(name, out var element) ? element.GetString() : null;
            (authToken, deviceToken, proxyToken, urlToken) = (
                Get("auth_token"),
                Get("device_token"),
                Get("proxy_token"),
                Get("url_token")
            );
            if (!string.IsNullOrEmpty(authToken))
                httpClient.DefaultRequestHeaders.Add("Authorization", authToken);
            return content;
        }

        public async Task<string> GetServers()
        {
            var data = JsonContent.Create(new { device_token = deviceToken });
            var response = await httpClient.PostAsync($"{apiUrl}/v2/servers/ping", data);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetAccountInfo()
        {
            var response = await httpClient.PostAsync($"{apiUrl}/user/info", null);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetNotifications()
        {
            var response = await httpClient.PostAsync($"{apiUrl}/notifications/list", null);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
