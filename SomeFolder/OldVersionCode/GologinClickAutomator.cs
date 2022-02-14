/*using Mouse_Hunter.Clickers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using System.Drawing;
using System.IO;
using Mouse_Hunter.AccountsSheetModels;

namespace Mouse_Hunter.ScenarioWorkers.SystemWorkers
{
    public class GologinClickAutomator : AbstractClickerWorker
    {
        public void Initialise(IProgress<string> progress)
        {
            ClickOnImg("GologinIcon");
            //Розгорнути
            Screen.Wait(new Pattern($@"{FullPath}\GologinMaximizeIcon.PNG"));
            Clicker.GoByAreaCoords(1102, 106, 1130, 122, 2);
            //ClickOnImg("GologinMaximizeIcon", 0.98);
            //Check if Launched
            WaitTill("GologinIsLaunched", 0.92);
            Random random = new Random();
            Thread.Sleep(random.Next(2450, 3550));
            //Bot folder
            Clicker.GoByAreaCoords(221, 252, 283, 272, 2);
            //Name order
            //Clicker.GoByAreaCoords(168, 325, 269, 356, 2);
        }

        public void ClickOnImg(string imgName)
        {
            Random random = new Random();
            Pattern gologinIcon = new Pattern($@"{FullPath}\{imgName}.PNG");
            Screen.Wait(gologinIcon);
            //clicker.GoByAreaCoords(497, 735, 531, 757, 1);
            Screen.Click(gologinIcon);
            Thread.Sleep(random.Next(2800, 3700));
        }

        public bool TryFindProfileName(int profileIndex, string profileName, out Point y1y2,
            ref int skrollDowncount, IProgress<string> progress)
        {
            progress.Report(profileIndex + "\r\n");
            int yOffset = (65 * (profileIndex - 1)) - skrollDowncount * 100;
            y1y2 = new Point()
            {
                X = 387 + yOffset,
                Y = 392 + yOffset
            };
            progress.Report(profileName + "  bottmom Y: " + y1y2.Y + "\r\n");
            CheckHeight(ref y1y2, ref skrollDowncount, progress);
            Clicker.GoToByAreaCoordsDoubleClick(167, y1y2.X, 167, y1y2.Y, 0);
            Thread.Sleep(350);
            SendKeys.SendWait("^{c}");
            Thread.Sleep(350);
            var checkText = Clipboard.GetText();
            var isMatch = profileName == checkText;
            if (isMatch)
                //press "Run"
                Clicker.GoByAreaCoords(1178, y1y2.X, 1215, y1y2.X, 0);
            return isMatch;
        }

        private void CheckHeight(ref Point y1y2, ref int skrollDowncount, IProgress<string> progress)
        {
            while (y1y2.Y > 720)
            {
                Clicker.ScrollDown(1);
                progress.Report("SCROLLDOWN\r\n");
                skrollDowncount++;
                progress.Report($"scrolldowncount: {skrollDowncount}\r\n");
                y1y2.X -= 100;
                y1y2.Y -= 100;
                progress.Report(y1y2.Y + "\r\n");
            }
        }

        public void OpenByProfileName(int profileIndex, string profileName, IEnumerable<AccountProfile> allProfiles,
            IProgress<string> progress)
        {
            int skrollDowncount = 0;
            //Prepare to Scroll
            Clicker.GoByAreaCoordsNoClick(400, 400, 600, 600, 0);
            if (!TryFindProfileName(profileIndex, profileName, out Point y1y2, ref skrollDowncount, progress))
            {
                Clicker.ScrollUp(skrollDowncount);
                skrollDowncount = 0;
                foreach (var profile in allProfiles)
                {
                    var index = int.Parse(profile.AntidetectIndex);
                    //try usual
                    if (TryFindProfileName(index, profileName, out y1y2, ref skrollDowncount, progress))
                        break;
                    //try payment
                    if (TryFindProfileName(index + 1, profileName, out y1y2, ref skrollDowncount, progress))
                        break;
                }
            }
            Clicker.ScrollUp(skrollDowncount);
            Thread.Sleep(350);
            //Check If browser window is launched
            WaitTill(progress, "AddNewTab", 10);
            //Развернуть окно
            Clicker.GoByAreaCoords(1280, 5, 1314, 23, 3);
        }
    }
}
*/