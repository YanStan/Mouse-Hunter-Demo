/*using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.Clickers;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.ScenarioWorkers.RamblerRegistrators
{
    public class RamblerClickRegistrator1366_768 : AbstractClickerWorker
    {     
        public bool TryExecute(AccountProfile profile, IProgress<string> progress)
        {
            bool shouldContinue = true;
            for (int i = 0; i < 1; i++)
                shouldContinue = TryGoThroughRegistration(profile, progress);
            return shouldContinue;
        }

        private bool TryGoThroughRegistration(AccountProfile profile, IProgress<string> progress)
        {
            //Адресная строка:
            Clicker.PasteInByAreaCoordsWithEnter(256, 47, 431, 63, "https://www.rambler.ru/", 1);
            //Войти в почту:
            WaitTill("RamblerStartPageLoaded", 0.99);
            Clicker.GoByAreaCoords(1171, 148, 1289, 154, 4);
            //Войти в почту-2:
            for(int i = 0; i < 3; i++)
                Clicker.GoByAreaCoords(1171, 148, 1289, 154, 1);
            //Регистрация
            ClickOnImg("RamblerRegBtnLoaded", 0.8);//Clicker.GoByAreaCoords(860, 643, 1055, 677, 4);

            
            bool isRecaptcha2 = IsAvailable("Recaptcha2Available", 0.8, 1145);
            if(isRecaptcha2 || IsAvailable("HCaptchaAvailable", 0.8, 1145))
            {
                //Логин
                string emailLogin = profile.EmailNPaymentLogin.Split('@')[0];
                Clicker.PasteInByAreaCoords(669, 263, 807, 285, emailLogin, 4);
                //Пароль
                Clicker.PasteInByAreaCoords(669, 351, 913, 376, profile.EmailNPaymentPass, 1);
                //Повтор пароля
                Clicker.PasteInByAreaCoords(670, 464, 915, 490, profile.EmailNPaymentPass, 1);

                Clicker.ScrollDown(1);

                //Выбрать вопрос
                Clicker.GoByAreaCoords(668, 456, 916, 478, 1);
                ChooseElementInDropList(2);
                //Ответ на вопрос
                Clicker.PasteInByAreaCoords(672, 545, 959, 572, "Da", 1);
                bool shouldContinue = TryPassCaptchaThroughConsole(progress, isRecaptcha2);
                if(shouldContinue)
                {
                    //F12
                    SendKeys.SendWait("{F12}");
                    //Refresh Browser
                    Clicker.GoByAreaCoords(81, 47, 92, 55, 1);
                    Thread.Sleep(new Random().Next(3178, 3581));
                }
                return shouldContinue;
            }
            else
            {
                progress.Report("Тип капчи неизвестен, или не найден на экране! Программа остановлена.");
                return false;
            }
        }
        private bool TryPassCaptchaThroughConsole(IProgress<string> progress, bool isRecaptcha2)
        {
            Random random = new Random();
            //F12
            SendKeys.SendWait("{F12}");
            Thread.Sleep(random.Next(3178, 3581));
            //click on "Save login and password" popup

            Pattern ramblerSavePopUp = new Pattern($@"{FullPath}\SavePopUp.PNG");//PLEASE Dont change similarity here.
            Thread.Sleep(random.Next(2178, 3581));
            if (Screen.Exists(ramblerSavePopUp))
                Screen.Click(ramblerSavePopUp);
            //Console
            Clicker.GoByAreaCoords(958, 75, 998, 89, 1);
            AttemptToSetInputConsolePosition();

            progress.Report("Проходится капча, ожидайте результата...\r\n");
            if (isRecaptcha2)
                GoThroughRecaptcha2Proxyless(progress, "https://www.rambler.ru/", "6LeHeSkUAAAAANUvgxwQ6HOLXCT6w6jTtuJhpLU7");
            else
                GoThroughHCatchaProxyless(progress, "https://www.rambler.ru/", "322e5e22-3542-4638-b621-fa06db098460");
            //Prepare to Scroll
            Clicker.GoByAreaCoordsNoClick(300, 100, 780, 600, 1);
            Clicker.ScrollDown(2);

            return TryPassCaptchaTillSuccess(progress, isRecaptcha2, 2);
        }

        private bool TryPassCaptchaTillSuccess(IProgress<string> progress, bool isRecaptcha2, int maxcount, int counter = 0)
        {
            if(counter < maxcount)
            {
                //Finish button
                Clicker.GoByAreaCoords(464, 648, 710, 672, 1);
                if (WaitTillForTime("CaptchaDidntPassed", progress, 0.95, 1))
                {
                    progress.Report("К сожалению, сервис прошел капчу на рамблере неправильно!\r\n");
                    progress.Report("Проходится капча, ожидайте результата...\r\n");
                    //input js in console  - set cursor
                    ClickOnImg("ConsoleCurcor", 0.7);
                    if (isRecaptcha2)
                        GoThroughRecaptcha2Proxyless(progress, "https://www.rambler.ru/", "6LeHeSkUAAAAANUvgxwQ6HOLXCT6w6jTtuJhpLU7");
                    else
                        GoThroughHCatchaProxyless(progress, "https://www.rambler.ru/", "322e5e22-3542-4638-b621-fa06db098460");
                    counter++;
                    //If the counter is not exhausted
                    //And what is needed has not been found
                    //return recursion
                    return TryPassCaptchaTillSuccess(progress, isRecaptcha2, maxcount, counter);
                }
                else
                {
                    //If the counter is not exhausted
                    //And what is needed has been found
                    //return success
                    return true;
                }
            }
            else
            {
                //If the counter is exhausted
                //return failure
                return false;
            }
        }

        private void AttemptToSetInputConsolePosition()
        {
            //Top
            Clicker.GoByAreaCoords(886, 106, 996, 117, 1);
            //Mail Registration (droplist)
            ClickOnImg("mail-registration", 0.7);
            //input js in console  - set cursor
            ClickOnImg("ConsoleCurcor", 0.7);
        }
        public string GetSkrillCodeInBuffer(IProgress<string> progress)
        {
            Random random = new Random();
            //Email verification
            Thread.Sleep(random.Next(3178, 3581));
            if(WaitTillForTime("PartOfEmailVerifMsg", progress, 0.92, 3))
            {
                ClickOnImg("PartOfEmailVerifMsg", 0.92);//TODO return bool
            }
            else
            {
                progress.Report("Не пришел код на рамблер!\r\n");
                Thread.Sleep(Timeout.Infinite); 
            }
            //Code
            WaitTill("EmailCodeIsInteractible", 0.92);
            Clicker.GoToByAreaCoordsDoubleClick(509, 404, 543, 408, 2);
            Thread.Sleep(100);
            SendKeys.SendWait("^{c}");
            Thread.Sleep(100);
            string codeFromEmail = Clipboard.GetText();
            return codeFromEmail;
        }
    }
}
*/