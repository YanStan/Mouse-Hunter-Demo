/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class WilliamHillWalker : AbstractBrowserWorker
    {
        public WilliamHillWalker(IWebDriver browser) : base(browser)
        {
            OffsetPointScrolled = new Point
            {
                X = OffsetPoint.X + 40,
                Y = OffsetPoint.Y + 40
            };
        }

        public override void RefreshOffset() => OffsetPointScrolled.Y = OffsetPoint.Y + 40;

        public override void Execute(IProgress<string> progress)
        {
            Random random = new Random();
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            MouseOperator moveMover = new MouseOperator(1, 3000);
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, moveMover, ww);

            cssSerfer.NavigateToSite("https://games.williamhill.com/ru-ru/action/login");
            progress.Report("Бот зашел на games.williamhill.com.\r\n");
            Thread.Sleep(random.Next(18000, 21000));

            cssSerfer.GoToElementByCss("[class^=\"css-1gnjdoq css-1twshr0 css-1cm9x32 css-tno1xl css-2npark css-1mbcue6\"]", 5, OffsetPoint);
            progress.Report("Войти clicked!\r\n");
            Thread.Sleep(random.Next(5000, 8000));

            //cssSerfer.NavigateToSite(Browser.Url); //refreshing page
            //Thread.Sleep(random.Next(5000, 8000));

            OffsetScrollDown(cssSerfer, 4);
            Thread.Sleep(random.Next(2000, 4000));


            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, moveMover, ww);
            var buttonAreaXpath = $"(//li[@class=\"whgg-tile-grid__element\"])[{random.Next(1, 6)}]";
            xPathSerfer.GoToElementByXPath(buttonAreaXpath, 6, OffsetPointScrolled); //first game
            RefreshOffset();
            progress.Report("Стартовая игра выбрана, ожидание...\r\n");
            Thread.Sleep(random.Next(11000, 20000));

            for(int i  = 1; i < random.Next(13, 23); i++)
            {
                var gameXpath = $"(//div[@class=\"whggc-game-launcher__mini-lobby-list-item\"])[{random.Next(1, 6)}]";
                xPathSerfer.GoToElementByXPath(gameXpath, 6, OffsetPoint); //bottom menu
                Thread.Sleep(random.Next(12000, 21000));
                progress.Report($"{i}: Новая игра выбрана, ожидание...\r\n");
            }
            cssSerfer.GoToElementByCss("[class^=\"whggc-icon__x whgg-game-launcher-action-group__button-icon\"]", 5, OffsetPoint);
        }

        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }
    }
}
*/