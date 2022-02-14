/*using Mouse_Hunter.Clickers;
using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Sikuli4Net.sikuli_UTIL;
using System;
using System.Threading;


namespace Mouse_Hunter.ScenarioWorkers.WhRegistrators
{
    public class WhClickRegistrator1366_768 : AbstractClickerWorker
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
            Clicker.PasteInByAreaCoordsWithEnter(300, 53, 349, 60, "https://games.williamhill.com/ru-ru/", 3);
            //sikuli part
            APILauncher launcher = new APILauncher(true);
            launcher.Start();
            if (!WaitTillForTime("WhGamesPageLoaded", progress, 0.92, 5))
            {
                if (WaitTillForTime("403Error", progress, 0.92, 5) || WaitTillForTime("403Error2", progress, 0.92, 5))
                    progress.Report("WilliamHill Error 403: We are sorry. It looks like you do not have permission" +
                        " to view this page.\r\n");
                else
                    progress.Report("WilliamHill: Cannot find WhGamesPageLoaded on screen.\r\n");
                return false;
            }
            //Registry btn
            Clicker.GoByAreaCoords(1236, 83, 1327, 105, 6);
            WaitTill("WhRegFormLoaded", 0.92);
            //Name
            Clicker.PasteInByAreaCoords(514, 484, 666, 512, profile.AccountData.Name, 5);
            //Surname
            Clicker.PasteInByAreaCoords(686, 484, 840, 512, profile.AccountData.Surname, 1);

            Clicker.ScrollDown(1);

            //Day
            Clicker.PasteInByAreaCoords(513, 465, 606, 490, profile.AccountData.BirthDay, 1);
            //Month
            Clicker.PasteInByAreaCoords(628, 465, 723, 490, profile.AccountData.BirthMonth, 1);
            //Year
            Clicker.PasteInByAreaCoords(746, 465, 840, 490, profile.AccountData.BirthYear, 1);

            Clicker.ScrollDown(1);
            //Email
            Clicker.PasteInByAreaCoords(512, 443, 837, 470, profile.EmailNPaymentLogin, 1);
            //Mobile phone
            Random random = new Random();
            var phoneNum = random.Next(1226842, 9976842);
            Clicker.PasteInByAreaCoords(513, 523, 833, 548, $"+7922{phoneNum}", 1);
            //Expand Address area
            Clicker.GoByAreaCoords(722, 650, 839, 653, 1);
            //Address
            Clicker.PasteInByAreaCoords(513, 603, 839, 627,
                profile.AccountData.Street + " " + profile.AccountData.Building, 1);

            Clicker.ScrollDown(2);

            //City
            Clicker.PasteInByAreaCoords(512, 560, 839, 586, profile.AccountData.City, 1);

            Clicker.ScrollDown(1);

            //Post Index
            Clicker.PasteInByAreaCoords(513, 618, 837, 643, profile.PostIndex, 1);

            Clicker.ScrollDown(2);
            progress.Report("WilliamHill: first registry page passed successfully!\r\n");
            //Continue (next page)
            Clicker.GoByAreaCoords(511, 585, 840, 611, 1);
            Thread.Sleep(4864);
            WaitTill("WhRegFormLoaded", 0.92);
            //Login
            Clicker.PasteInByAreaCoords(514, 182, 838, 204, profile.Login, 4);
            //Password
            Clicker.PasteInByAreaCoords(514, 354, 765, 375, profile.Password, 1);
            //Senseless Click To Hide offered passwords
            Clicker.GoByAreaCoords(314, 254, 414, 475, 1);
            //Secret Answer
            Clicker.GoByAreaCoords(513, 337, 813, 359, 1);
            //My first School
            Clicker.GoByAreaCoords(511, 438, 840, 445, 1);
            //My answer
            Clicker.PasteInByAreaCoords(513, 414, 839, 439, "25", 1);
            //Currency
            Clicker.GoByAreaCoords(512, 493, 839, 515, 1);
            //USD-Dollar
            Clicker.GoByAreaCoords(513, 594, 839, 601, 1);

            Clicker.ScrollDown(2);

            //Choose limit
            Clicker.GoByAreaCoords(515, 423, 811, 447, 1);
            //No limit
            Clicker.GoByAreaCoords(512, 476, 841, 480, 1);
            //Confirm Terms
            Clicker.GoByAreaCoords(511, 534, 518, 542, 1);

            Clicker.ScrollDown(2);
            //-------------------------
            //Finish Registry
            Clicker.GoByAreaCoords(512, 537, 836, 562, 1);
            progress.Report("WilliamHill: profile registry finished !\r\n");
            //Close profile
            Thread.Sleep(random.Next(1581, 5163));
            Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
            //-------------------------
            *//*
                        Pattern ramblerSavePopUp = new Pattern($@"{FullPath}\SavePopUp.PNG");//PLEASE Dont change similarity here.
                        Thread.Sleep(random.Next(2178, 3581));
                        if (Screen.Exists(ramblerSavePopUp))
                            //Screen.Click(ramblerSavePopUp); //(!!!!)
                            Clicker.GoByAreaCoords(972, 373, 1034, 383, 1);


                        //Click Refresh page
                        Clicker.GoByAreaCoords(81, 49, 89, 55, 1);
                        Thread.Sleep(random.Next(3581, 5163));
                        Clicker.ScrollUp(random.Next(3, 5));
                        //Change Lang to Deutch
                        WaitTill("WhGamesPageLoaded", 0.92);
                        Clicker.GoByAreaCoords(1093, 129, 1156, 133, 1);
                        Clicker.GoByAreaCoords(919, 275, 1115, 287, 1);
                        Thread.Sleep(random.Next(3581, 5163));
                        WaitTill("WhGamesPageLoaded", 0.92);
                        //Click Bonus small circle
                        Clicker.GoByAreaCoords(282, 191, 289, 199, 1);
                        //Deposit button
                        Clicker.GoByAreaCoords(1017, 196, 1064, 203, 2);
                        //Make deposit and GET BONUS
                        WaitTill("GetWhBonusLoaded", 0.92);
                        Clicker.GoByAreaCoords(1055, 410, 1290, 420, 2);*/

            /*            Thread.Sleep(random.Next(4500, 15500));
                        //Close profile
                        Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                        ClickOnImg("AnotherProfile", 0.95);
                        Thread.Sleep(random.Next(1500, 2500));*//*



            //Close another profile
            *//*            Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                        Thread.Sleep(random.Next(1500, 2500));*//*
            return true;
        }
    }
}
*/