/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.Resources.AntiCaptcha;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Drawing;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class HappyMinRegistrator : AbstractBrowserWorker
    {
        public HappyMinRegistrator(IWebDriver browser) : base(browser) { }

        public override void RefreshOffset() => OffsetPointScrolled.Y = OffsetPoint.Y;

        public override void Execute(IProgress<string> progress)
        {
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            //TODO if want change search (1, 3000) everywhere
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, new MouseOperator(1, 3000), ww);

            cssSerfer.NavigateToSite("https://happyministry.com.ua/");
            progress.Report("Бот перешел на сайт Министерства.\r\n");

            cssSerfer.GoToElementByLinkText("Каталог", 6);
            progress.Report("Бот перешел в каталог.\r\n");

            var elementName = cssSerfer.GoToRandElInCssStruct("[class^=objects][class~=justify][class$=loaded] a", 6, OffsetPoint);
            progress.Report($"Случайно выбран элемент каталога: {elementName}\r\n");

            elementName = cssSerfer.GoToRandElInCssStruct("[class^=owl-item][class$=active] a", 5, OffsetPoint);
            progress.Report($"Случайно выбран элемент каталога: {elementName}\r\n");

            elementName = cssSerfer.GoToRandElInCssStruct("[class^=owl-item][class$=active] a", 5, OffsetPoint);
            progress.Report($"Случайно выбран элемент каталога: {elementName}\r\n");

            elementName = cssSerfer.GoToRandElInCssStruct("[class^=owl-item][class$=active] a", 6, OffsetPoint);
            progress.Report($"Случайно выбран элемент каталога: {elementName}\r\n");

            cssSerfer.GoToElementByLinkText("Заказать", 8);
            progress.Report("Бот нажал Заказать\r\n");

            cssSerfer.AcceptAlert(1);
            cssSerfer.GoToElementByLinkText("Оформить заказ", 2);
            progress.Report("Бот нажал Оформить заказ\r\n");

            cssSerfer.SendKeysToCssElement("[class^=inputbox][class$=name]", "Программа Яна", 1);
            cssSerfer.SendKeysToCssElement("[class^=inputbox][class~=phone][class$=phone_transformer]",
                "999999999", 3);
            //cssSerfer.SelectDropListByCss("[class^=inputbox][class$=shop_delivery_ident]", "Курьером", 2, OffsetPointScrolled);
            //cssSerfer.SelectDropListByCss("[class^=inputbox][class$=shop_delivery_zone_ident]", "Киев", 2, OffsetPointScrolled);
            cssSerfer.SendKeysToCssElement("[class^=inputbox][class$=address]",
                "ул. Тайный Адрес кв. -1", 2);
            //cssSerfer.SelectDropListByCss("[class^=inputbox][class$=shop_payment_ident]", "Наличный расчет", 2, OffsetPointScrolled);
            cssSerfer.SendKeysToCssElement("[class^=inputbox][class$=comment]", "Хочу любви и ласки, запакуйте вместе" +
                "с красным конвертом. Спасибо!", 2);
            progress.Report("Данные введены.");

            CaptchaSender sender = new CaptchaSender();
            progress.Report("Проходится капча, ожидайте результата...");

            string token = sender.DoRecaptcha2Proxyless("https://happyministry.com.ua/",
                "6LcCHBYUAAAAAPkUMxWuZJWYJp0tsHCn5EaZbOKH");
            if (token != null)
            {
                cssSerfer.SendTokenToCaptcha(token, "g-recaptcha-response", 0);
                cssSerfer.GoToElementByCss("#comShopOrderAdminPagerForm_1_submitButton", 0, OffsetPoint);
                progress.Report("Заказ создан!");
            }
            else
            {
                progress.Report("Сервису Антикапча не удалось пройти прокси!");
            }
        }

        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }
    }
}
*/