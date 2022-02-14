using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SeleniumHappyMin_test1.Tools
{
    public class BrowserCreator
    {
        public IWebDriver Create()
        {
            var options = new ChromeOptions();
            List<string> args = new List<string>() {
                "--ignore-certificate-errors",
                "--ignore-ssl-errors=yes",
                "--​ignore-certificate-errors-spki-list",
                "--disable-blink-features",
                "--disable-blink-features=AutomationControlled",//disable navigator.webdriver == true
                "--profile-directory=Default",
                "start-maximized"
                };
            options.AddArguments(args);
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);
            options.AddUserProfilePreference("Page.addScriptToEvaluateOnNewDocument", //deleting this navigator at all for headless chrome!
                "const newProto = navigator.proto;delete newProto.webdriver;navigator.proto = newProto;");

            browser = new ChromeDriver(@"C:\Users\User\UndetectChromeDriver", options);
            //TODO delete all cookies if nessessary

            //again antidetect
            //TODO sould I put string somewhere??
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser; //disable navigator.webdriver in alternative way.
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
            return browser;
        }
    }
}