using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.Wrappers;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class CatcasinoRegistrator : AbstractClickerWorker
    {
        private bool isSingleTestMode = true;
        private int superDelayPower;
        private AccountProfile currentProfile;
        public CatcasinoRegistrator(NamedPipeServerStream pipeStream)
            : base(pipeStream) { }

        public void SetCurrentProfile(AccountProfile currentProfile)
        {
            this.currentProfile = currentProfile;
            this.isSingleTestMode = false;
        }

        public bool TryToReg(int repeatCount)
        {
            bool result = true;
            Initialise();
            superDelayPower = random.Next(1, 12);
            MySeriLogger.LogText("Registration delay rate: " + superDelayPower);
            for (int i = 0; i < repeatCount; i++)
                //if at least 1 failure, return false
                result = result && TryToRegRecursive();
            return result;
        }

        private bool TryToRegRecursive(int counter = 0)
        {
            if (!TryRegAccountS3() && counter < 3)
                return TryToRegRecursive(counter + 1);
            else if (counter >= 3)
                return false;
            return true;              
        }


        public bool TryRegAccountS3()
        {
            //0) Preparing
            AccountProfile profile;
            if (isSingleTestMode)
            {
                var allProfiles = LaunchCompositeWorker();
                if (allProfiles == null)
                    return false;
                var activeProfiles = new AccountProfilesAnalyser(allProfiles).GetAllActiveProfiles("Да").ToList();
                profile = activeProfiles[0];
            }
            else
                profile = currentProfile;


            //Clicking on adress bar
            if (!uniClicker.TryPasteInAddressBarEvent("catcasino1.com/ru").isAimFound)
                return false;
            //scrollupshould


            //1)
            clicker.MouseMover.SimulateDelay(superDelayPower);
            if (!TryCatchRegBtnS2())
                return false;


            //2)
            var wrap = new SearchArgsWrapper()
            {
                text = "почта",
                cropPercents = new float[] { 15, 0, 85, 100 },
                textContainsInWord = true,
                shouldBitmapLive = true,
                shouldPressEnter = false
            };
            if(!isSingleTestMode)//TODO del this in production
                wrap.pasteWithBackspace = true;
            var regFieldsArea = Baf.PasteNFinish(MethodBase.GetCurrentMethod().Name, wrap,
                profile.EmailNPaymentLogin, preClickDelay: superDelayPower / 2);
            if (!regFieldsArea.isAimFound)
                return false;


            //3)
            regFieldsArea.AimName = "Пароль";
            boxClicker.PasteInByAreaCoordsNLog(false, wrap, ref regFieldsArea, profile.EmailNPaymentPass,
                 superDelayPower * 2);
            boxClicker.ScrollDown(random.Next(2, 5));
            if (!regFieldsArea.isAimFound)
            {
                MySeriLogger.LogError(regFieldsArea, MethodBase.GetCurrentMethod().Name);
                return false;
            }


            //4)////////
            wrap.text = "АЦИЯ";
            wrap.textContainsInWord = true;
            //TODO DEL this
            if (isSingleTestMode)
                wrap.clickCount = 0;
            wrap.preDelayPower = superDelayPower;
            if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap).isAimFound)
                return false;


            //test mode only
            if(isSingleTestMode)
            {
                uniClicker.TryScrollUpToTopEvent();
                wrap = new SearchArgsWrapper()
                {
                    path = @"CatCasino\close_search_btn.jpg",
                    cropPercents = new float[] { 40, 0, 100, 60 },
                    maxWaitCount = 6,
                    preDelayPower = 0
                };
                if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 2).isAimFound)
                    return false;
            }


            //5)
            wrap = new SearchArgsWrapper()
            {
                path = @"Chrome\save_pass.jpg",
                cropPercents = new float[] { 40, 0, 100, 60 },
                maxWaitCount = 18,
                preDelayPower = 1
            };
            Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, superDelayPower / 2, false);
            //dont return if false. Cause now this button can be not available after successful reg



            if (!isSingleTestMode)
            {
                clicker.MouseMover.SimulateDelay(7);
                wrap = new SearchArgsWrapper()
                {
                    path = @"CatCasino\account.jpg",
                    cropPercents = new float[] { 40, 0, 100, 60 },
                    maxWaitCount = 6,
                    preDelayPower = 1
                };
                if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, 1, false).isAimFound)
                {
                    wrap = new SearchArgsWrapper()
                    {
                        path = @"CatCasino\account_for_max_screen.jpg",
                        cropPercents = new float[] { 40, 0, 100, 60 },
                        maxWaitCount = 6,
                        preDelayPower = 1
                    };
                    if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0, 0).isAimFound)
                        return false;
                }

                wrap = new SearchArgsWrapper()
                {
                    path = @"CatCasino\exit_account.jpg",
                    cropPercents = new float[] { 40, 0, 100, 60 },
                    maxWaitCount = 6,
                    preDelayPower = 0,
                    threshhold = 0.82
                };
                if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, 1).isAimFound)
                    return false;

                wrap = new SearchArgsWrapper() { path = @"CatCasino\confirm_exit.jpg" };
                if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, 1).isAimFound)
                    return false;

            }


            return true;
        }

        private bool TryCatchRegBtnS2()
        {
            //1)
            //searching "Регистрация" button approximate location (has random loc)
            var wrap = new SearchArgsWrapper()
            {
                path = @"CatCasino\reg_btn.jpg",
                cropPercents = new float[] { 0, 25, 30, 60 },
                clickCount = 0,
                shouldMoveToAim = false
            };
            var regArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0, shouldLogError: false);
            if (!regArea.isAimFound)
                return false;

            clicker.MouseMover.SimulateDelay(random.Next(3, 21));

            //2)
            //cant change Point itself so:
            var rC = new RectangleConverter(0);
            var vertices = rC.GetVerticesFromRectangle(regArea.OriginAimCoords);
            int[] apprOriginBtnCoords = new int[]
            {
                 (int)vertices[0].X - 20, (int)vertices[0].Y - 40,
                 (int)vertices[2].X + 20, (int)vertices[2].Y + 40,
            };
            wrap = new SearchArgsWrapper()
            {
                path = @"CatCasino\reg_btn.jpg",
                originCropCoords = apprOriginBtnCoords,
                preDelayPower = 0,
                maxWaitCount = 1,
                threshhold = 0.4
            };
            return TryCatchRegBtnS1(wrap, @"CatCasino\start_adventure.jpg");
        }

        private bool TryCatchRegBtnS1(SearchArgsWrapper wrap, string newTitle, int counter = 0)
        {
            //1)
            //looking for usual title and try to get it
            if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0).isAimFound)
                return false;


            //2)
            ///СМЫСЛ ЭТОГО ВЫЗОВА ИМЕННО В ТОМ ЧТОБЫ ЕСЛИ НЕ НАЙДЕНО, ЗАПУСТИТЬ ВТОРОЙ РАЗ
            ///
            //Seek for new text in new window
            float[] cropPerc = new float[] { 25, 10, 75, 30 };
            wrap = new SearchArgsWrapper() { path = newTitle, cropPercents = cropPerc, maxWaitCount = 4 };
            var area = imgWatcher.WaitForElDeniedClick(wrap, preCycleDelay: 3);
            //If aim element not available, we should try again
            if (!area.isAimFound && counter < 7)
                return TryCatchRegBtnS1(wrap, newTitle, counter+1);
            return true;
        }
    }
}
