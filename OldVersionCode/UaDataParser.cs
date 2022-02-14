/*using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Mouse_Hunter.Wrappers;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class UaDataParser : AbstractBrowserWorker
    {
*//*        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool settingsReturn, refreshReturn;*//*

        public UaDataParser(IWebDriver browser) : base(browser)
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
        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer) =>
            xPathSerfer.GetElementByXPath("//body", 0).Text == "Вы исчерпали лимит. Зайдите завтра...";

        public override void Execute(IProgress<string> progress)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 0);

            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            var mouseMover = new MouseOperator(1, 3000);
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, mouseMover, ww);
            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, mouseMover, ww);  
            
            cssSerfer.NavigateToSite("http://nomer-org.website/allukraina/");
            Thread.Sleep(1000);     
            progress.Report("Бот перешел на сайт укр. даннных.\r\n");

            Proxies proxies = new Proxies(new List<string>
            {
"45.67.121.103:30038",
"45.67.120.43:30038",
"45.67.122.198:30038",
"45.148.152.118:30038",
"45.148.152.167:30038",
"45.67.121.10:30038",
"45.67.123.228:30038",
"45.148.155.115:30038",
"45.148.153.10:30038",
"45.67.123.113:30038",
"45.67.123.13:30038",
"45.148.154.195:30038",
"45.148.152.220:30038",
"45.67.123.108:30038",
"45.67.121.90:30038",
"45.67.120.84:30038",
"45.67.123.224:30038",
"45.148.152.114:30038",
"45.148.153.250:30038",
"45.148.153.55:30038",
"45.148.152.153:30038",
"193.176.220.105:30038",
"193.176.220.7:30038",

            });
            Parse(progress, cssSerfer, xPathSerfer, proxies);
        }

        private void Parse(IProgress<string> progress, CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer,
            Proxies proxies)
        {
            string fileName = @"filtered_russian_surnames_ua.txt";
            string surname = GetRandomRowFromTxt(progress, fileName);
            progress.Report($"Поиск: {surname}\r\n");

            ChangeProxyIfLimited(progress, xPathSerfer, proxies); //beginning of surname entering page
            cssSerfer.PasteInFieldByCss("[class^=\"w3-input w3-teal\"]", surname, cssSerfer, 1, OffsetPointScrolled);
            cssSerfer.GoToElementByCss("[class^=\"w3-btn w3-teal\"]", 2, OffsetPoint);
            ChangeProxyIfLimited(progress, xPathSerfer, proxies); //on the new page after clicking "Found"

            CheckData(progress, cssSerfer, xPathSerfer, proxies);
            Parse(progress, cssSerfer, xPathSerfer, proxies);
        }


        private void CheckData(IProgress<string> progress, CssBrowserSerfer cssSerfer, XPathBrowserSerfer xPathSerfer,
            Proxies proxies)
        {
            if (ChangeProxyIfLimited(progress, xPathSerfer, proxies))
                Thread.Sleep(5000);
            var fullNames = cssSerfer.GetElementsByCss("tr > td:nth-of-type(1)", 0);
            var dates = cssSerfer.GetElementsByCss("tr > td:nth-of-type(3)", 0);
            var cities = cssSerfer.GetElementsByCss("tr > td:nth-of-type(4)", 0);
            var streets = cssSerfer.GetElementsByCss("tr > td:nth-of-type(5)", 0);
            var buildings = cssSerfer.GetElementsByCss("tr > td:nth-of-type(6)", 0);



            //Filter rows by Date
            var datesText = dates.Select(x => x.Text).ToList();
            List<int> rowNums = new List<int>() { };
            int i = 0;
            foreach (var dateText in datesText) 
            {
                int year = int.Parse(dateText.Substring(0, 4));
                if (year > 1990)
                    rowNums.Add(i);
                i++;
            }

            //Filter rows by Sex
            int index = 0;
            var tempList = rowNums.ToList();
            rowNums.ForEach(x => 
            {
                var name = fullNames[x].Text.Split(' ')[1];
                var nameEnding = name.Last();
                if (name.Last() == 'А' || name.Last() == 'Я')
                {
                    tempList.RemoveAt(index);
                    index--;
                }
                index++;
            });
            rowNums = tempList;

            //Build Rows
            foreach (int rowNum in rowNums)
            {
                var fullNamesText = fullNames.Select(x => x.Text).ToList();
                var citiesText = cities.Select(x => x.Text).ToList();
                var streetsText = streets.Select(x => x.Text).ToList();
                var buildingsText = buildings.Select(x => x.Text).ToList();
                string row = fullNamesText[rowNum] + " " +
                                datesText[rowNum] + " " +
                                citiesText[rowNum] + " " +
                                streetsText[rowNum] + " " +
                                buildingsText[rowNum] + "\r\n";
                File.AppendAllText(@"filtered_parsed_data_ua.txt", row, Encoding.UTF8);
                progress.Report(row);
            }
            int maxSearchAttemtps = 0;
            try
            {
                OffsetScrollDown(xPathSerfer, fullNames.Count);
                TryToFindNextPageButton(xPathSerfer);
                RefreshOffset();
                CheckData(progress, cssSerfer, xPathSerfer, proxies);
            }
            catch (NoSuchElementException)
            {
                OffsetScrollUp(xPathSerfer, fullNames.Count + maxSearchAttemtps);
                RefreshOffset();
                cssSerfer.NavigateToSite("http://nomer-org.website/allukraina/");              
            }          
        }

        private void TryToFindNextPageButton(XPathBrowserSerfer xPathSerfer)
        {
            var element = xPathSerfer.GetElementByXPath("//*[text()=\"вперед->\"]", 1);
            int yBottomCoordOfElement = element.Location.Y + element.Size.Height + OffsetPointScrolled.Y;
            int windowHeight = Browser.Manage().Window.Size.Height - BrowserNavigationPanelHeight - 70;
            Random random = new Random();
            if (yBottomCoordOfElement >= windowHeight)
            {
                OffsetScrollDown(xPathSerfer, 1);            
                Thread.Sleep(random.Next(350, 450));
                TryToFindNextPageButton(xPathSerfer);
            }
            else
            {
                Thread.Sleep(random.Next(650, 750));
                xPathSerfer.GoToElementByXPath("//*[text()=\"вперед->\"]", 0, OffsetPointScrolled);
            }
        }
    }
}
*/