using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.AccountsSheetModels
{
    public class EmailProfile//001_Bot_WH_UA:212.81.37.21:9071:2mvdtM:xm6Epj:golikeswan:001YanYanYan
    {
        public string ProfileName { get; private set; }
        public string Ip { get; private set; }
        public string Port { get; private set; }
        public string ProxyLogin { get; private set; }
        public string ProxyPass { get; private set; }
        public string MailLogin { get; private set; }
        public string MailPass { get; private set; }

        public EmailProfile(string colonSeparatedValues)
        {
            string[] valuesArray = colonSeparatedValues.Split(':');
            ProfileName = valuesArray[0];
            Ip = valuesArray[1];
            Port = valuesArray[2];
            ProxyLogin = valuesArray[3];
            ProxyPass = valuesArray[4];
            MailLogin = valuesArray[5];
            MailPass = valuesArray[6];
        }
    }
}
