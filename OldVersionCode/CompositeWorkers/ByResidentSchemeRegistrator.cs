/*using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.ScenarioWorkers.RamblerRegistrators;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.ScenarioWorkers.WhRegistrators;
using System;
using System.Runtime.InteropServices;

namespace Mouse_Hunter.ScenarioWorkers.CompositeWorkers
{
    public class ByResidentSchemeRegistrator : AbstractClickerWorker
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
                var ramblerReg = new RamblerClickRegistrator1366_768();
                if (ramblerReg.TryExecute(profile, progress))
                {
                    //Open snd window and Register Skrill
                    gologiner.ClickOnImg("GologinIcon");
                    gologiner.OpenByProfileName(profileIndex + 1, profile.ProfileName + profile.PaymentType, allProfiles, progress);
                    var skrillReg = new SkrillClickRegistrator1366_768();
                    if (skrillReg.TryExecute(profile, progress))
                    {
                        //Verify Skrill
                        gologiner.ClickOnImg("AnotherProfile", 0.95);
                        var codeFromEmail = ramblerReg.GetSkrillCodeInBuffer(progress);
                        gologiner.ClickOnImg("AnotherProfile", 0.95);
                        skrillReg.VerifySkrill(codeFromEmail);
                        //Register Williamhill
                        gologiner.ClickOnImg("AnotherProfile", 0.95);
                        WhClickRegistrator1366_768 whReg = new WhClickRegistrator1366_768();
                        whReg.ClickOnImg("AddNewTab", 0.92);
                        if (!whReg.TryExecute(profile, progress))
                        {
                            progress.Report(profile.ProfileName + " не смог пройти регистрацию на Williamhill\r\n");
                            //Close profile
                            Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                        }
                    }
                    else
                    {
                        progress.Report(profile.ProfileName + " не смог пройти капчу на Skrill\r\n");
                        //Close profile
                        Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                    }
                }
                else
                {
                    progress.Report(profile.ProfileName + " не смог пройти капчу на Rambler\r\n");
                    //Close profile
                    Clicker.GoByAreaCoords(1328, 5, 1358, 20, 1);
                }
            }
        }
    }
}
*/