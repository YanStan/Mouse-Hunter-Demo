using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.Wrappers;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.Clickers
{
    public class BoundingBoxClicker : Clicker
    {
        public BoundingBoxClicker(MouseOperator mouseMover)
            : base(mouseMover) { }

        public void GoByRandOccurance(VisionArea area, SearchArgsWrapper wrap,
            int preClickDelay)
        {
            int randN = new Random().Next(0, area.AreaAnnos.Count);
            var vertices = area.AreaAnnos[randN].BoundingPoly.Vertices;
            //TODO This "0" is bad code. Just give him it as mtd param.
            var randAimBox = new NeuralVision.RectangleConverter(0)
                .GetRectangleFromVertices(vertices);
            GoByRectangle(randAimBox, preClickDelay, wrap.clickCount);
        }
        public VisionArea PasteInByAreaCoordsWithEnterNLog(bool shouldBitmapLive, SearchArgsWrapper wrap, ref VisionArea visionArea,
            string inputText, int predelayPower, int clickCount = 1, bool shouldLog = true)
        {   
            visionArea = PasteInByAreaCoordsNLog(shouldBitmapLive, wrap, ref visionArea, inputText, predelayPower,
                clickCount, shouldLog);
            if (visionArea.isAimFound && wrap.shouldPressEnter)
                SendKeys.SendWait("{ENTER}");
            return visionArea;
        }

        public VisionArea PasteInByAreaCoordsNLog(bool shouldBitmapLive, SearchArgsWrapper wrap, ref VisionArea visionArea, string inputText,
            int predelayPower, int clickCount = 1,
            bool shouldLog = true)
        {
            visionArea = GoToTitleNLog(shouldBitmapLive, ref visionArea, predelayPower, clickCount,
                wrap.textContainsInWord, wrap.getBiggestEl, wrap.getOneMostSimilar, shouldLog);
            if (wrap.pasteWithBackspace)
                for (int i = 0; i < 5; i++)
                    SendWaitFullStrAtOnce("^{BACKSPACE}");
            if (visionArea.isAimFound)
                SendWaitAsHumanTyping(inputText);
            return visionArea;
        }

        public VisionArea GoToTitleNLog(bool shouldBitmapLive, ref VisionArea area,
            int predelayPower, int clickCount = 1, bool textContainsInWord = false,
            bool getBiggestEl = true, bool getOneMostSimilar = true, bool shouldLog = true)
        {
            var lotOfBounds = new BoundSeeker().
                SearchBoxesCoordsByDescription(area.AreaAnnos, area.AimName, textContainsInWord, getOneMostSimilar);
            if (lotOfBounds != null && lotOfBounds.Count != 0 && lotOfBounds[0] != null)
            {
                Rectangle bound;
                if (getBiggestEl)
                    bound = new NeuralVision.RectangleConverter(0).GetBiggestBox(lotOfBounds);
                else
                    bound = lotOfBounds[0];
                GoByRectangle(bound, predelayPower, clickCount);
                area.OriginAimCoords = bound;
                area.isAimFound = true;
            }
            else
            {
                area.isAimFound = false;
            }
            //This method SHOULD LOG cause
            //You may use it by changed Title for text search
            if(shouldLog)
            {
                //ONLY IN THAT ORDER! OTHERWISE logfile: will be zero
                area.Log();
                MySeriLogger.LogInfo(area);
            }
            if (!shouldBitmapLive)
                area.AreaBitmap.Dispose();
            return area;               
        }
    }
}
