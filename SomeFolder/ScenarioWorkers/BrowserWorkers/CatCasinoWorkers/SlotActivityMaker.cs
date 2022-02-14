using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.Wrappers;
using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class SlotActivityMaker : AbstractClickerWorker
    {
        public SlotActivityMaker(NamedPipeServerStream pipeStream)
            : base(pipeStream) { }

        public bool TryToWalk(int repeatCount)
        {
            bool result = true;
            Initialise();
            for (int i = 0; i < repeatCount; i++)
                //It means 1-3 successful slot playings
                for (int j = 0; j < random.Next(1, 4); j++)
                    //if at least 1 failure, return false
                    result = result && TryToWalkRecursive();
            return result;
        }

        private bool TryToWalkRecursive(int tryCounter = 0)
        {
            if (!TryWalkOnSlotsStage() && tryCounter < 3)
                return TryToWalkRecursive(tryCounter+1);
            else if (tryCounter >= 3)
                return false;
            return true;
        }

        private bool TryWalkOnSlotsStage()
        {
            //1.1
            if (!TryGetMainPageS1())
                return false;

            string searchOption = GetRandSearchOption();
            //1.2
            var slotPlayer = TrySearchSlotSubStage(searchOption);
            if (slotPlayer == null)
                return false;

            //1.3
            return MySeriLogger.LogTime(slotPlayer.TryPlaySlotSubStage,
                $"Время, потраченное на игру в слот {slotPlayer.slotFolderName}: ");
        }

        public bool TryGetMainPageS1()
        {
            if (!TryGetMainPageSubStage())
            {
                var area = uniClicker.TryPasteInAddressBarEvent("catcasino1.com/ru");
                if (!area.isAimFound)
                    return false;
                clicker.MouseMover.SimulateDelay(7);
                if(!uniClicker.TryScrollUpToTopEvent().isAimFound)
                    return false;
            }
            return true;
        }

        private bool TryGetMainPageSubStage()
        {
            var coords = new int[] { 20, 15, 100, 30 };
            clicker.GoByAreaCoords(coords, 0, 1);
            uniClicker.TryScrollUpToTopEvent();//+
            //checking if we already on start page after scrolling
            var wrap = new SearchArgsWrapper()
            {
                text = "catcasino1.com/ru",
                panelN = 2,
                preDelayPower = 0
            };
            if(!textWatcher.IsAimFound(false, wrap).isAimFound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        if (!WaitForUpperGUIEvent("three_lines"))//
                            return false;
                        if (!WaitForLiveOrCasinoEvent())//
                            return false;
                        break;
                    case 1:
                        if (!WaitForUpperGUIEvent("catcas_main_icon"))//
                            return false;
                        break;
                }
            }
            return true;
        }
        private bool WaitForUpperGUIEvent(string btnTitle)
        {
            var wrap = new SearchArgsWrapper()
            {
                path = $@"CatCasino\{btnTitle}.jpg",
                cropPercents = new float[] { 0, 0, 80, 40 },
                preDelayPower = random.Next(1, 3)
            };
            //LOGGING
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap);
            area.AreaBitmap.Dispose();
            return area.isAimFound;
        }

        private SlotPlayer TrySearchSlotSubStage(string searchOption)
        {
            //Search by strategy and slot
            var sS = new SlotSearcher(PipeStream);
            sS.Initialise(textWatcher, imgWatcher, uniClicker);
            return sS.TrySearchSubStage(searchOption);
        }

       
        private bool WaitForLiveOrCasinoEvent()
        {
            var btnNamesArr = new string[] { "Casino_btn", "Live_Casino_btn" };
            string btnName = btnNamesArr[random.Next(0, 2)];
            var wrap = new SearchArgsWrapper()
            {
                path = $@"CatCasino\{btnName}.jpg",
                cropPercents = new float[] { 0, 0, 25, 90 },
                preDelayPower = random.Next(2, 4)
            };
            //LOGGING
            var casinoArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap);
            if (!casinoArea.isAimFound)
                return false;

            var wrapL = new SearchArgsWrapper() { text = "Лобби" };
            if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapL, 0, 7).isAimFound)
                return false;

            boxClicker.MouseMover.SimulateDelay(random.Next(2, 4));//2 to 3
            return true;
        }

        private string GetRandSearchOption()
        {
            var optionsArr = new string[] { "Popular", "Lobby", "Search" };
            return optionsArr[random.Next(0, 3)];
        }

    }
}
