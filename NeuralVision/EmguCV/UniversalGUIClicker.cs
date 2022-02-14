using Mouse_Hunter.Clickers;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision.GoogleOCR;
using Mouse_Hunter.Wrappers;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision.EmguCV
{
    public class UniversalGUIClicker : EmguWatcher
    {
        public static MouseOperator mover;
        public static BoundingBoxClicker clicker;
        public static TextWatcher textWatcher;
        public UniversalGUIClicker(bool isBrowserMode = true)
        : base(isBrowserMode)
        {
            mover = new MouseOperator(1, BrowserWindRightX);
            clicker = new BoundingBoxClicker(mover);
            textWatcher = new TextWatcher(isBrowserMode);
        }

        public VisionArea TryScrollUpToTopEvent()
        {
            var maxY = Screen.PrimaryScreen.Bounds.Height - 40;
            var random = new Random();
            //1) form crop original Coords for browser window's polzunok;
            var origCropCoords = new int[]
            {
                BrowserWindRightX - 20,
                71,
                BrowserWindRightX,
                maxY
            };
            var wrap = new SearchArgsWrapper()
            {
                path = $@"Chrome\scroll_bar.jpg",
                originCropCoords = origCropCoords,
                threshhold = 0.95,
                preDelayPower = 1,
                maxWaitCount = 3,
                shouldMoveToAim = false,
                clickCount = 0
            };
            var wrapEmptySpace =
                 new SearchArgsWrapper()
                 {
                     path = $@"Chrome\scroll_empty_space.jpg",
                     originCropCoords = new int[]
                     {
                        BrowserWindRightX - 20,
                        88,
                        BrowserWindRightX,
                        maxY
                     },
                     threshhold = 0.75,
                     preDelayPower = 0,
                     maxWaitCount = 3,
                     shouldMoveToAim = false,
                     clickCount = 0,
                 };
            switch (random.Next(0, 2))
            {
                case 0:
                    return ScrollByDraggingBar(wrap, wrapEmptySpace,clicker);
                case 1:
                    return ScrollByScrolling(wrap, wrapEmptySpace, clicker, mover);
            }
            //analog of null for us but with no code crash
            return new VisionArea() { isAimFound = false};
        }
        private VisionArea ScrollByDraggingBar(SearchArgsWrapper wrap, SearchArgsWrapper wrapEmptySpace,
            Clicker clicker)
        {
            MySeriLogger.LogText(MethodBase.GetCurrentMethod().Name);
            var area = new BotActionFinisher(this).Finish(MethodBase.GetCurrentMethod().Name, wrap);
            var aimBox = area.OriginAimCoords;
            if (area.isAimFound)
            {       
                var spaceArea = new BotActionFinisher(this).Finish(MethodBase.GetCurrentMethod().Name,
                    wrapEmptySpace, shouldLogError: false);
                //If there is space between up arrow and bar (if bar is not at top)
                if (spaceArea.isAimFound)
                {
                    clicker.GoByRectangle(aimBox, 1, 0);
                    var slowMover = new MouseOperator(0.1, BrowserWindRightX);
                    //We are not sure that recognizing will be at very top of bar
                    //if missrecognising or if we still need to scroll more
                    if (aimBox.Y > 91)
                        slowMover.ClickAndDrag(0, new Random().Next(-20, -10));
                    //if should, scroll anyway
                    if (aimBox.Y > 100)
                    {
                        slowMover.ClickAndDrag(0, -(aimBox.Y - 85));
                        return ScrollByDraggingBar(wrap, wrapEmptySpace, clicker);
                    }
                }

            }
            return area;
        }
        private VisionArea ScrollByScrolling(SearchArgsWrapper wrap, SearchArgsWrapper wrapEmptySpace,
            Clicker clicker, MouseOperator mover)
        {
            MySeriLogger.LogText(MethodBase.GetCurrentMethod().Name);
            wrap.shouldMoveToAim = false;
            var area = new BotActionFinisher(this).Finish(MethodBase.GetCurrentMethod().Name, wrap);
            var aimBox = area.OriginAimCoords;
            if (area.isAimFound)
            {
                var spaceArea = new BotActionFinisher(this).Finish(MethodBase.GetCurrentMethod().Name,
                    wrapEmptySpace, shouldLogError: false);
                //If there is space between up arrow and bar (if bar is not at top)
                if (spaceArea.isAimFound)
                {
                    clicker.GoByRectangle(aimBox, 1, 0);
                    //We are not sure that recognizing will be at very top of bar
                    //if missrecognising or if we still need to scroll more
                    if (aimBox.Y > 91)
                        mover.ScrollUp(new Random().Next(3, 4));
                    //if should, scroll anyway
                    if (aimBox.Y > 100)
                    {
                        var destination = aimBox.Y - 85;
                        int scrollCounts = (int)Math.Ceiling((double)destination / 30);
                        mover.ScrollUp(scrollCounts);
                        return ScrollByScrolling(wrap, wrapEmptySpace, clicker, mover);
                    }
                }
            }
            return area;
        }
        public VisionArea WaitNClick(SearchArgsWrapper wrap, int preCycleDelay = 0, int preClickDelay = 2)
        {
            var area = new VisionArea();
            if (wrap.path != null)
                //Logging here inside in EmguWatcher
                area = WaitForElDeniedClick(wrap, true, preCycleDelay);
            else if (wrap.text != null)
                area = textWatcher.WaitForFutureClick(wrap, preCycleDelay);

            if (area.isAimFound)
            {
                var mover = new MouseOperator(1, BrowserWindRightX);
                var clicker = new BoundingBoxClicker(mover);
                if(wrap.shouldMoveToAim)
                {
                    if(wrap.randOccurance)
                        clicker.GoByRandOccurance(area, wrap, preClickDelay);
                    else
                        clicker.GoToTitleNLog(true, ref area, preClickDelay, wrap.clickCount,
                            wrap.textContainsInWord, wrap.getBiggestEl, wrap.getOneMostSimilar,
                            shouldLog: false);
                }
            }
            if (!wrap.shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return area;
        }

        public VisionArea WaitNPaste(bool shouldBitmapLive, SearchArgsWrapper wrap, string input, int preClickDelay = 2,
            int[] offsetCoords = null, int preCycleDelay = 0, int clickCount = 1)
        {
            var area = new VisionArea();
            if(wrap.path != null)
                //Logging here
                area = WaitForElDeniedClick(wrap, shouldBitmapLive, preCycleDelay);
            else if(wrap.text != null)
                //Logging here
                area = textWatcher.WaitForElDeniedClick(wrap, shouldBitmapLive, preCycleDelay);

            if (area.isAimFound)
            {
                    if(offsetCoords != null)
                    {
                        var v = area.AreaAnnos[0].BoundingPoly.Vertices;
                        v[0].X += offsetCoords[0];
                        v[0].Y += offsetCoords[1];
                        v[2].X += offsetCoords[2];
                        v[2].X += offsetCoords[3];
                    }
                    var mover = new MouseOperator(1, BrowserWindRightX);
                    new BoundingBoxClicker(mover).
                        PasteInByAreaCoordsWithEnterNLog(true, wrap, ref area, input, preClickDelay, clickCount,
                        shouldLog: false);
            }
            return area;
        }
        public VisionArea TryPasteInAddressBarEvent(string input, int preDelayPower = 2)
        {
            var wrap = new SearchArgsWrapper()
            {
                path = @"Chrome\refresh_page_icon.jpg",
                threshhold = 0.8,
                panelN = 2
            };
            var offsetCoords = new int[] { 100, 0, 300, 0 };

            var addressBarArea = new BotActionFinisher(this).
                PasteNFinish(
                    MethodBase.GetCurrentMethod().Name, wrap, input, preDelayPower,
                    offsetCoords, 0, 3
                    );
            return addressBarArea;
        }
    }
}
