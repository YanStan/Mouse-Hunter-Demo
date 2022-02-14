/*using OpenQA.Selenium;
using System.Drawing;

namespace Mouse_Hunter.Wrappers
{
    public class Points
    {
        public Point LeftPoint { get; private set; }
        public Point RightPoint { get; private set; }
        public Points(IWebElement element, Point offsetPoint)
        {
            int xLeft = element.Location.X + offsetPoint.X;
            int yLeft = element.Location.Y + offsetPoint.Y;
            int xRight = xLeft + element.Size.Width;
            int yRight = yLeft + element.Size.Height;
            LeftPoint = new Point()
            {
                X = xLeft,
                Y = yLeft
            };
            RightPoint = new Point()
            {
                X = xRight,
                Y = yRight
            };
        }
    }
}
*/