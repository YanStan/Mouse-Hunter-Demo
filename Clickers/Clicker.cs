using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.Clickers
{
    public class Clicker
    {
        public MouseOperator MouseMover;
        protected Random random = new Random();
        protected IProgress<string> Progress;

        public Clicker(MouseOperator mouseMover)
        {
            MouseMover = mouseMover;
        }
        public int ScrollDown(int amount) => MouseMover.ScrollDown(amount);
        public int ScrollUp(int amount) => MouseMover.ScrollUp(amount);
        
        public List<Point> BuildBoxPoints(int leftX, int leftY, int rightX, int rightY)
        {
            return new List<Point> {
                new Point()
                {
                    X = leftX,
                    Y = leftY
                },
                new Point()
                {
                    X = rightX,
                    Y = rightY
                }
            };
        }
        public void GoByRectangle(Rectangle bound, int predelayPower, int clickCount = 1)
        {
            var leftX = bound.X;
            var leftY = bound.Y;
            var rightX = leftX + bound.Width;
            var rightY = leftY + bound.Height;
            var coords = new int[] { leftX, leftY, rightX, rightY };
            GoByAreaCoords(coords, predelayPower, clickCount);
        }
        public void GoByAreaCoords(int[] coords, int predelayPower, int clickCount = 1)
        {
            MouseMover.SimulateDelay(predelayPower);
            var boxPoints = BuildBoxPoints(coords[0], coords[1], coords[2], coords[3]);
            MouseMover.MoveToArea(boxPoints[0], boxPoints[1]);
            for (int i = 0; i < clickCount; i++)
                MouseMover.Click();
        }

        public VisionArea GoByAimCoords(bool shouldBitmapLive, ref VisionArea visArea, 
            int predelayPower, int clickCount = 1, bool shouldLog = true)
        {
            if (visArea.OriginAimCoords != null)
            {
                var bound = visArea.OriginAimCoords;
                GoByRectangle(bound, predelayPower, clickCount);
                visArea.isAimFound = true;
            }
            else
                visArea.isAimFound = false;
            //This method SHOULD LOG cause
            //You may use it by changed aimCoords manually
            if(shouldLog)
            {
                visArea.Log();
                MySeriLogger.LogInfo(visArea);
            }
            if(!shouldBitmapLive)
                visArea.AreaBitmap.Dispose();
            return visArea;
        }
/*
        public void GoByAreaCoordsNDrag(int leftX, int leftY, int rightX, int rightY, int predelayPower)
        {
            GoByAreaCoordsNoClick(leftX, leftY, rightX, rightY, predelayPower);
            Thread.Sleep(random.Next(55, 100));
            MouseMover.ClickAndDrag();
        }*/

        public void SendWaitAsHumanTyping(string text)
        {
            Thread.Sleep(random.Next(500, 1000));
            foreach (char character in text)
            {
                SendKeys.SendWait(character.ToString());
                Thread.Sleep(random.Next(70, 300));
            }
        }
        public void SendWaitFullStrAtOnce(string text)
        {
            SendKeys.SendWait(text);
            Thread.Sleep(random.Next(70, 300));
        }

        public void ScrollOverView(int minScrollDown, int maxScrollDown, int minFinSkrollDown, int maxFinSkrollDown)
        {
            MouseMover.SimulateDelay(4);
            MySeriLogger.LogText(MethodBase.GetCurrentMethod().Name +
                $"{minScrollDown} {maxScrollDown} {minFinSkrollDown} {maxFinSkrollDown}");

            var skrollCount = random.Next(minScrollDown, maxScrollDown+1);
            ScrollDown(skrollCount);
            if (skrollCount > maxFinSkrollDown)
                ScrollUp((skrollCount - maxFinSkrollDown) + random.Next(0, maxFinSkrollDown - minFinSkrollDown+1));
        }
    }
}
