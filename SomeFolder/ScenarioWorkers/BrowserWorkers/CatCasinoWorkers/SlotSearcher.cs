using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using Mouse_Hunter.Wrappers;
using System;
using System.IO.Pipes;
using System.Reflection;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class SlotSearcher : AbstractClickerWorker
    {
        public SlotSearcher(NamedPipeServerStream pipeStream)
                : base(pipeStream) { }
        public SlotPlayer TrySearchSubStage(string searchOption)
        {
            //SlotPlayer selection is here because when at least nothing is found in the Lobby
            //We should give to Lobby method another SlotPlayer.
            //So we:
            /*          1) pass search option here
                        2) here we  generate which slot we will look for (SlotChooser.GetRandSlotName())
                        3) find slot
                        4) create a player by slotname (SlotChooser.FormPlayerBySlotName)
                        5) send the player out.
                        6) click on slot
                        7) if slot not found, return null. Check it outside!
                            */
            var sC = new SlotСhooser(PipeStream);
            sC.Initialise(textWatcher, imgWatcher, uniClicker);
            if (searchOption == "Lobby")
            {
                MySeriLogger.LogText($"Выбран запуск слотов: с главной страницы");

                if (!WaitForBtnIfNotStartPageEvent("Лобби").isAimFound)//+
                    return null;
                int slotsCount = sC.CountAllSlots();
                boxClicker.ScrollOverView(2, 7, 2, 2);
                return TryLobbyRecursiveSubStage(sC, slotsCount);
            }
            if (searchOption == "Popular")
            {
                MySeriLogger.LogText($"Выбран запуск слотов: с вкладки \"Популярные\"");
                return TryPopularSubStage(sC);
            }
            if (searchOption == "Search")
            {
                MySeriLogger.LogText($"Выбран запуск слотов: поисковый запрос");
                var player =  TrySearchSubStage(sC);
                if (player == null)
                    FinishSearch();
                return player;
            }        
            return null;
        }

        private void FinishSearch()
        {
            var wrap = new SearchArgsWrapper()
            {
                path = @"CatCasino\close_search_btn.jpg",
                cropPercents = new float[] { 60, 0, 100, 20 },
                threshhold = 0.7,
                preDelayPower = 3
            };
            var area = uniClicker.WaitNClick(wrap);
            if (!area.isAimFound)
                uniClicker.TryPasteInAddressBarEvent("catcasino1.com/ru");
        }

        private SlotPlayer TryPopularSubStage(SlotСhooser sC)
        {
            if(!WaitForBtnEvent("Популярные").isAimFound)
                return null;
            var slotFolderName = sC.GetRandSlotName();
            var slotImagePath = $@"CatCasino\{slotFolderName}\title.jpg";

            //cause in next step zero preDelays
            clicker.MouseMover.SimulateDelay(1);
            var area = TrySearchSlotRecursiveSubStage(slotImagePath); //
            if (area.isAimFound)
                return sC.FormPlayerBySlotName(slotFolderName);
            return null;
        }
        private SlotPlayer TryLobbyRecursiveSubStage(SlotСhooser sC, int slotsCount, int counter = 0)
        {
            //Getting rand slot
            var slotFolderName = sC.GetRandSlotName();
            var slotImagePath = $@"CatCasino\{slotFolderName}\title.jpg";
            var wrap = new SearchArgsWrapper() 
            {
                path = slotImagePath,
                preDelayPower = 0,
                clickCount = 0,
                maxWaitCount = 2
            };
            //try to search
            //NOT log if error
            var area = uniClicker.WaitNClick(wrap, 0);
            if (!area.isAimFound && counter < slotsCount)
                //search another slot
                return TryLobbyRecursiveSubStage(sC, slotsCount, counter);
            else
            {
                if (area.isAimFound)
                {
                    if (!WaitForDemoSlotEvent(area))
                        return null;
                    return sC.FormPlayerBySlotName(slotFolderName);
                }
            }                
            return null;
        }
        private SlotPlayer TrySearchSubStage(SlotСhooser sC)
        {
            var slotFolderName = sC.GetRandSlotName();
            var slotImagePath = $@"CatCasino\{slotFolderName}\title.jpg";

            var area = TrySearchBoxSubStage(slotFolderName);
            if (!area.isAimFound)
            {
                return null;
            }
            else
            {
                var slotArea = WaitForSlotTitleEvent(slotImagePath);
                if (!slotArea.isAimFound)//
                    return null;
                if (!WaitForDemoSlotEvent(slotArea))//
                    return null;
                return sC.FormPlayerBySlotName(slotFolderName);
            }        
        }

        private VisionArea TrySearchBoxSubStage(string slotFolderName)
        {
            var area = WaitForBtnEvent("Лобби");
            boxClicker.ScrollUp(2);
            if (!area.isAimFound)
                return area;

            return TryGetGamesSearchSubStage(slotFolderName);
        }
        public VisionArea WaitForSlotTitleEvent(string slotImagePath)
        {
            var wrap = new SearchArgsWrapper() { 
                path = slotImagePath,
                preDelayPower = 5,
                moveWhileDelay = false,
                maxWaitCount = 3,
                clickCount = 0
            };
            return Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap);
        }
        private VisionArea TryGetGamesSearchSubStage(string slotFolderName)
        {
            var wrap = new SearchArgsWrapper()
            {
                text = "Поиск игр",
                preDelayPower = 3,
                textContainsInWord = false
            };
            //LOGGING
            var searchArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0);
            if (!searchArea.isAimFound)
                return searchArea;
          
            
            wrap = new SearchArgsWrapper()
            {
                text = "Поиск игр",
                cropPercents = new float[] { 0, 0, 50, 40 },
                preDelayPower = 5,
                textContainsInWord = false
            };
            string input = slotFolderName.Replace("_", " ");
            searchArea = Baf.PasteNFinish(MethodBase.GetCurrentMethod().Name, wrap,
                input);
            return searchArea;
        }
        private VisionArea WaitForBtnIfNotStartPageEvent(string btnName)
        {
            //checking if we already on start page after scrolling
            var wrap = new SearchArgsWrapper()
            {
                text = "catcasino1.com/ru",
                panelN = 2,
                preDelayPower = 0
            };
            var addBarArea = textWatcher.IsAimFound(false, wrap);
            if (!addBarArea.isAimFound)
                return WaitForBtnEvent(btnName);
            return addBarArea;
        }
        private VisionArea WaitForBtnEvent(string btnName)
        {
            var wrap = new SearchArgsWrapper()
            {
                text = btnName,
                preDelayPower = 2,
                textContainsInWord = false
            };
            var popularArea = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, preCycleDelay: 4);
            boxClicker.ScrollOverView(2, 7, 2, 2);
            return popularArea;
        }

        private bool WaitForDemoSlotEvent(VisionArea slotArea)
        {
            var cropCoords = RectConverter.GetIntArrayFromRectangle(slotArea.OriginAimCoords);
            //Waiting For
            var wrap = new SearchArgsWrapper()
            {
                text = "ЕМ",
                textContainsInWord = true,
                originCropCoords = cropCoords,
                preDelayPower = 1,
                moveWhileDelay = false,
            };
            return Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 0).isAimFound;
        }


        private VisionArea TrySearchSlotRecursiveSubStage(string slotImagePath, int skrollcount = 0)
        {
            var wrap = new SearchArgsWrapper
            {
                path = slotImagePath,
                preDelayPower = 0,
                cropPercents = new float[] { 0, 0, 100, 75},
                maxWaitCount = 2,
                clickCount = 0
            };
            var area = Baf.Finish(MethodBase.GetCurrentMethod().Name, wrap, 1, shouldLogError:false);
            if (!area.isAimFound)
            {
                skrollcount++;
                var moreGamesArea = TryLoadMoreSlotsSubEvent(skrollcount);
                if (!moreGamesArea.isAimFound)
                    return moreGamesArea;
                return TrySearchSlotRecursiveSubStage(slotImagePath, skrollcount);
            }
            else
            {
                if (!WaitForDemoSlotEvent(area))
                {
                    //must return this false result inside area as inside wrapper
                    //Exiting
                    area.isAimFound = false;
                }
                return area;
            }           
        }
        private VisionArea TryLoadMoreSlotsSubEvent(int skrollcount)
        {
            var wrap = new SearchArgsWrapper
            { 
                path = $@"CatCasino\load_more_games.jpg",
                preDelayPower = 0
            };
            var loadMoreArea = imgWatcher.IsAimFound(true, wrap);
            if (loadMoreArea.isAimFound)
            {
                boxClicker.GoToTitleNLog(false, ref loadMoreArea, 0);
                return loadMoreArea;
            }
            else
            {
                if (skrollcount < 5)
                {
                    MySeriLogger.LogError(loadMoreArea, MethodBase.GetCurrentMethod().Name);
                    return loadMoreArea;
                }
                else//Just exiting with no error
                {
                    MySeriLogger.LogInfo(loadMoreArea);
                    if(!uniClicker.TryScrollUpToTopEvent().isAimFound)
                        loadMoreArea.isAimFound = false;//use area as wrapper
                    return loadMoreArea;
                }
            }
        }
    }
}
