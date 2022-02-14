using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.AccountsSheetModels
{
    public class AccountProfile
    {
        public readonly string AntidetectIndex;
        public readonly string ActivityState;
        public readonly string FinishState;
        public readonly string DateOfFullCreation;
        public readonly string ProfileName;
        public readonly PasswordedProxy Proxy;
        public readonly string BrowserResolution;
        public readonly AccountRealData AccountData;
        public readonly string PostIndex;
        public readonly string EmailNPaymentLogin;
        public readonly string EmailNPaymentPass;
        public readonly string PaymentType;
        public readonly string PaymentPin;
        public readonly string Login;
        public readonly string Password;
        public readonly string QuestionType;
        public readonly string Answer;

        public AccountProfile(IList<object> dataRow)
        {
            string[] dataLine = dataRow.OfType<string>().ToArray();         
            ActivityState = dataLine[0];
            FinishState = dataLine[1];
            DateOfFullCreation = dataLine[2];
            AntidetectIndex = dataLine[3];
            ProfileName = dataLine[4];
            var proxyData = dataLine[5].Split(':');
            Proxy = new PasswordedProxy(proxyData[0], int.Parse(proxyData[1]), proxyData[2], proxyData[3]);
            BrowserResolution = dataLine[6];
            AccountData = new AccountRealData(dataLine[7]);
            PostIndex = dataLine[8];
            EmailNPaymentLogin = dataLine[9];
            EmailNPaymentPass = dataLine[10];
            PaymentType = dataLine[11];
            PaymentPin = dataLine[12];
            Login = dataLine[13];
            Password = dataLine[14];
            QuestionType = dataLine[15];
            Answer = dataLine[16];
        }
        public override string ToString() =>
            $"ActivityState: {ActivityState}\r\n" +
            $"FinishState: {FinishState}\r\n" +
            $"DateOfFullCreation: {DateOfFullCreation}\r\n" +
            $"AntidetectIndex: {AntidetectIndex}\r\n" +
            $"ProfileName: {ProfileName}\r\n" +
            $"ProxyAddress: {Proxy.ProxyAddress}\r\n" +
            $"ProxyPort: {Proxy.ProxyPort}\r\n" +
            $"ProxyLogin: {Proxy.ProxyLogin}\r\n" +
            $"ProxyPassword: {Proxy.ProxyPassword}\r\n" +
            $"BrowserResolution: {BrowserResolution}\r\n" +
            $"Surname: {AccountData.Surname}\r\n" +
            $"Name: {AccountData.Name}\r\n" +
            $"PatroName: {AccountData.PatroName}\r\n" +
            $"BirthYear: {AccountData.BirthYear}\r\n" +
            $"BirthMonth: {AccountData.BirthMonth}\r\n" +
            $"BirthDay: {AccountData.BirthDay}\r\n" +
            $"PostIndex: {PostIndex}\r\n" +
            $"EmailNPaymentLogin: {EmailNPaymentLogin}\r\n" +
            $"EmailNPaymentPass: {EmailNPaymentPass}\r\n" +
            $"PaymentType: {PaymentType}\r\n" +
            $"PaymentPin: {PaymentPin}\r\n" +
            $"Login: {Login}\r\n" +
            $"Password: {Password}\r\n" +
            $"QuestionType: {QuestionType}\r\n" +
            $"Answer: {Answer}\r\n\r\n";
    }
}
