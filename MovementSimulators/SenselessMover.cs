using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_Hunter.MovementSimulators
{
    public class SenselessMover
    {
        private int Speed;
        private int BrowserWindRightX;
        public SenselessMover(int speed, int browserWindRightX)
        {
            Speed = speed;
            BrowserWindRightX = browserWindRightX;
        }

        public void ChangeSpeed(int speed) => Speed = speed;
        private Point GetRightDownPoint(int length) => GetPoint(length, length);
        private Point GetRightUpPoint(int length) => GetPoint(length, -length);
        private Point GetLeftDownPoint(int length) => GetPoint(-length, length);
        private Point GetLeftUpPoint(int length) => GetPoint(-length, -length);
        private Point GetUpPoint(int length) => GetPoint(0, -length);
        private Point GetDownPoint(int length) => GetPoint(0, length);
        private Point GetLeftPoint(int length) => GetPoint(-length, 0);
        private Point GetRightPoint(int length) => GetPoint(length, 0);

        private void Move(Point target)
        {
            //TODO REF 3000 later.
            MouseOperator mouseMover = new MouseOperator(Speed, 3000);
            mouseMover.MoveBySinglePoint(target.X, target.Y);
        }


        //=> Cursor.Position = new Point(target.X, target.Y);


        private void SmallMove1(int length) => Move(GetRightDownPoint(length));
        private void SmallMove2(int length) => Move(GetRightUpPoint(length));
        private void SmallMove3(int length)
        {
            Move(GetLeftUpPoint(length));
            Move(GetLeftPoint(length));
        }
        private void SmallMove4(int length) => Move(GetLeftPoint(length));
        private void SmallMove5(int length)
        {
            Move(GetDownPoint(length));
            Move(GetRightDownPoint(length));
            Move(GetRightUpPoint(length));
        }

        private void SmallMove6(int length) => Move(GetLeftUpPoint(length));
        private void SmallMove7(int length)
        {
            Move(GetDownPoint(length));
            Move(GetRightDownPoint(length));
        }
        private void SmallMove8(int length)
        {
            Move(GetLeftUpPoint(length));
            Move(GetUpPoint(length));
        }
        private void SmallMove9(int length)
        {
            Move(GetDownPoint(length));
            Move(GetLeftDownPoint(length));
        }
        private void SmallMove10(int length) => Move(GetLeftDownPoint(length));

        private void SmallMove11(int length)
        {
            Move(GetUpPoint(length));
            Move(GetRightUpPoint(length));
        }

        private void SmallMove12(int length)
        {
            Move(GetLeftUpPoint(length));
            SmallMove11(length);
        }

        private void SmallMove13(int length)
        {
            Move(GetRightPoint(length));
            Move(GetRightUpPoint(length));
        }

        private void SmallMove14(int length)
        {
            Move(GetLeftDownPoint(length));
            Move(GetDownPoint(length));
        }

        private void SmallMove15(int length)
        {
            Move(GetUpPoint(length));
            Move(GetLeftUpPoint(length));
        }
        private void SmallMove16(int length)
        {
            Move(GetLeftDownPoint(length));
            Move(GetLeftPoint(length));
        }
        private void SmallMove17(int length)
        {
            Move(GetLeftDownPoint(length));
            Move(GetRightDownPoint(length));
            Move(GetRightUpPoint(length));
        }

        private void SmallMove18(int length)
        {
            Move(GetRightDownPoint(length));
            Move(GetRightPoint(length));
        }

        private void SmallMove19(int length)
        {
            Move(GetDownPoint(length));
            Move(GetDownPoint(length));
            Move(GetRightDownPoint(length * 2));

        }

        private void SmallMove20(int length)
        {
            Move(GetDownPoint(length));
            Move(GetLeftDownPoint(length));
            Move(GetRightPoint(length));

        }


        public void SmallMoveRandom(int predelPowerForCalcChance, bool forcedChance = false)
        {
            int probability = (int)Math.Floor((double)10 / Math.Max(
                predelPowerForCalcChance, 1)
                );
            Random random = new Random();
            var isNoSenseless = random.Next(0, probability+1);
            if (isNoSenseless == 0)
            {
                ChooseSmallMoveType(random.Next(42, 58));
                Thread.Sleep(random.Next(250, 300));
            }
        }
        private void ChooseSmallMoveType(int length)
        {
            Random random = new Random();
            int movementNum = random.Next(1, 21);
            switch (movementNum)
            {
                case 1:
                    SmallMove1(length);
                    break;
                case 2:
                    SmallMove2(length);
                    break;
                case 3:
                    SmallMove3(length);
                    break;
                case 4:
                    SmallMove4(length);
                    break;
                case 5:
                    SmallMove5(length);
                    break;
                case 6:
                    SmallMove6(length);
                    break;
                case 7:
                    SmallMove7(length);
                    break;
                case 8:
                    SmallMove8(length);
                    break;
                case 9:
                    SmallMove9(length);
                    break;
                case 10:
                    SmallMove10(length);
                    break;
                case 11:
                    SmallMove11(length);
                    break;
                case 12:
                    SmallMove12(length);
                    break;
                case 13:
                    SmallMove13(length);
                    break;
                case 14:
                    SmallMove14(length);
                    break;
                case 15:
                    SmallMove15(length);
                    break;
                case 16:
                    SmallMove16(length);
                    break;
                case 17:
                    SmallMove17(length);
                    break;
                case 18:
                    SmallMove18(length);
                    break;
                case 19:
                    SmallMove19(length);
                    break;
                case 20:
                    SmallMove20(length);
                    break;
            }
        }


        private Point GetPoint(int lengthX, int lengthY)
        {
            int randOffstet = Math.Abs(lengthX) / 4;
            Random random = new Random();
            int originalX = Cursor.Position.X;
            int originalY = Cursor.Position.Y;
            var targetX = originalX + lengthX + random.Next(-randOffstet, randOffstet + 1);
            var targetY = originalY + lengthY + random.Next(-randOffstet, randOffstet + 1);

            //No SenseleSS Outside mainBox
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;
            var maxYForSenseless = screenHeight - 50;
            var minYForSenseless = 81;
            targetY = Math.Min(targetY, maxYForSenseless);
            targetY = Math.Max(targetY, minYForSenseless);

            targetX = Math.Min(targetX, BrowserWindRightX - 25);      
            return new Point()
            {
                X = targetX,
                Y = targetY
            };
        }
    }
}
