using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Hunter.NeuralVision.GoogleOCR
{
    public class TextWatcher : ImageWatcher
    {
        public TextWatcher(bool isBrowserMode = true)
            : base(isBrowserMode) { }

        public VisionArea WaitForElDeniedClick(SearchArgsWrapper wrap, bool shouldBitmapLive = false, 
            int preCycleDelay = 0,
            int counter = 1)
        {
            var area = WaitForFutureClick(wrap, counter, preCycleDelay);
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return area;
        }
        public VisionArea WaitForFutureClick(SearchArgsWrapper wrap, int preCycleDelay = 0, int counter = 1)
        {
            var mover = new MouseOperator(1, BrowserWindRightX);
            if (counter == 1)
                mover.SimulateDelay(preCycleDelay);
            if (wrap.moveWhileDelay)
                mover.SimulateDelay((int)Math.Ceiling((double)wrap.preDelayPower / counter));
            else
                Thread.Sleep((int)Math.Floor((double)wrap.preDelayPower * 1500 / counter));
            var area = GetArea(true, wrap);
            area.AimName = wrap.text;
            var allElPoints = new BoundSeeker().SearchBoxesCoordsByDescription(area.AreaAnnos,
                wrap.text, wrap.textContainsInWord);
            //!!!!!
            area.isAimFound = allElPoints != null && allElPoints.Count != 0;
            if (!area.isAimFound && counter < wrap.maxWaitCount + 1)
            {
                counter++;
                area.Log();
                area = WaitForFutureClick(wrap, counter: counter);
            }
            else
                area = FinishWaiting(wrap.elNum, area, allElPoints);
            return area;
        }
        private static VisionArea FinishWaiting(int elNum, VisionArea area, List<Rectangle> allBounds)
        {

            if (area.isAimFound)
                area.OriginAimCoords = allBounds[elNum - 1];
            area.Log();
            MySeriLogger.LogInfo(area);
            return area;
        }

        public VisionArea IsAimFound(bool shouldBitmapLive, SearchArgsWrapper wrap)
        {
            new MouseOperator(1, 3000).SimulateDelay(wrap.preDelayPower);
            var area = GetArea(true, wrap);
            var bS = new BoundSeeker();
            var allElPoints = bS.SearchBoxesCoordsByDescription(area.AreaAnnos, wrap.text, wrap.textContainsInWord);
            area.AimName = wrap.text;
            area.isAimFound = allElPoints != null && allElPoints.Count != 0;
            if(area.isAimFound)
                area.OriginAimCoords = allElPoints[wrap.elNum - 1];
            area.Log();
            MySeriLogger.LogInfo(area);
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return area;
        }

        public VisionArea GetArea(bool shouldBitmapLive, SearchArgsWrapper wrap)
        {
            ScreenMaker.MakeScreenshot();
            //Preparing  
            var visionArea = new VisionArea()
            {
                OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(wrap.panelN)
            };
            //Crop if we Should
            bool shouldCrop = CropIfShould(ref visionArea, wrap);
            //Process visor anyway
            GoogleOCRVisor visor = new GoogleOCRVisor();
            var lang = GoogleLanguage.GetAll.FirstOrDefault(x => x.Name == "Russian").Code;
            TryGetOCRWordsRecursive(wrap, visionArea, shouldCrop, visor, lang);
            if (string.IsNullOrEmpty(visor.Error) == false)
                return null;
            else
                return FixCropAnnosToOriginal(visionArea);
        }

        private static void TryGetOCRWordsRecursive(SearchArgsWrapper wrap, VisionArea visionArea,
            bool shouldCrop, GoogleOCRVisor visor, string lang, int counter = 0)
        {
            try
            {
                visionArea.AreaAnnos = visor.GetOCRWords(wrap.panelN, shouldCrop, lang);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MySeriLogger.LogConnectionError(MethodBase.GetCurrentMethod().Name);
                if(counter < 5)
                {
                    Thread.Sleep(2000);
                    TryGetOCRWordsRecursive(wrap, visionArea, shouldCrop, visor, lang, counter+1);
                }
            }
        }
    }
}
