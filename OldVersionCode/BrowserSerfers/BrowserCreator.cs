/*using System.Collections.Generic;

namespace Mouse_Hunter.BrowserSerfers
{
    public class BrowserCreator
    {
        public IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArguments();
            
            List<string> args = new List<string>() {
                "--ignore-certificate-errors",
                "--ignore-ssl-errors=yes",
                "--​ignore-certificate-errors-spki-list",
                "--disable-blink-features",
                "--disable-blink-features=AutomationControlled",//disable navigator.webdriver == true
                "--profile-directory=Default",
                "start-maximized",
                //specify location for profile creation/ access
                //@"user-data-dir=C:\Users\User\AppData\Local\Google\Chrome\User Data\NAMEYOUCHOOSE"
                };
            options.AddArguments(args);
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);
            options.AddUserProfilePreference("Page.addScriptToEvaluateOnNewDocument", //deleting this navigator at all for headless chrome!
                "const newProto = navigator.proto;delete newProto.webdriver;navigator.proto = newProto;");

            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);//saving pdf instead opening
            options.AddUserProfilePreference("credentials_enable_service", false); //disabling save password popup
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            var browser = new ChromeDriver(options);
            //TODO delete all cookies if nessessary

            //again antidetect
            IJavaScriptExecutor js = browser; //disable navigator.webdriver in alternative way.
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
            return browser;
        }
    }
}*/