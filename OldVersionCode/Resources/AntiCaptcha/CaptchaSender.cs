using System;
using Mouse_Hunter.Resources.AntiCaptcha.Helper;
using Mouse_Hunter.Resources.AntiCaptcha.Api;
using Mouse_Hunter.Wrappers;

namespace Mouse_Hunter.Resources.AntiCaptcha
{
    class CaptchaSender
    {
        private readonly string ClientKey = ClientKeyHolder.ClientKey;

        public void GetBalance()
        {
            DebugHelper.VerboseMode = true;
            var api = new ImageToText
            {
                ClientKey = ClientKey
            };
            var balance = api.GetBalance();
            if (balance == null)
                DebugHelper.Out("GetBalance() failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else
                DebugHelper.Out("Balance: " + balance, DebugHelper.Type.Success);
        }

        public string DoRecaptcha2Proxyless(string url, string sitekey)
        {
            DebugHelper.VerboseMode = true;

            var api = new RecaptchaV2Proxyless
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri(url),
                WebsiteKey = sitekey
            };

            if (!api.CreateTask())
            {
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                return null;
            }
            else if (!api.WaitForResult())
            {
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                return null;
            }
            else
            {
                var token = api.GetTaskSolution().GRecaptchaResponse;
                DebugHelper.Out("Result: " + token, DebugHelper.Type.Success);
                return token;
            }
        }
        public string DoHCaptchaProxyless(string url, string sitekey)
        {
            DebugHelper.VerboseMode = true;

            var api = new HCaptchaProxyless
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri(url),
                WebsiteKey = sitekey
            };

            if (!api.CreateTask())
            {
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                return null;
            }
            else if (!api.WaitForResult())
            {
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                return null;
            }
            else
            {
                var token = api.GetTaskSolution().GRecaptchaResponse;
                DebugHelper.Out("Result: " + token, DebugHelper.Type.Success);
                return token;
            }
        }

        public string DoHCaptchaWithProxy(string url, string sitekey)
        {
            DebugHelper.VerboseMode = true;

            var api = new HCaptcha
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri(url),
                WebsiteKey = sitekey,
                UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36",
                // proxy access parameters
                ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                ProxyAddress = "193.23.50.188",
                ProxyPort = 10609,
                ProxyLogin = "vladheylo8649",
                ProxyPassword = "c5df3b"
            };
            if (!api.CreateTask())
            {
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                return null;
            }
            else if (!api.WaitForResult())
            {
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                return null;
            }
            else
            {
                var token = api.GetTaskSolution().GRecaptchaResponse;
                DebugHelper.Out("Result: " + token, DebugHelper.Type.Success);
                return token;
            }
        }

      
        public string DoRecaptcha2WithProxy(string url, string sitekey, PasswordedProxy proxy)
        {
            DebugHelper.VerboseMode = true;

            var api = new RecaptchaV2()
            {
                ClientKey = ClientKey,
                WebsiteUrl = new Uri(url),
                WebsiteKey = sitekey,
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36",
                // proxy access parameters
                ProxyType = AnticaptchaBase.ProxyTypeOption.Http,
                ProxyAddress = proxy.ProxyAddress,
                ProxyPort = proxy.ProxyPort,
                ProxyLogin = proxy.ProxyLogin,
                ProxyPassword = proxy.ProxyPassword
            };

            if (!api.CreateTask())
            {
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                return null;
            }
            else if (!api.WaitForResult())
            {
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                return null;
            }
            else
            {
                var token = api.GetTaskSolution().GRecaptchaResponse;
                DebugHelper.Out("Result: " + token, DebugHelper.Type.Success);
                return token;
            }
        }
    }
}
