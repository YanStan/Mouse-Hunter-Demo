/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.Repositories;
using Mouse_Hunter.Resources.AntiCaptcha;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class RamblersRegistrator : AbstractBrowserWorker
    {
        public RamblersRegistrator(IWebDriver browser) : base(browser)
        {
            OffsetPointScrolled = new Point
            {
                X = OffsetPoint.X,
                Y = OffsetPoint.Y
            };
        }
        public override void RefreshOffset()
        {
            OffsetPointScrolled.X = OffsetPoint.X;
            OffsetPointScrolled.Y = OffsetPoint.Y;
        }

        public override void Execute(IProgress<string> progress)
        {
            Random random = new Random();
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            var mouseMover = new MouseOperator(1, 3000);
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, mouseMover, ww);

            TxtRepository txtRepos = new TxtRepository();
            var lines = txtRepos.GetStringArrayFromTxt(@"ramblerdata.txt");
            var profiles = lines.Select(x => new EmailProfile(x));
            *//*            foreach(var profile in profiles)
                        {
                            progress.Report($"Профиль: {profile.ProfileName}\r\n" +
                                $"Ip: {profile.Ip}\r\n" +
                                $"Порт: {profile.Port}\r\n" +
                                $"ПроксиЛогин: {profile.ProxyLogin}\r\n" +
                                $"ПроксиПасс: {profile.ProxyPass}\r\n" +
                                $"Логин: {profile.MailLogin}\r\n" +
                                $"Пасс: {profile.MailPass}\r\n\r\n");
                        }*//*


            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, mouseMover, ww);
            for (int i = 0; i < 5; i++)
            {
                Registrate(progress, random, cssSerfer, xPathSerfer);
                RefreshOffset();
            }
        }

        private void Registrate(IProgress<string> progress, Random random, CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer)
        {
            cssSerfer.NavigateToSite("https://www.rambler.ru/");
            progress.Report("Бот зашел на rambler.ru\r\n");
            //Войти в почту
            cssSerfer.GoToElementByCss("[class^=\"rui__3ILc74r\"]", 1, OffsetPointScrolled);
            Thread.Sleep(random.Next(10000, 11000));
            Browser.SwitchTo().Window(Browser.WindowHandles.Last());
            AdaptOffsetToIframe(xPathSerfer);
            OffsetScrollDown(cssSerfer, 1);

            //Registration
            xPathSerfer.GoToElementByXPath("//*[text()=\"Регистрация\"]", 1, OffsetPointScrolled);
            //Getting all input fields (elements)
            var inputElements = cssSerfer.GetElementsByCss("[class^=\"rui-Input-input -metrika-nokeys\"]", 1);
            //Paste email login
            cssSerfer.PasteInFieldByElement(inputElements[0], "omesnogil", 1, OffsetPointScrolled);
            //Paste password
            cssSerfer.PasteInFieldByElement(inputElements[2], "1000YanYanYan", 1, OffsetPointScrolled);
            //Again paste password
            cssSerfer.PasteInFieldByElement(inputElements[3], "1000YanYanYan", 1, OffsetPointScrolled);
            //Choosing question
            cssSerfer.SelectDropListByCss("#question", 1, 1, OffsetPointScrolled);
            //Paste answer
            cssSerfer.PasteInFieldByElement(inputElements[4], "Da", 1, OffsetPointScrolled);

            OffsetScrollDown(cssSerfer, 1);
            Thread.Sleep(2000);

            CaptchaSender sender = new CaptchaSender();
            progress.Report("Проходится капча, ожидайте результата...");
            string token = sender.DoRecaptcha2Proxyless("https://www.rambler.ru/", "6LeHeSkUAAAAANUvgxwQ6HOLXCT6w6jTtuJhpLU7");
            if (token != null)
            {
                xPathSerfer.SendTokenToCaptchaByXPath(token, "(//*[@name=\"g-recaptcha-response\"])[1]", 1);
                progress.Report("Капча пройдена!\r\n");
            }
            else
            {
                progress.Report("Сервису Антикапча не удалось пройти прокси!\r\n");
            }
      
            //Finish Registry
            Thread.Sleep(1000);
            //cssSerfer.GoToElementByCss("[class^=\"rui-Button-content\"]", 1, OffsetPointScrolled);
            Thread.Sleep(600000);
        }

        private void AdaptOffsetToIframe(XPathBrowserSerfer xPathSerfer)
        {
            Browser.SwitchTo().DefaultContent();
            int registryFrameOffsetX = 582;
            int registryFrameOffsetY = 53;
            OffsetPointScrolled.X += registryFrameOffsetX;
            OffsetPointScrolled.Y += registryFrameOffsetY;
            var frame = xPathSerfer.GetElementByXPath("//*[@id=\"root\"]/div/div[2]/div[2]/iframe", 1);
            Browser.SwitchTo().Frame(frame);

        }
        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }
    }
}
*/