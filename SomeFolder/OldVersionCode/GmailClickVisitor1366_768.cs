/*using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Sikuli4Net.sikuli_UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers
{
    public class GmailClickVisitor1366_768 : AbstractClickerWorker
    {
        public bool TryExecute(AccountProfile profile, IProgress<string> progress)
        {
            bool shouldContinue = true;
            for (int i = 0; i < 1; i++)
                shouldContinue = TryGoThroughVisit(profile, progress);
            return shouldContinue;
        }
        private bool TryGoThroughVisit(AccountProfile profile, IProgress<string> progress)
        {
            //sikuli part
            APILauncher launcher = new APILauncher(true);
            launcher.Start();
            *//*//New tab
            Clicker.GoByAreaCoords(264, 11, 274, 19, 2);
            //Post
            WaitTill(@"Gmail\GmailPostLoaded", 0.92);
            Clicker.GoByAreaCoords(1196, 102, 1222, 105, 2);            
            //Enter
            if(WaitTillForTime(@"Gmail\GmailEnterPageLoaded2", progress, 0.92, 1))
                Clicker.GoByAreaCoords(1000, 89, 1071, 116, 2);
            //Email
            WaitTill(@"Gmail\GmailEmailPageLoaded", 0.92);
            Clicker.PasteInByAreaCoordsWithEnter(515, 354, 855, 377, profile.EmailNPaymentLogin, 2);
                    *//*            //Next
                    Clicker.GoByAreaCoords(785, 545, 856, 562, 2);*//*
            //Password
            WaitTill(@"Gmail\GmailPassPageLoaded", 0.92);
            Clicker.PasteInByAreaCoordsWithEnter(515, 377, 641, 401, profile.EmailNPaymentPass, 2);
                    *//*            //Next
                    Clicker.GoByAreaCoords(784, 499, 860, 519, 2);*//*
            Thread.Sleep(new Random().Next(9864, 11945));
            //Confirm
            if (WaitTillForTime(@"Gmail\GmailConfirmLaunched", progress, 0.92, 1))     
                Clicker.GoByAreaCoords(736, 676, 828, 689, 2);
            progress.Report($"Бот зашел в gmail-ящик {profile.EmailNPaymentLogin}\r\n");*//*
            return true;
        }
    }
}
*/