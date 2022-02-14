using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.Wrappers;
using System;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;


namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class SlotPlayer : AbstractClickerWorker
    {
        protected string bkFolderName;
        protected bool shouldClickWhenLoad = false;
        protected bool shouldOpenNCloseMenu = false;
        protected bool shouldClickNearReuseBtn = false;
        protected bool shouldAutoplay = true;
        protected bool isOpenCloseSameCoords = false;
        public string slotFolderName { get; private set; }
        public string searchOption { get; private set; }

        public SlotPlayer(string bkFolderName, string searchOption, NamedPipeServerStream pipeStream)
        : base(pipeStream)
        {
            this.bkFolderName = bkFolderName;
            this.searchOption = searchOption;
        }
        public void SetSlotName(string slotFolderName) => this.slotFolderName = slotFolderName;
        public void AddSettings(bool shouldClickWhenLoad = false,
            bool shouldOpenNCloseMenu = false, bool shouldClickNearReuseBtn = false, bool shouldAutoplay = true)
        {
            this.shouldClickWhenLoad = shouldClickWhenLoad;
            this.shouldOpenNCloseMenu = shouldOpenNCloseMenu;
            this.shouldClickNearReuseBtn = shouldClickNearReuseBtn;
            this.shouldAutoplay = shouldAutoplay;
        }
        public void AddOpenCloseSameCoords() => isOpenCloseSameCoords = true;

        public bool TryPlaySlotSubStage(int repeatCount = 1)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                //try scroll to top
                if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                    return false;

                if (!TryContinueIfShouldEvent())
                    return false;
                var reuseArea = ClickOnReuseBtn();

                for (int j = 0; j < random.Next(2, 6); j++)
                    if (!TrySimulateFrameSubStage(reuseArea))
                        return false;

            }
            return WaitExitSlotEvent();
        }

        private bool WaitExitSlotEvent()
        {
            SearchArgsWrapper wrap = null;
            switch (random.Next(0, 2))
            {
                case 0:
                    wrap = new SearchArgsWrapper()
                    {
                        path = $@"{bkFolderName}\exit_slot.jpg",
                        cropPercents = new float[] { 70, 0, 100, 50 },
                    };
                    break;
                case 1:
                    wrap = new SearchArgsWrapper()
                    {
                        path = $@"Chrome\previous_page_btn.jpg",
                        panelN = 2
                    };
                    break;
            }
            //LOGGING
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap);
            return area.isAimFound;
        }

        private bool TryContinueIfShouldEvent()
        {
            clicker.MouseMover.SimulateDelay(5);
            if (shouldClickWhenLoad)
            {
                var wrap = new SearchArgsWrapper()
                {
                    path = $@"{bkFolderName}\{slotFolderName}\Continue.jpg",
                    cropPercents = new float[] { 10, 30, 90, 100 },
                    threshhold = 0.7,
                    preDelayPower = 3,
                    maxWaitCount = 26,
                    clickCount = random.Next(2, 4)//2 to 3
                };
                //LOGGING
                var continueArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap);         
                return continueArea.isAimFound;
            }
            return true;
        }

        private bool TrySimulateFrameSubStage(VisionArea reuseArea)
        {
            var randomNum = random.Next(0, 11);
            //(1 of 3)
            if (randomNum % 3 == 0)
                OpenNCloseMenuIfShould(true);
            //(1 of 2)
            if (randomNum % 2 == 0)
                AutoplayIfShould();
            //(1 of 2)
            if (randomNum % 2 == 1)
                ClickFrequentBtn();
            return ReuseSlotEvent(reuseArea);
        }

        private bool ReuseSlotEvent(VisionArea reuseArea)
        {
            bool wereSmthFound = false;
            var area = new VisionArea();
            //from 10 to 30 seconds, around 10 sec per cycle
            for (int i = 0; i < random.Next(1, 4); i++) // 
            {
                if (reuseArea.isAimFound)
                    ClickOffsetAreaIfShould(shouldClickNearReuseBtn, reuseArea);
                area = ClickOnReuseBtn();
                wereSmthFound = area.isAimFound || wereSmthFound;
            }
            //LOGGING
            if (!wereSmthFound)
                MySeriLogger.LogError(area, MethodBase.GetCurrentMethod().Name);
            Thread.Sleep(random.Next(200, 600));
            return wereSmthFound;
        }

        private void AutoplayIfShould()
        {
            if (shouldAutoplay)
            {
                //delay here.
                boxClicker.MouseMover.SimulateDelay(2);
                var wrap = new SearchArgsWrapper()
                {
                    path = $@"{bkFolderName}\{slotFolderName}\autoplay.jpg",
                    threshhold = 0.7,
                };
                var freqArea = imgWatcher.GetArea(true, wrap);
                boxClicker.GoToTitleNLog(true, ref freqArea, 0);
                var delay = random.Next(4000, 7000);
                Thread.Sleep(delay);
                boxClicker.GoToTitleNLog(false, ref freqArea, 0);
            }
        }

        private void ClickFrequentBtn()
        {
            //delay here.
            boxClicker.MouseMover.SimulateDelay(1);
            var randNum = random.Next(1, 5);
            var wrap = new SearchArgsWrapper()
            {
                path = $@"{bkFolderName}\{slotFolderName}\frequent_btn_{randNum}.jpg",
                threshhold = 0.7,
            };
            var freqArea = imgWatcher.GetArea(true, wrap);
            boxClicker.GoToTitleNLog(false, ref freqArea, 0);
        }

        private void OpenNCloseMenuIfShould(bool should)
        {
            if(should)
            {
                //delay here.
                boxClicker.MouseMover.SimulateDelay(2);
                var wrap = new SearchArgsWrapper()
                {
                    path = $@"{bkFolderName}\{slotFolderName}\open_menu.jpg",
                    preDelayPower = 0,
                    threshhold = 0.7,
                    shouldBitmapLive = true
                };
                var openArea = uniClicker.WaitNClick(wrap, 0, 0);             
                if (openArea.isAimFound)
                {
                    if (isOpenCloseSameCoords)
                    {
                        boxClicker.GoToTitleNLog(false, ref openArea, 3);
                    }
                    else
                    {
                        wrap = new SearchArgsWrapper()
                        {
                            path = $@"{bkFolderName}\{slotFolderName}\close_menu.jpg",
                            preDelayPower = 1,
                            threshhold = 0.78
                        };
                        uniClicker.WaitNClick(wrap);
                    }
                }
            }
        }

        private void ClickOffsetAreaIfShould(bool should, VisionArea reuseArea)
        {
            if(should)
            {
                var bound = reuseArea.OriginAimCoords;
                bound.Y -= bound.Height;
                var cropCoords = RectConverter.GetIntArrayFromRectangle(bound);
                var wrap = new SearchArgsWrapper()
                {
                    path = $@"{bkFolderName}\{slotFolderName}\ofsetted_from_reuse_area_1.jpg",
                    originCropCoords = cropCoords,
                    preDelayPower = 0
                };
                var a1 = imgWatcher.IsAimFound(true, wrap);
                if (a1.isAimFound)
                    boxClicker.GoByAimCoords(true, ref a1, 1);
                else
                {
                    wrap.path = $@"{bkFolderName}\{slotFolderName}\ofsetted_from_reuse_area_2.jpg";
                    var a2 = imgWatcher.IsAimFound(false, wrap);
                    if (a2.isAimFound)
                        boxClicker.GoByAimCoords(false, ref a1, 1);
                }
            }
        }
        private VisionArea ClickOnReuseBtn()
        {
            var wrap = new SearchArgsWrapper()
            {
                path = $@"{bkFolderName}\{slotFolderName}\reuse_btn.jpg",
                cropPercents = new float[] { 20, 50, 100, 100 },
                preDelayPower = 0,
                maxWaitCount = 10
            };
            var reuseArea = uniClicker.WaitNClick(wrap, 0, 0);
            return reuseArea;
        }
    }
}
