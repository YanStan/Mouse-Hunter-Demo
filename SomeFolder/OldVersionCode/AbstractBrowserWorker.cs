/*using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.Wrappers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.ScenarioWorkers
{
    public abstract class AbstractBrowserWorker
    {
        protected readonly IWebDriver Browser;
        protected Point OffsetPoint;
        protected Point OffsetPointScrolled;
        protected readonly int BrowserNavigationPanelWidth;
        protected readonly int BrowserNavigationPanelHeight;

        public AbstractBrowserWorker(IWebDriver browser)
        {
            Browser = browser;
            IJavaScriptExecutor js = (ChromeDriver)Browser;
            BrowserNavigationPanelWidth = Convert.ToInt32(js.ExecuteScript("return window.outerWidth - window.innerWidth;"));
            BrowserNavigationPanelHeight = Convert.ToInt32(js.ExecuteScript("return window.outerHeight - window.innerHeight;"));
            OffsetPoint = new Point
            {
                X = 0,
                Y = BrowserNavigationPanelHeight
            };
        }

        public void OffsetScrollDown(BrowserSerfer serfer, int amount) =>
            OffsetPointScrolled.Y += serfer.ScrollDown(amount);

        public void OffsetScrollUp(BrowserSerfer serfer, int amount) =>
            OffsetPointScrolled.Y += serfer.ScrollUp(amount);

        public abstract void RefreshOffset();
        public abstract void Execute(IProgress<string> progress);

        protected static string GetRandomRowFromTxt(IProgress<string> progress, string fileName)
        {
            Random random = new Random();
            var tempFile = Path.GetTempFileName();
            var lines = File.ReadAllLines(fileName, Encoding.UTF8).ToList();
            int rowNum = random.Next(0, lines.Count);
            var line = lines[rowNum];
            lines.RemoveAt(rowNum);
            File.WriteAllLines(tempFile, lines);

            File.Delete(fileName);
            File.Move(tempFile, fileName);
            return line;
        }
        protected bool ChangeProxyIfLimited(IProgress<string> progress, XPathBrowserSerfer xPathSerfer, Proxies proxies)
        {
            bool isLimited = IsConnectionLimited(xPathSerfer);
            if (isLimited)
            {
                progress.Report("Переключение прокси...\r\n");
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                registry.SetValue("ProxyEnable", 1);
                var proxy = proxies.GetNextProxy();
                if (proxy != null)
                {
                    registry.SetValue("ProxyServer", proxy);
                }
                else
                {
                    progress.Report("Список проксей исчерпан.");
                    Browser.Close();
                    return true;
                }
                Browser.Navigate().Refresh();
                Thread.Sleep(1000);
                SendKeys.SendWait("need_leonid_gmail_co{TAB}7f7e0fadd6{ENTER}");
                Thread.Sleep(1500);
                RefreshOffset();
                ChangeProxyIfLimited(progress, xPathSerfer, proxies);
            }
            return isLimited;
        }

        protected abstract bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer);
    }
}
*/