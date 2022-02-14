/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Mouse_Hunter.BrowserSerfers
{
    public class CssBrowserSerfer : BrowserSerfer
    {
        public CssBrowserSerfer(IWebDriver browser, MouseOperator mouseOperator, WebDriverWait ww)
        : base(browser, mouseOperator, ww) { }


        public List<IWebElement> GetElementsByCss(string cssSelector, int predelayPower)
        {
            //MoveSenseless(predelayPower);
            SimulateDelay(predelayPower);
            return Browser.FindElements(By.CssSelector(cssSelector)).ToList();
            //return ww.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(cssSelector))).ToList();
        }

        private IWebElement GetElementByCss(string cssSelector, int predelayPower)
        {
            SimulateDelay(predelayPower);
            //MoveSenseless(predelayPower);
            return Browser.FindElement(By.CssSelector(cssSelector));
        }

        public IWebElement GoToElementByCss(string cssSelector, int predelayPower, Point OffsetPoint)
        {

            var element = GoToElementByCssNoClick(cssSelector, predelayPower, OffsetPoint);
            MouseMover.Click();
            return element;
        }

        public IWebElement GoToElementByCssNoClick(string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var element = GetElementByCss(cssSelector, predelayPower);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveToArea(points.LeftPoint, points.RightPoint);
            return element;
        }
        public IWebElement GoToElementByElementTestCorners(IWebElement element, int predelayPower, Point OffsetPoint)
        {
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveBySinglePoint(points.LeftPoint.X, points.LeftPoint.Y);
            Thread.Sleep(5000);
            MouseMover.MoveBySinglePoint(points.RightPoint.X, points.RightPoint.Y);
            return element;
        }
        public IWebElement GoToElementByCssTestCorners(string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var element = GetElementByCss(cssSelector, predelayPower);
            return GoToElementByElementTestCorners(element, predelayPower, OffsetPoint);
        }



        public IWebElement GoToElementByCssNoClickWithProgress(IProgress<string> progress, string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var element = GetElementByCss(cssSelector, predelayPower);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveToAreaWithProgress(progress, points.LeftPoint, points.RightPoint);
            return element;
        }
        public IWebElement GoToElementByCssWithProgress(IProgress<string> progress, string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var element = GoToElementByCssNoClickWithProgress(progress, cssSelector, predelayPower, OffsetPoint);
            MouseMover.Click();
            return element;
        }

        public IWebElement GoToElementByCssDoubleClick(string cssSelector, int predelayPower, Point offsetPoint)
        {
            var element = GoToElementByCssNoClick(cssSelector, predelayPower, offsetPoint);
            MouseMover.DoubleClick();
            return element;
        }


        public void SelectDropListByCss(string cssSelector, int elementNum, int predelayPower, Point OffsetPoint)
        {
            string key = Keys.Down;
            if (elementNum < 0)
                key = Keys.Up;

            var element = GoToElementByCss(cssSelector, predelayPower, OffsetPoint);
            SendKeyByCycleToElement(element, elementNum, key);
        }
        public void SelectDropListByCssNoClick(string cssSelector, int elementNum, int predelayPower, Point OffsetPoint)
        {
            string key = Keys.Down;
            if (elementNum < 0)
                key = Keys.Up;

            var element = GoToElementByCssNoClick(cssSelector, predelayPower, OffsetPoint);
            SendKeyByCycleToElement(element, elementNum, key);
        }

        private static void SendKeyByCycleToElement(IWebElement element ,int elementNum, string key) 
        {
            Random random = new Random();
            Thread.Sleep(random.Next(200, 300));
            for (int i = 0; i < Math.Abs(elementNum); i++)
            {
                element.SendKeys(key);
                Thread.Sleep(random.Next(25, 300));
            }
            Thread.Sleep(random.Next(300, 400));
            element.SendKeys(Keys.Enter);
            Thread.Sleep(random.Next(200, 300));
        }

        public void SendKeysToCssElement(string cssSelector, string keysText, int predelayPower)
        {
            var inputName = GetElementByCss(cssSelector, predelayPower);
            inputName.SendKeys(keysText);
        }

        public string GoToRandElInCssStruct(string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var elements = GetElementsByCss(cssSelector, predelayPower);
            var element = elements[GetRandomServiceNum(elements)];
            var text = string.Copy(element.Text);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveToArea(points.LeftPoint, points.RightPoint);
            return text;
        }

        public void PasteInFieldByCss(string cssSelector, string text, CssBrowserSerfer cssSerfer, int predelayPower, Point offsetPoint)
        {
            var fNameField = cssSerfer.GoToElementByCss(cssSelector, predelayPower, offsetPoint);
            cssSerfer.PasteAsHumanTyping(fNameField, text);
        }

        public void PasteInFieldByElement(IWebElement element, string text, int predelayPower, Point offsetPoint)
        {
            GoToElementByElement(element, predelayPower, offsetPoint);
            PasteAsHumanTyping(element, text);
        }

        private static int GetRandomServiceNum(List<IWebElement> services)
        {
            Random random = new Random();
            return random.Next(1, services.Count);
        }
    }
}
*/