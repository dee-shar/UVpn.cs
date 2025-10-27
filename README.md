# UVpn.cs
Web-API for [uvpn.me](https://uvpn.me) that will give you private internet access and the best internet security so public Wi-Fi hotspot will never be a problem for you

## Example
```cs
using UVpnApi;

namespace Application
{
    internal class Program
    {
        static async Task Main()
        {
            var api = new UVpn();
            await api.Login();
            string servers = await api.GetServers();
            Console.WriteLine(servers);
        }
    }
}
```
