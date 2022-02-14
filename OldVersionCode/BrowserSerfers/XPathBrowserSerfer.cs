/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Mouse_Hunter.BrowserSerfers
{
    public class XPathBrowserSerfer : BrowserSerfer
    {
        public XPathBrowserSerfer(IWebDriver browser, MouseOperator mouseOperator, WebDriverWait ww)
            : base(browser, mouseOperator, ww) { }


        public List<IWebElement> GetElementsByXPath(string xpath, int predelayPower)
        {
            SimulateDelay(predelayPower);
            return Browser.FindElements(By.XPath(xpath)).ToList();
            //return ww.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(xpath))).ToList();
        }

        public IWebElement GetElementByXPath(string xpath, int predelayPower)
        {
            SimulateDelay(predelayPower);
            return Browser.FindElement(By.XPath(xpath));
        }


        public IWebElement GoToElementByXPath(string xpath, int predelayPower, Point OffsetPoint)
        {
            var element = GoToElementByXPathNoClick(xpath, predelayPower, OffsetPoint);
            MouseMover.Click();
            return element;
        }

        public IWebElement GoToElementByXPathNoClick(string xpath, int predelayPower, Point OffsetPoint)
        {
            var element = GetElementByXPath(xpath, predelayPower);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveToArea(points.LeftPoint, points.RightPoint);
            return element;
        }


        public IWebElement GoToElementByXPathTestCorners(string cssSelector, int predelayPower, Point OffsetPoint)
        {
            var element = GetElementByXPath(cssSelector, predelayPower);
            var points = GetPoints(element, OffsetPoint);
            MouseMover.MoveBySinglePoint(points.LeftPoint.X, points.LeftPoint.Y);
            Thread.Sleep(5000);
            MouseMover.MoveBySinglePoint(points.RightPoint.X, points.RightPoint.Y);
            return element;
        }
        public void SendTokenToCaptchaByXPath(string keysText, string captchaXPath, int predelayPower)
        {
            var element = GetElementByXPath(captchaXPath, predelayPower);
            IJavaScriptExecutor js = (IJavaScriptExecutor)Browser;
            //js.ExecuteScript($"arguments[0].value='{keysText}'", element);
            js.ExecuteScript($"arguments[0].innerHTML='{keysText}'", element);
            //js.executeScript("document.getElementById('g-recaptcha-response').innerHTML='" + responseToken + "';");
            Thread.Sleep(500);
            js.ExecuteScript($"___grecaptcha_cfg.clients[0].G.G.callback('{keysText}')");
            Thread.Sleep(500);
            *//*            js.ExecuteScript("var element=document.getElementById(\"g-recaptcha-response\"); element.style.display=\"\";");
                        js.ExecuteScript("\"\"document.getElementById(\"g-recaptcha-response\").innerHTML = arguments[0]\"\"", keysText);
                        js.ExecuteScript("var element=document.getElementById(\"g -recaptcha-response\"); element.style.display=\"none\";");

            *//*
            //js.ExecuteScript($"document.getElementByXPath('{captchaXPath}').value='{keysText}'");

            //((JavascriptExecutor) driver).executeScript("arguments[0].className='clearr'",element);


            //js.ExecuteScript($"document.evaluate({captchaXPath}, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.value='{keysText}'");
        }
        public void SelectDropListByXPathEnterKey(string xpath, int elementNum, int predelayPower)
        {
            string key = Keys.Down;
            if (elementNum < 0)
                key = Keys.Up;
            Random random = new Random();
            var element = GetElementByXPath(xpath, predelayPower);
            Thread.Sleep(random.Next(2000, 3000));
            for (int i = 0; i < Math.Abs(elementNum); i++)
            {
                element.SendKeys(key);
                Thread.Sleep(random.Next(250, 3000));
            }
            Thread.Sleep(random.Next(3000, 4000));
            element.SendKeys(Keys.Enter);
            Thread.Sleep(random.Next(2000, 3000));
        }

        public void SendKeysToXPathElement(string xpath, string keysText, int predelayPower)
        {
            var inputName = GetElementByXPath(xpath, predelayPower);
            inputName.SendKeys(keysText);
        }

        public string GoToRandElInXPathStruct(string xpath, int predelayPower)
        {
            var elements = GetElementsByXPath(xpath, predelayPower);
            var element = elements[GetRandomServiceNum(elements)];
            var text = string.Copy(element.Text);
            element.Click();
            return text;
        }
        public void PasteInFieldByXPath(string xPath, string text, XPathBrowserSerfer xPathSerfer, Point offsetPoint)
        {
            var fNameField = xPathSerfer.GoToElementByXPath(xPath, 1, offsetPoint);
            xPathSerfer.PasteAsHumanTyping(fNameField, text);
        }

        private static int GetRandomServiceNum(List<IWebElement> services)
        {
            Random random = new Random();
            return random.Next(1, services.Count);
        }
    }
}
*/