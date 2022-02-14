/*using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.ScenarioWorkers.BrowserWorkers;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.ScenarioWorkers.WhRegistrators;
using System;
using System.Runtime.InteropServices;

namespace Mouse_Hunter.ScenarioWorkers.CompositeWorkers
{
    public class ByGmailNPayRotationRegistrator : AbstractClickerWorker
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);
        public void Execute(IProgress<string> progress)
        {
            var allProfiles = LaunchCompositeWorker(progress);
            GologinClickAutomator gologiner = new GologinClickAutomator();
            gologiner.Initialise(progress);
            var activeProfiles = new AccountProfilesAnalyser(allProfiles).GetAllActiveProfiles("Да");

            //found my profile
            foreach (var profile in activeProfiles)
            {
                //Open fst window and Register Rambler
                int profileIndex = int.Parse(profile.AntidetectIndex);
                gologiner.OpenByProfileName(profileIndex, profile.ProfileName, allProfiles, progress);
                GmailClickVisitor1366_768 gmailReg = new GmailClickVisitor1366_768();
                if (gmailReg.TryExecute(profile, progress))
                {
                    //Register Williamhill
                    WhClickRegistrator1366_768 whReg = new WhClickRegistrator1366_768();
                    ClickOnImg("AddNewTab", 0.92);
                    if (!whReg.TryExecute(profile, progress))
                    {
                        progress.Report(profile.ProfileName + " не смог пройти регистрацию на Williamhill\r\n");
                        //Close profile
                        Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                    }
                }
                else
                {
                    progress.Report(profile.ProfileName + " не смог зайти в Gmail-почту\r\n");
                    //Close profile
                    Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                }
            }
        }
    }
}
*/