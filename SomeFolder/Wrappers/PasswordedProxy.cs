using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.Wrappers
{
    public class PasswordedProxy
    {
        public readonly string ProxyAddress;
        public readonly int ProxyPort;
        public readonly string ProxyLogin;
        public readonly string ProxyPassword;

        public PasswordedProxy(string address, int port, string login, string pass)
        {
            ProxyAddress = address;
            ProxyPort = port;
            ProxyLogin = login;
            ProxyPassword = pass;
        }
    }
}
