using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.MovementSimulators
{
    public class MouseOperator
    {

        Random gaussian = new Random();
        //double nextNextGaussian;
        double NormalSpeedMultiPlyer;
        double SpeedMultiPlyer;
        SenselessMover senselessMover;

        public MouseOperator(double speedMultiPlyer, int browserWindRightX)
        {
            SpeedMultiPlyer = speedMultiPlyer;
            NormalSpeedMultiPlyer = speedMultiPlyer;
            senselessMover = new SenselessMover(1, browserWindRightX);
        }
        public void ChangeSpeedMultiPlyer(double speedMultiPlyer) => SpeedMultiPlyer = speedMultiPlyer;
        public void NormalizeSpeedMultiPlyer() => SpeedMultiPlyer = NormalSpeedMultiPlyer;

        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            MOUSEEVENTF_WHEEL = 0x0800

    }

    [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public void MoveToAreaWithProgress(IProgress<string> progress, Point leftPoint, Point rightPoint)
        {
            var targetPoint = MoveToArea(leftPoint, rightPoint);
            progress.Report($"X: {targetPoint.X}, Y: {targetPoint.Y}\r\n");
        }

        public void MoveToOffsettedArea(int offsetX1, int offsetY1, 
            int offsetX2, int offsetY2)
        {
            int originalX = Cursor.Position.X;
            int originalY = Cursor.Position.Y;
            var p1 = new Point()
            {
                X = originalX + offsetX1,
                Y = originalY + offsetY1,
            };
            var p2 = new Point()
            {
                X = originalX + offsetX2,
                Y = originalY + offsetY2,
            };
            MoveToArea(p1, p2);
        }

        public Point MoveToArea(Point leftPoint, Point rightPoint)
        {
            GaussianPointDistr distr = new GaussianPointDistr();
            var targetPoint = distr.RandomPointOnArea(leftPoint, rightPoint);
            int targetX = targetPoint.X;
            int targetY = targetPoint.Y;
            return MoveBySinglePoint(targetX, targetY);
        }
        public Point MoveBySinglePoint(int targetX, int targetY)
        {
            int originalX = Cursor.Position.X;
            int originalY = Cursor.Position.Y;
            int xLen = Math.Abs(originalX - targetX);
            int yLen = Math.Abs(originalY - targetY);

            var vectorLength = Math.Sqrt((xLen * xLen) + (yLen * yLen));
            var paramNormalizer = Math.Pow(vectorLength, 1.0 / 3);
            double rawSpeed = paramNormalizer * SpeedMultiPlyer;
            int sleepTimeMs = 1;
            if (rawSpeed <= 0.5)
                sleepTimeMs = (int)Math.Round(1 / rawSpeed);
            int speed = Math.Max(1, (int)Math.Round(rawSpeed));

            List<Point> points = BuildCurvePoints(targetX, targetY, vectorLength);

            int counter = 0;
            if (points.Count != 0)
            {
                Point finalPoint = points[0]; //!!!!!!FOR TESTING!!!!!
                foreach (var point in points)
                {
                    Cursor.Position = new Point(point.X, point.Y);
                    if (counter % speed == 0)
                        Thread.Sleep(sleepTimeMs);
                    counter++;
                    if (counter == points.Count)
                        finalPoint = point;
                };
                return finalPoint;
            }
            else
            {
                return new Point()
                {
                    X = originalX,
                    Y = originalY
                };
            }
        }

        public void Click()
        {
            Random random = new Random();
            Thread.Sleep(random.Next(85, 155));
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(random.Next(45, 55));
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(random.Next(45, 155));
        }
        public void ClickAndDrag(int xOffset, int yOffset)
        {
            Random random = new Random();
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(random.Next(250, 650));
            MoveBySinglePoint(Cursor.Position.X + xOffset, Cursor.Position.Y + yOffset);
            Thread.Sleep(random.Next(250, 450));
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(random.Next(45, 155));
        }
        public void DoubleClick()
        {
            Random random = new Random();
            Click();
            Thread.Sleep(random.Next(170, 250));
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(random.Next(45, 55));
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(random.Next(45, 155));
        }

        public int ScrollDown(int amount)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(520, 1800));
            for (int i = 0; i < amount; i++)
            {
                mouse_event((int)(MouseEventFlags.MOUSEEVENTF_WHEEL), 0, 0, -120, 0);
                Thread.Sleep(random.Next(90, 700));
            }
            Thread.Sleep(random.Next(220, 1500));
            return amount * -100; //real offset in pixels in Google Chrome
        }
        public int ScrollUp(int amount)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(520, 800));
            for (int i = 0; i < amount; i++)
            {
                mouse_event((int)(MouseEventFlags.MOUSEEVENTF_WHEEL), 0, 0, 120, 0);
                Thread.Sleep(random.Next(120, 200));
            }
            return amount * 100; //real offset in pixels in Google Chrome
        }

        public double NextGaussian(int gaussianAccuracy)
        {

            double v1, v2, s;
            do
            {
                v1 = 2 * gaussian.NextDouble() - 1;
                v2 = 2 * gaussian.NextDouble() - 1;

                s = v1 * v1 + v2 * v2;
            } while (s >= 1 || s == 0);
            double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
            //nextNextGaussian = v2 * multiplier;

            return v1 * multiplier / gaussianAccuracy;
        }

        public List<Point> BuildCurvePoints(int targetX, int targetY, double vectorLength)
        {
            int originalX = Cursor.Position.X;
            int originalY = Cursor.Position.Y;
            int midPointX = (originalX + targetX) / 2;
            int midPointY = (originalY + targetY) / 2;

            //Find a co-ordinate normal to the straight line between start and end point, starting at the midpoint and normally distributed
            //This is reduced by a factor of 4 to model the arc of a right handed user.
            Random random = new Random();

            var curvatureNormalizer = Math.Pow(vectorLength, 1.0 / 1.4) / 2.4;
            var randOffsetX = random.Next(-(int)curvatureNormalizer / 3, (int)curvatureNormalizer / 3 + 1);
            var randOffsetY = random.Next(-(int)curvatureNormalizer / 3, (int)curvatureNormalizer / 3 + 1);

            int bezierMidPointX = (int)Math.Round(midPointX + curvatureNormalizer + randOffsetX);
            int bezierMidPointY = (int)Math.Round(midPointY + curvatureNormalizer + randOffsetY);


            BezierCurve bc = new BezierCurve();
            double[] input = new double[]
            { originalX, originalY, bezierMidPointX, bezierMidPointY, targetX, targetY };

            int numberOfDataPoints = (int)vectorLength;
            double[] output = new double[numberOfDataPoints];

            //co-ords are couplets of doubles hence the / 2
            bc.Bezier2D(input, numberOfDataPoints / 2, output);


            List<System.Drawing.Point> points = new List<Point>();
            for (int count = 1; count < numberOfDataPoints - 2; count += 2)
            {
                Point point = new System.Drawing.Point((int)output[count + 1], (int)output[count]);
                points.Add(point);
            }
            return points;
        }

        public string SimulateDelay(int predelayPower)
        {
            Random random = new Random();
            int delayMs = random.Next(1000 * predelayPower, 2200 * predelayPower);
            int smallerDelayMs = (int)Math.Round((double)delayMs / 3);
            Thread.Sleep(Math.Max(87, smallerDelayMs));
            MoveSenseless(predelayPower);
            Thread.Sleep(smallerDelayMs * 2);
            MoveSenseless(predelayPower);
            return $"Бот симулирует задержку в {delayMs} мс.\r\n\r\n";
        }
        public void MoveSenseless(int predelayPower, int speed = 1)
        {
            senselessMover.SmallMoveRandom(predelayPower);
        }
    }
}
