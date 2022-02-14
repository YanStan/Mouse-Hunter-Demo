using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.Clickers;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;

namespace Mouse_Hunter.NeuralVision.EmguCV
{
    public class EmguWatcher : ImageWatcher
    {
        public EmguWatcher(bool isBrowserMode = true)
            : base(isBrowserMode) { }

        public VisionArea WaitForElDeniedClick(SearchArgsWrapper wrap, bool shouldBitmapLive = false, int preCycleDelay = 0,
            int counter = 1)
        {
            var mover = new MouseOperator(1, BrowserWindRightX);
            if (counter == 1)
                mover.SimulateDelay(preCycleDelay);
            mover.SimulateDelay((int)Math.Ceiling((double)wrap.preDelayPower / counter));
            var area = GetArea(true, wrap);
            area.isAimFound = area.AreaAnnos.Count != 0;
            if (!area.isAimFound && counter < wrap.maxWaitCount + 1)
            {
                area.Log();
                area = WaitForElDeniedClick(wrap, shouldBitmapLive, counter: counter+1);
            }
            else
                area = FinishWaiting(wrap.elNum, area, shouldBitmapLive);
            return area;
        }

        private static VisionArea FinishWaiting(int elNum, VisionArea area, bool shouldBitmapLive)
        {
            var allBounds = area.AreaAnnos;
            if (area.isAimFound)
                area.OriginAimCoords = RectConverter.
                        GetRectangleFromVertices(allBounds[elNum - 1].BoundingPoly.Vertices);

            area.Log();
            MySeriLogger.LogInfo(area);
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();  
            return area;
        }

        public VisionArea IsAimFound(bool shouldBitmapLive, SearchArgsWrapper wrap)
        {
            new MouseOperator(1, 3000).SimulateDelay(wrap.preDelayPower);
            var area = GetArea(true, wrap);
            area.isAimFound = area.AreaAnnos.Count != 0;
            if (area.isAimFound)
            {
                area.OriginAimCoords = RectConverter.
                        GetRectangleFromVertices(area.AreaAnnos[wrap.elNum - 1].BoundingPoly.Vertices);
            }
            area.Log();
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return area;
        }

        public VisionArea GetArea(bool shouldBitmapLive, SearchArgsWrapper wrap)
        {
            //Screening
            ScreenMaker.MakeScreenshot();
            var area = new VisionArea()
            {
                AreaAnnos = new List<EntityAnnotation>(),
                OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(wrap.panelN)
            };
            //Crop if we Should
            bool shouldCrop = CropIfShould(ref area, wrap);
            
            area = new ImageEMGUVisor(RectConverter).
                WatchArea(shouldCrop, wrap, ref area);
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return FixCropAnnosToOriginal(area);
        }  
    }
}
