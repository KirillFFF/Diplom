using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ProductsServer.Classes
{
    public class IPAddressClient
    {
        private static volatile IPAddressClient _class;
        public static IPAddressClient Instance
        {
            get
            {
                if (_class == null)
                    _class = new IPAddressClient();

                return _class;
            }
        }

        public string Get(HttpContext httpContext)
        {
            string ipAddress = string.Empty;
            IPAddress ip = httpContext.Connection.RemoteIpAddress;
            if (ip != null)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    ip = Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);

                ipAddress = ip.ToString();
            }

            return ipAddress;
        }

        public string Get(TokenValidatedContext httpContext)
        {
            string ipAddress = string.Empty;
            IPAddress ip = httpContext.HttpContext.Connection.RemoteIpAddress;
            if (ip != null)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    ip = Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);

                ipAddress = ip.ToString();
            }

            return ipAddress;
        }
    }
}
