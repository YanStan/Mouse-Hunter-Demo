/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class RusDataParser : AbstractBrowserWorker
    {
        private List<string> Names = new List<string>()
        {
             "Александр",
             "Дмитрий",
             "Сергей",
             "Андрей",
             "Алексей",
             "Артём",
             "Иван",
             "Владислав",
             "Владимир",
             "Константин",
             "Николай"
        };

        public RusDataParser(IWebDriver browser) : base(browser)
        {
            OffsetPointScrolled = new Point
            {
                X = OffsetPoint.X,
                Y = OffsetPoint.Y
            };
        }
        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }

        public override void RefreshOffset()
        {
            OffsetPointScrolled.X = OffsetPoint.X;
            OffsetPointScrolled.Y = OffsetPoint.Y;
        }
        public override void Execute(IProgress<string> progress)
        {
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            var mouseMover = new MouseOperator(1, 3000);
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, mouseMover, ww);
            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, mouseMover, ww);

            cssSerfer.NavigateToSite("https://www.reestr-zalogov.ru/search/index");
            Thread.Sleep(1000);
            SendKeys.SendWait("2mvdtM{TAB}xm6Epj{ENTER}");   
            Thread.Sleep(1500);
            progress.Report("Бот перешел на сайт рос. даннных.\r\n");

            string surnamesFile = @"filtered_russian_surnames_rus.txt";
            string surname = GetRandomRowFromTxt(progress, surnamesFile);

            foreach (string name in Names)
            {
                TryParse(progress, cssSerfer, xPathSerfer, name, surname);
            }
        }

        private void TryParse(IProgress<string> progress, CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer,
             string name, string surname)
        {
            try
            {
                xPathSerfer.GoToElementByXPath("//*[text()=\"По информации о залогодателе\"]", 1, OffsetPointScrolled);
                cssSerfer.PasteInFieldByCss("[id^=\"privatePerson.lastName\"]", surname, cssSerfer, 1, OffsetPointScrolled);
                Parse(progress, cssSerfer, xPathSerfer, name);
            }
            catch (NoSuchElementException)
            {
                OffsetScrollDown(xPathSerfer, 5);
                cssSerfer.GoToElementByCss("#back-btn", 1, OffsetPointScrolled);
                RefreshOffset();
                Browser.Navigate().Refresh();
            }
        }

        private void Parse(IProgress<string> progress, CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer, string name)
        {
            cssSerfer.PasteInFieldByCss("[id^=\"privatePerson.firstName\"]", name, cssSerfer, 1, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 6);
            OffsetPointScrolled.Y += 40;
            cssSerfer.GoToElementByCss("#find-btn", 1, OffsetPointScrolled);

            OffsetScrollUp(xPathSerfer, 8);
            RefreshOffset();
            OffsetScrollDown(xPathSerfer, 1);
            var pledgeNotices = xPathSerfer.GetElementsByXPath($"//*[text()=\"История изменений\"]", 5); //Delay for Loading page.
            var pledgerBirthDatesNPledgees = cssSerfer.GetElementsByCss($"td > div > div > a", 2);

            Random random = new Random();
            for(int i = 0; i < pledgerBirthDatesNPledgees.Count; i+=2)
            {
                string pledgerData = pledgerBirthDatesNPledgees[i].Text;
                int date = int.Parse(pledgerData.Substring(pledgerData.Length - 4, 4));
                if (date >= 1988)
                {
                    xPathSerfer.GoToElementByElement(pledgeNotices[i], 1, OffsetPointScrolled);
                    GetPdf(cssSerfer, xPathSerfer, random);
                }
                OffsetScrollDown(xPathSerfer, 1);
            }
            OffsetScrollUp(xPathSerfer, 10);
            RefreshOffset();
            Browser.Navigate().Refresh();
        }

        private void GetPdf(CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer, Random random)
        {
            var changelog = cssSerfer.GetElementsByCss("tr > td > span", 1);
            var element = changelog[changelog.Count - 1];
            xPathSerfer.GoToElementByElement(element, 1, OffsetPointScrolled);
            Thread.Sleep(random.Next(3650, 4150));
            xPathSerfer.ClickOnCoords(1348, 700); //hiding download bar
            xPathSerfer.GoToElementByXPath($"//*[text()=\"ЗАКРЫТЬ\"]", 1, OffsetPointScrolled);
            Thread.Sleep(random.Next(300, 500));
        }
    }
}
*/