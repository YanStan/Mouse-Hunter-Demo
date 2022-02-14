/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.Wrappers;
using System;
using System.Drawing;
using System.Threading;

namespace Mouse_Hunter.BrowserSerfers
{
    public class BrowserSerfer
    {
        protected IWebDriver Browser;
        protected MouseOperator MouseMover;
        protected WebDriverWait WW;

        public int ScrollDown(int amount) => MouseMover.ScrollDown(amount);
        public int ScrollUp(int amount) => MouseMover.ScrollUp(amount);

        public BrowserSerfer(IWebDriver browser, MouseOperator mouseOperator, WebDriverWait ww)
        {
            Browser = browser;
            MouseMover = mouseOperator;
            WW = ww;
        }

        public void ChangeMouseSpeed(double speed) => MouseMover.ChangeSpeedMultiPlyer(speed);

        public void NormalizeMouseSpeed() => MouseMover.NormalizeSpeedMultiPlyer();

        public Points GetPoints(IWebElement element, Point offsetPoint) => new Points(element, offsetPoint);

        public void NavigateToSite(string url) => Browser.Navigate().GoToUrl(url);

        public void RefreshPage() => Browser.Navigate().Refresh();
        public void AcceptAlert(int predelayPower)
        {
            SimulateDelay(predelayPower);
            IAlert alert = Browser.SwitchTo().Alert();
            alert.Accept();
        }

        public void SendTokenToCaptcha(string keysText, string captchaId, int predelayPower)
        {
            SimulateDelay(predelayPower);
            IJavaScriptExecutor js = (IJavaScriptExecutor)Browser;
            js.ExecuteScript($"document.getElementById('{captchaId}').value='{keysText}'");
        }

        public void GoToElementByLinkText(string linkText, int predelayPower)
        {
            SimulateDelay(predelayPower);
            IWebElement element = Browser.FindElement(By.LinkText(linkText));
            element.Click();
        }
        public void GoToElementByElementNoClick(IWebElement element, int predelayPower, Point OffsetPoint)
        {
            SimulateDelay(predelayPower);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveToArea(points.LeftPoint, points.RightPoint);
        }
        public void GoToElementByElement(IWebElement element, int predelayPower, Point OffsetPoint)
        {
            GoToElementByElementNoClick(element, predelayPower, OffsetPoint);
            MouseMover.Click();
        }

        public void PasteAsHumanTyping(IWebElement element, string text)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(500, 1000));
            foreach (char character in text)
            {
                element.SendKeys(character.ToString());
                Thread.Sleep(random.Next(70, 300));
            }
        }
        public void MoveSenseless(int predelayPower, int speed = 1)
        {
            //TODO change 3000 if want  (later.)
            SenselessMover senseless = new SenselessMover(speed, 3000);
            senseless.SmallMoveRandom(predelayPower);
        }


        public void ClickOnCoords(int x, int y)
        {
            Random random = new Random();
            MouseMover.MoveBySinglePoint(x, y);
            MouseMover.Click();
            Thread.Sleep(random.Next(55, 100));
        }
        protected string SimulateDelay(int predelayPower)
        {
            Random random = new Random();
            int delayMs = random.Next(1000 * predelayPower, 2200 * predelayPower);
            int smallerDelayMs = (int)Math.Round((double)delayMs / 3);
            Thread.Sleep(Math.Max(87, smallerDelayMs));
            MoveSenseless(predelayPower);
            Thread.Sleep(smallerDelayMs * 2);
            MoveSenseless(predelayPower);
            return $"Бот симулирует задержку в {delayMs} мс.\r\n\r\n";
        }
    }
}
*/