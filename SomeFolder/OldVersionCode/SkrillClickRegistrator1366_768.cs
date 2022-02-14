/*using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.Clickers;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.ScenarioWorkers
{
    public class SkrillClickRegistrator1366_768 : AbstractClickerWorker
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
            //Check If browser window is launched
            WaitTill(progress, "AddNewTab", 10);
            Thread.Sleep(5000);
            //Развернуть окно
            Clicker.GoByAreaCoords(1280, 5, 1314, 23, 3);
            Random random = new Random();
            //Адресная строка:
            Clicker.PasteInByAreaCoordsWithEnter(253, 49, 364, 60, "node-ru-6.astroproxy.com:10609/api/changeIP?apiToken=b778b5ef82740105", 1);
            Clicker.PasteInByAreaCoordsWithEnter(453, 49, 464, 60, "https://www.skrill.com/en/", 2);


            //Apply cookis if bar enabled
            Clicker.GoByAreaCoords(1040, 140, 1224, 152, 12);
            //Регистрация
            ClickOnImg("SkrillRegBtn", 0.92);
            Clicker.GoByAreaCoords(1090, 93, 1236, 120, 1);
            for (int i = 0; i < 3; i++)
                Clicker.GoByAreaCoords(1090, 93, 1236, 120, 1);

            //Имя
            WaitCheckByMultipleClickAiming("SkrillRegisterFormLoaded", 0.96, 3);
            //(So big delay cause after window is available fields can bet still not interactible for some time
            Thread.Sleep(random.Next(10000, 12000));
            Clicker.PasteInByAreaCoords(745, 347, 902, 359, profile.AccountData.Name, 1);
            //Фамилия
            Clicker.PasteInByAreaCoords(967, 345, 1113, 360, profile.AccountData.Surname, 1);
            //Currency
            Clicker.GoByAreaCoords(1027, 453, 1112, 473, 1);
            ChooseElementInDropList(30);
            //Email
            Clicker.PasteInByAreaCoords(755, 556, 1103, 569, profile.EmailNPaymentLogin, 1);
            //Password
            Clicker.PasteInByAreaCoords(747, 663, 1055, 668, profile.EmailNPaymentPass, 1);
            bool shouldContinue = TryPassCaptchaThroughConsole(progress);
            if (shouldContinue)
            {
                //F12 Close Inspector
                Thread.Sleep(random.Next(2145, 3451));
                SendKeys.SendWait("{F12}");
                //Pay online
                WaitTill("PayOnlineAvailable", 0.8);
                Clicker.GoByAreaCoords(345, 440, 527, 485, 3);
                //Get started
                ClickOnImg("GetStartedBtn", 0.92);
                //Clicker.GoByAreaCoords(625, 700, 726, 718, 6);
                //Address
                Clicker.PasteInByAreaCoords(479, 461, 647, 496,
                    $"{profile.AccountData.Street} {profile.AccountData.Building}", 5);

                Clicker.ScrollDown(6);
                //City
                Clicker.PasteInByAreaCoords(488, 82, 615, 115, profile.AccountData.City, 1);
                //Index
                Clicker.PasteInByAreaCoords(488, 190, 627, 217, profile.PostIndex, 1);
                //Birthday
                Clicker.PasteInByAreaCoords(480, 296, 606, 326,
                    profile.AccountData.BirthDay + profile.AccountData.BirthMonth + profile.AccountData.BirthYear, 1);
                //Phone Number
                var phoneNum = random.Next(1226842, 9976842);
                Clicker.PasteInByAreaCoords(576, 406, 691, 435, $"99{phoneNum}", 1);
                //Next
                Clicker.GoByAreaCoords(771, 591, 869, 614, 3);

                //Send code to email
                Clicker.GoByAreaCoords(697, 701, 792, 704, 8);
                Thread.Sleep(random.Next(2545, 3456));
            }
            return shouldContinue;
        }

        private bool TryPassCaptchaThroughConsole(IProgress<string> progress)
        {
            //F12
            SendKeys.SendWait("{F12}");
            //Prepare to Scroll
            Clicker.GoByAreaCoords(300, 100, 780, 600, 6);

            Clicker.ScrollDown(8);

            //Register button
            Clicker.GoByAreaCoords(210, 700, 579, 714, 1);
            //Console
            Clicker.GoByAreaCoords(958, 75, 998, 89, 6);
            //input js in console  - set cursor
            Clicker.GoByAreaCoords(838, 701, 949, 709, 7);
            Thread.Sleep(21276);
            if(!IsAvailable("PayOnlineAvailable", 0.8, 1145))
            {
                bool shouldContinue = TryPassCaptchaWithLimit(progress, 2);
                return shouldContinue;
            }
            else
            {
                progress.Report("Skrill не запросил прохождение капчи. Включается второй этап регистрации Skrill.\r\n");
                //return "Should Continue"
                return true;
            }
        }

        private bool TryPassCaptchaWithLimit(IProgress<string> progress, int maxcount, int counter = 0)
        {
            if(counter < maxcount)
            {
                progress.Report("Проходится капча на Skrill, ожидайте результата...\r\n");
                if (!GoThroughRecaptcha2Proxyless(progress, "https://account.skrill.com/wallet/account/sign-up?locale=en", "6LcGyH4UAAAAAMz26I7kwBSvb8cW9T61ezhoq4ku")
                    || IsAvailable("SkrillCaptchaWasNotPassed", 0.85, 14145))
                {
                    counter++;
                    progress.Report("К сожалению, капча на Skrill пройдена неправильно.\r\n");
                    //If the counter is not exhausted
                    //And what is needed has not been found
                    //return recursion
                    return TryPassCaptchaWithLimit(progress, maxcount, counter);
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
        public void VerifySkrill(string codeFromEmail)
        {
            //Insert code
            Random random = new Random();
            Clicker.GoByAreaCoords(515, 459, 634, 473, 2);
            Thread.Sleep(100);
            Clipboard.SetData(DataFormats.Text, codeFromEmail);
            SendKeys.SendWait("^{v}");
            Thread.Sleep(random.Next(450, 550));

            //Click Verify Button
            Clicker.GoByAreaCoords(626, 596, 721, 615, 2);
            WaitTill("SkrillIsVerified", 0.92);
            Thread.Sleep(random.Next(5500, 6500));
            Clicker.SendWaitAsHumanTyping("250396");
            Clicker.ScrollDown(1);
            //Click Save button
            Clicker.GoByAreaCoords(625, 681, 715, 702, 2);
        }
    }
}*/