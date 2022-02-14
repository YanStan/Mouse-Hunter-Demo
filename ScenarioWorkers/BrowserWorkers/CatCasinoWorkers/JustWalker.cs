using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    //с 18:00
    public class JustWalker : AbstractClickerWorker
    {
        public JustWalker(NamedPipeServerStream pipeStream)
            : base(pipeStream) { }

        public bool TryToWalk(int repeatCount)
        {
            bool result = true;
            Initialise();
            for (int i = 0; i < repeatCount; i++)
                //if at least 1 failure, return false
                result = result && TryToWalkRecursive();
            return result;
        }
        private bool TryToWalkRecursive(int tryCounter = 0)
        {
            if (!TryWalkS4() && tryCounter < 3)
                TryToWalkRecursive(tryCounter+1);
            else if (tryCounter >= 3)
                return false;
            return true;
        }

        public bool TryWalkS4()
        {
            //0) Preparing
            //try get to main page if not.
            var aMaker = new SlotActivityMaker(PipeStream);
            aMaker.Initialise(textWatcher, imgWatcher, uniClicker, Baf);
            if(!aMaker.TryGetMainPageS1())
                return false;


            //1)
            //Always Start 1 frame. If not breaking, each frame return to zero page.
            if (!TryWalkFrameS2())
                return false;

            //2)
            //MEDIUM ACTIVITY
            if (!TryMediumS3())
                return false;


            //3) Always Start 2 frame & BACKPAGE
            for (int i = 0; i < 2; i++)
            {
                if (!TryWalkFrameS2())
                    return false;
            }
            //back page
            if (!TryWalkBackPage())
                return false;

            //4
            //Continue from 0 to 7 frames (3 to 10 together with first n third) 
            for (int i = 0; i < random.Next(0, 8); i++)
            {
                if (!TryWalkFrameS2())
                    return false;
            }
            return true;
        }

        private bool TryWalkBackPage()
        {
            if (random.Next(0, 3) == 0)
            {
                var wrap = new SearchArgsWrapper() 
                {
                    panelN = 2,
                    path = $@"Chrome\previous_page_btn.jpg"
                };
                for (int i = 0; i < random.Next(1, 3); i++)//from 1 to 2
                {
                    if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap).isAimFound)
                        return false;
                }
            }
            //2) try return to main page. Should return true and finish stage.
            var aMaker = new SlotActivityMaker(PipeStream);
            aMaker.Initialise(textWatcher, imgWatcher, uniClicker, Baf);
            return aMaker.TryGetMainPageS1();
        }
        private bool TryMediumS3()
        {
            //1)
            //one case of 4
            TryClickOnBannerArrowsS1();

            //one case of 2 //TODO set 0 2
            int ranN2 = random.Next(0, 1);
            if (ranN2 == 0)
                return TryWalkIn3LinesS2();

            //if here, not error. its 1 cases of 2.
            return true;
        }

        private bool TryWalkIn3LinesS2()
        {
            //2.0) Returning to top condition
            if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                return false;

            //2.1) Getting inside 3 lines
            //one case of 4
            var wrap = new SearchArgsWrapper() { path = $@"CatCasino\three_lines.jpg" };
            var area3L = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap,
                random.Next(0, 3));
            if (!area3L.isAimFound)
                return false;
            //Delay and move for activating left window
            clicker.MouseMover.SimulateDelay(random.Next(1, 3));
            clicker.MouseMover.MoveToOffsettedArea(-20, 70, +100, 140);


            //2.2) in 1 chance of 2 try change some appearance
            if (random.Next(0, 2) == 0)
                ChangeColorOrLangEvent();

            //2.3) try walk outside 3lines' menu
            if (!TryWalkFrom3LinesS1())
                return false;

            //2.4) try return to main page. Should return true and finish stage.
            var aMaker = new SlotActivityMaker(PipeStream);
            aMaker.Initialise(textWatcher, imgWatcher, uniClicker, Baf);
            return aMaker.TryGetMainPageS1();
        }

        private bool TryClickOnBannerArrowsS1()
        {
            //TODO set 0, 4
            int ranN1 = random.Next(0, 1);
            if (ranN1 == 0)
            {
                //0) Returning to top condition
                if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                    return false;


                //0.5) //prepare. We should be in right place
                var wrapL = new SearchArgsWrapper(){ text = "Лобби"};
                if (!Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapL, 0).isAimFound)
                    return false;
                boxClicker.MouseMover.SimulateDelay(random.Next(2, 4));//2 to 3


                //1) //first move down to active area cause it can be possible that there would not be scroll
                clicker.MouseMover.MoveToOffsettedArea(-100, 75, 0, 120);
                clicker.ScrollDown(1);
                MySeriLogger.LogText("ScrollDown 1");

                //2)
                var wrap = new SearchArgsWrapper()
                {
                    path = $@"CatCasino\banner_forw_arr.jpg",
                    threshhold = 0.8,
                    clickCount = random.Next(2, 5), //2-4
                    moveWhileDelay = false
                };
                Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1);
                clicker.MouseMover.SimulateDelay(random.Next(0, 3));
                if (random.Next(0, 3) == 0)//one of three
                {
                    var wrapBack = new SearchArgsWrapper()
                    {
                        path = $@"CatCasino\banner_back_arr.jpg",
                        threshhold = 0.8,
                        clickCount = random.Next(1, 3), //1-2
                        moveWhileDelay = false
                    };
                    Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapBack, 0);
                }
                //3)
                clicker.MouseMover.SimulateDelay(random.Next(0, 2));
                clicker.ScrollUp(1);
                MySeriLogger.LogText("ScrollUp 1");
            }
            return true;
        }

        private bool TryWalkFrom3LinesS1()
        {
            //1 Do it!
            string option = new string[] 
            { 
                "Withdrawal", 
                "Полит",
                "Ответ",
                "Чест",

            }[random.Next(0, 4)];
            var wrap = new SearchArgsWrapper() 
            { 
                text = option,
                textContainsInWord = true,
                cropPercents = new float[] { 0, 0, 40, 100 }
            };
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0);
            if (!area.isAimFound)
                return false;
            //2) Deep Scrollreview
            boxClicker.ScrollOverView(5, 25, 2, 10);

            //3) Returning to top condition
            if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                return false;


            return true;
        }

        private void ChangeColorOrLangEvent()
        {
            //1) Scrolling Down
            int scrollNum = random.Next(4, 7);
            clicker.ScrollDown(scrollNum);


            //2) Do it!
            var option = new string[] { "change_lang", "change_color" }[random.Next(0, 2)];
            var wrap = new SearchArgsWrapper()
            {
                path = $@"CatCasino\{option}.jpg",
                moveWhileDelay = false,
                cropPercents = new float[] { 0, 60, 40, 100}
            };
            if (option == "change_color")
                wrap.threshhold = 0.83;
            //TODO make compact way for this option:
            var changeArea = imgWatcher.GetArea(true, wrap);
            if (changeArea.isAimFound)
            {
                boxClicker.GoToTitleNLog(true, ref changeArea, 0);
                Thread.Sleep(random.Next(1300, 3000));
                boxClicker.GoToTitleNLog(false, ref changeArea, 0);
                boxClicker.MouseMover.SimulateDelay(1);
            }
            else
                if(option == "change_lang")//Log but not return
                    MySeriLogger.LogError(changeArea, MethodBase.GetCurrentMethod().Name);



            //3) Scrolling Up same value anyway
            clicker.ScrollUp(scrollNum);
        }

        private bool TryWalkFrameS2()
        {
            //from 1 to 8
            int rNum = random.Next(1, 9);
            //from 1 to 5
            if (rNum < 6)
                if (!TrySimpleWalkS1().isAimFound)
                    return false;
            if (rNum == 6)
                if (!TryAskQuestionS1().isAimFound)
                    return false;
            if (rNum == 7)
                if (!TryHardWalkS1("Все провайдеры").isAimFound)
                    return false;
            if (rNum == 8)
                if (!TryHardWalkS1("Поиск игр").isAimFound)
                    return false;

            return true;
        }

        private VisionArea TrySimpleWalkS1()
        {
            //0) Returning to top condition
            if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                return new VisionArea() { isAimFound = false};

            //1)
            var optionsArr = new string[]
            {
                "Новы", "Покупка бонус", "Мегавей", "Слот", "Наст",
            };
            string option = optionsArr[random.Next(0, 5)];
            var wrap = new SearchArgsWrapper() { text = option, textContainsInWord = true };
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, random.Next(3, 5));
            if (!area.isAimFound)
                return area;

            //2)
            clicker.MouseMover.SimulateDelay(3);
            boxClicker.ScrollDown(1);
            if(random.Next(0, 2) == 0)//1 to 2
            {
                var wrapLoad = new SearchArgsWrapper { path = $@"CatCasino\load_more_games.jpg" };
                Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapLoad, 1);
            }
            clicker.ScrollOverView(0, 16, -5, -5);

            //3)
            if(option == "Новы")
            {
                var wrapLobby = new SearchArgsWrapper() { text = "Лобби"};
                var areaLobby = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapLobby);
                if (!areaLobby.isAimFound)
                    return areaLobby;
                boxClicker.MouseMover.SimulateDelay(random.Next(2, 4));//2 to 3
            }
            return area;
        }

        private VisionArea TryAskQuestionS1()
        {
            //1) open ask area
            var wrap = new SearchArgsWrapper() { path = $@"CatCasino\ask_question.jpg", };
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 4);
            if (!area.isAimFound)
                return area;

            //1) close ask area
            var wrapExit = new SearchArgsWrapper()
            {
                path = $@"CatCasino\exit_ask_question.jpg",
            };
            var exitArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapExit, 2);
            return exitArea;
        }
        private VisionArea TryHardWalkS1(string taskName)
        {
            //0) Returning to top condition
            if (!uniClicker.TryScrollUpToTopEvent().isAimFound)
                return new VisionArea() { isAimFound = false };


            //1)
            var wrap = new SearchArgsWrapper() { text = taskName };
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 4);
            if (!area.isAimFound)
                return area;

            //2)
            var wrapCtg = new SearchArgsWrapper()
            {
                path = $@"CatCasino\all_categories.jpg",
                threshhold = 0.45
            };
            var areaCtg = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapCtg, random.Next(0,3));
            if (!areaCtg.isAimFound)
                return areaCtg;


            //3)
            //choose Rand category
            var wrapR = new SearchArgsWrapper()
            {
                path = $@"CatCasino\rand_category.jpg",
                randOccurance = true,
                threshhold = 0.98
            };
            var rArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapR, random.Next(0,3));
            if (!rArea.isAimFound)
                return rArea;

            //4)
            var wrapExit = new SearchArgsWrapper() { path = $@"CatCasino\close_search_btn.jpg" };
            var exitArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrapExit, 2);
            return exitArea;
        }
    }
}
