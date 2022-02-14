using MathNet.Numerics.Distributions;
using System;
using System.Drawing;

namespace Mouse_Hunter.MovementSimulators
{
    public class GaussianPointDistr
    {
        public Point GetRandomPoint(int xMiddleOffsetted, int yMiddleOffsetted, int deviationX, int deviationY)
        {
            return new Point
            {
                X = (int)Math.Round(new Normal(xMiddleOffsetted, deviationX).Sample()),
                Y = (int)Math.Round(new Normal(yMiddleOffsetted, deviationY).Sample())
            };
        }

        public Point RandomPointOnArea(Point leftPoint, Point rightPoint)
        {
            double xlenOffsetRaw = Math.Abs(leftPoint.X - rightPoint.X) / 10;
            double ylenOffsetRaw = Math.Abs(leftPoint.Y - rightPoint.Y) / 10;
            int xlenOffset = (int)Math.Round(xlenOffsetRaw);
            int ylenOffset = (int)Math.Round(ylenOffsetRaw);

            int xMiddleOffsetted = (int)Math.Round((double)((leftPoint.X + rightPoint.X) / 2)) - 3 * xlenOffset;
            int yMiddleOffsetted = (int)Math.Round((double)((leftPoint.Y + rightPoint.Y) / 2)) + ylenOffset;

            // if 8/10 of total length (4/10 in each side), then 368 of 1000 would pass 1/2 distance from center 
            int deviationX = xlenOffset * 4;
            int deviationY = ylenOffset * 4;

            Point p = GetRandomPoint(xMiddleOffsetted, yMiddleOffsetted, deviationX, deviationY);
            //click area constriction
            var leftX = leftPoint.X + (xlenOffset * 5);
            var rightX = rightPoint.X - (xlenOffset * 2);
            var leftY = leftPoint.Y + (ylenOffset * 5);
            var rightY = rightPoint.Y - (ylenOffset * 2);
            while (p.X < (leftX) || p.X > (rightX)
                || p.Y < (leftY) || p.Y > (rightY))
            {
                p = GetRandomPoint(xMiddleOffsetted, yMiddleOffsetted, deviationX, deviationY);
            }

            return p;
        }
    }
}
