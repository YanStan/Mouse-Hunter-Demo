using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.Wrappers
{
    public class Proxies
    {
        private List<string> ProxyList;
        private string ActiveProxy;

        public Proxies(List<string> proxyList)
        {
            ProxyList = proxyList;
            ActiveProxy = "localhost";
        }
        public string GetActiveProxy() => ActiveProxy;
        public string GetNextProxy()
        {
            if (ActiveProxy == "localhost")
            {
                ActiveProxy = ProxyList[0];
            }
            else
            {
                int nextProxyIndex = ProxyList.IndexOf(ActiveProxy) + 1;
                if (nextProxyIndex > ProxyList.Count - 1)
                    ActiveProxy = null;
                else
                    ActiveProxy = ProxyList[nextProxyIndex];
            }
            return ActiveProxy;
        }
    }
}
