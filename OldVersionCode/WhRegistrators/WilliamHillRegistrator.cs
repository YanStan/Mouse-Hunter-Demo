/*using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Mouse_Hunter.BrowserSerfers;
using Mouse_Hunter.MovementSimulators;
using System;
using System.Drawing;
using System.Threading;

namespace Mouse_Hunter.ScenarioWorkers.WhRegistrators
{
    public class WilliamHillRegistrator : AbstractBrowserWorker
    {
        public WilliamHillRegistrator(IWebDriver browser) : base(browser)
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


    public override void Execute(IProgress<string> progress)
        {
            Random random = new Random();
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(100));
            var mouseMover = new MouseOperator(1, 3000);
            CssBrowserSerfer cssSerfer = new CssBrowserSerfer(Browser, mouseMover, ww);
            XPathBrowserSerfer xPathSerfer = new XPathBrowserSerfer(Browser, mouseMover, ww);

            cssSerfer.NavigateToSite("https://games.williamhill.com/ru-ru/");
            progress.Report("Регистрация: Бот зашел на games.williamhill.com.\r\n");
            //for (int i = 0; i < 100; i++)
                cssSerfer.GoToElementByCss("[class^=\"sb-button sb-o-button sb-o-button--highlight sb-o-button--small action-registration__button\"]", 3, OffsetPoint);
            progress.Report("Зарегистрироваться clicked!\r\n");
            //progress.Report(Browser.Manage().Window.Size + "\r\n");

            AdaptOffsetToIframe(xPathSerfer);

            cssSerfer.PasteInFieldByCss("#reg-firstName", "Sana", cssSerfer, 1, OffsetPointScrolled);
            progress.Report("Введено имя\r\n");
            OffsetScrollDown(xPathSerfer, random.Next(0, 1));
            cssSerfer.PasteInFieldByCss("#reg-lastName", "Beauty", cssSerfer, 1, OffsetPointScrolled);
            progress.Report("Введена фамилия\r\n");
            OffsetScrollDown(xPathSerfer, 2);

            cssSerfer.PasteInFieldByCss("#reg-dobDay", "12", cssSerfer, 1, OffsetPointScrolled);
            xPathSerfer.PasteInFieldByXPath("//*[@name='dobMonth']", "05", xPathSerfer, OffsetPointScrolled);
            xPathSerfer.PasteInFieldByXPath("//*[@name='dobYear']", "1991", xPathSerfer, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, random.Next(0, 1));
            progress.Report("Введена дата рождения\r\n");


            cssSerfer.PasteInFieldByCss("#reg-email", "SanaSana@gmail.com", cssSerfer, 1, OffsetPointScrolled);
            cssSerfer.PasteInFieldByCss("#reg-mobile", "+380956748741", cssSerfer, 1, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 2);
            OffsetPointScrolled.Y += 95;
            *//*
                        //i cant explain why it is nessessary
                        OffsetPointScrolled.Y += 110; //i cant explain why it is nessessary
                        OffsetPointScrolled.Y -= 60; //i cant explain why it is nessessary
                        OffsetPointScrolled.Y -= 130; //i cant explain why it is nessessary*//*

            //OffsetPointScrolled.Y += 145; //i cant explain why it is nessessary


            //cssSerfer.ChangeMouseSpeed(0.01);
            cssSerfer.GoToElementByCssWithProgress(progress, "[class^=\"sb-link sb-link--anchor sb-link--bare cs-component-address__expand-link\"]", 1, OffsetPointScrolled);

            OffsetScrollDown(xPathSerfer, random.Next(0, 1));
            progress.Report("Введены email и номер телефона\r\n");

            cssSerfer.PasteInFieldByCss("#reg-street1", "Ptushkina 1", cssSerfer, 3, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 2);
            cssSerfer.PasteInFieldByCss("#reg-city", "Kiev", cssSerfer, 1, OffsetPointScrolled);
            cssSerfer.PasteInFieldByCss("#reg-postcode", "05140", cssSerfer, 1, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 2);

            OffsetPointScrolled.Y += 20;
            

            cssSerfer.GoToElementByCss("[class^=\"sb-button sb-o-button sb-o-button--full-width sb-o-button--primary sb-o-button--normal cs-reg-form-submit\"]", 3, OffsetPointScrolled);
            progress.Report("Введен адрес. Первая страница пройдена.\r\n");
            OffsetPointScrolled.Y -= 20; //i cant explain why it is nessessary

            RefreshOffset();
            AdaptOffsetToIframe(xPathSerfer);
            OffsetPointScrolled.Y -= 154;

            cssSerfer.PasteInFieldByCss("#reg-username", "SaniOnRails", cssSerfer, 1, OffsetPointScrolled);
            cssSerfer.PasteInFieldByCss("#reg-password", "A4y3PFh!H99b3eq", cssSerfer, 1, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 1);
            progress.Report("Введены логин и пароль.\r\n");

            cssSerfer.ChangeMouseSpeed(0.01);

            cssSerfer.GoToElementByCssWithProgress(progress, "#reg-challenge", 1, OffsetPointScrolled);
            cssSerfer.SelectDropListByCssNoClick("#reg-challenge", 4, 2, OffsetPointScrolled);
*//*            for (int i = 0; i < 30; i++)
            {
                cssSerfer.SelectDropListByCssNoClick("#reg-challenge", -4, 2, OffsetPointScrolled);
                cssSerfer.GoToElementByCss("#reg-password", 1, OffsetPointScrolled);
                cssSerfer.GoToElementByCssWithProgress(progress, "#reg-challenge", 1, OffsetPointScrolled);
                cssSerfer.SelectDropListByCssNoClick("#reg-challenge", 4, 2, OffsetPointScrolled);
            }*//*

            cssSerfer.ChangeMouseSpeed(3);

            OffsetScrollDown(xPathSerfer, 1);
            cssSerfer.PasteInFieldByCss("#reg-response", "28", cssSerfer, 1, OffsetPointScrolled);
            cssSerfer.SelectDropListByCss("#reg-currencyCode", 3, 2, OffsetPointScrolled);
            OffsetScrollDown(xPathSerfer, 2);
            cssSerfer.SelectDropListByCss("#reg-depositLimit", 1, 2, OffsetPointScrolled);
            progress.Report("Все поля заполнены.\r\n");

            xPathSerfer.GoToElementByXPath("(//div[@class=\"sb-choice__checkbox\"])[1]", 2, OffsetPointScrolled);
            OffsetPointScrolled.Y += 10;
            //for (int i  = 0; i < 100; i++)
                cssSerfer.GoToElementByCssNoClick("#reg-submit", 1, OffsetPointScrolled);
            progress.Report("Регистрация завершена!\r\n");

            RefreshOffset();
            Execute(progress);

        }

        private void AdaptOffsetToIframe(XPathBrowserSerfer xPathSerfer)
        {
            Browser.SwitchTo().DefaultContent();
            Random random = new Random();
            Thread.Sleep(random.Next(3000, 5000));
            var frame = xPathSerfer.GetElementByXPath("//iframe[@name='cp-registration-frame']", 4);
            int windowWidth = Browser.Manage().Window.Size.Width;
            int frameWidth = frame.Size.Width;
            int frameScrollColumn = 18;
            int registryFrameOffsetX = (int)Math.Round((double)((windowWidth - frameScrollColumn / 2 - frameWidth) / 2));
            int registryFrameOffsetY = 40;
            OffsetPointScrolled.X += registryFrameOffsetX;
            OffsetPointScrolled.Y += registryFrameOffsetY;
            Browser.SwitchTo().Frame("cp-registration-frame");
        }

        protected override bool IsConnectionLimited(XPathBrowserSerfer xPathSerfer)
        {
            throw new NotImplementedException();
        }
    }
}
*/