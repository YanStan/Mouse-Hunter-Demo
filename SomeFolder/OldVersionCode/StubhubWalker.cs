/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using System;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class StubhubWalker : AbstractBrowserWorker
    {
        public StubhubWalker(IWebDriver browser) : base(browser) { }
        public override void RefreshOffset() => OffsetPointScrolled.Y = OffsetPoint.Y;

        public override void Execute(IProgress<string> progress)
        {
            MouseOperator moveMover = new MouseOperator(1, 3000);
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, moveMover, ww);

            xPathSerfer.NavigateToSite("https://www.stubhub.com/");
            progress.Report("Бот перешел на сайт stubhub.com.\r\n");


            for (int i = 0; i < 5; i++)
            {
                xPathSerfer.GoToElementByXPath("//*[text()=\"Pop\"]", 3, OffsetPoint);
                progress.Report("Pop clicked!\r\n");

                xPathSerfer.GoToElementByXPath("//*[text()=\"Festivals\"]", 3, OffsetPoint);
                progress.Report("Festivals clicked!\r\n");
            }
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, moveMover, ww);
            cssSerfer.GoToElementByCss("[class^=\"mh__nav-menu-header-text menu__header-text-sports\"", 3, OffsetPoint);
            progress.Report("Sports clicked!\r\n");
        }

        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }
    }
}
*/