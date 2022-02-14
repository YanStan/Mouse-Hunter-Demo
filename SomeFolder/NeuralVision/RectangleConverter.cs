using Google.Apis.Vision.v1.Data;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision
{
    public class RectangleConverter
    {
        private static int BrowserWindRightX;
        public RectangleConverter(int browserWindRightX) => BrowserWindRightX = browserWindRightX;

        public Rectangle GetBiggestBox(List<Rectangle> lotOfBounds)
        {
            return lotOfBounds.MaxBy(x =>
            (x.Width - x.X)
            *
            (x.Height - x.Y))
                .ToList()[0];
        }

        public System.Drawing.Rectangle GetPanelAreaRectFromPanelMode(int panelMode)
        {
            Rectangle areaPoints = new Rectangle
            {
            };
            var screenH = Screen.PrimaryScreen.Bounds.Height;

            areaPoints.X = 0;
            areaPoints.Width = BrowserWindRightX;

            switch (panelMode)
            {
                //make absolute coords from crop's internal
                case 0:
                    areaPoints.Y = 71;
                    areaPoints.Height = screenH - 71 - 40;
                    break;
                case 1:
                    areaPoints.Y = 0;
                    areaPoints.Height = 34;
                    break;
                case 2:
                    areaPoints.Y = 34;
                    areaPoints.Height = 37;
                    break;
                case 3:
                    areaPoints.Y = screenH - 40;
                    areaPoints.Height = 40;
                    break;
            }
            return areaPoints;
        }

        public Rectangle GetRectangleFromVertices(IList<Vertex> vertices)
        {
            var x1 = vertices[0].X;
            var y1 = vertices[0].Y;
            var x3 = vertices[2].X;
            var y3 = vertices[2].Y;

            var rect = new Rectangle();
            if(x1 != null && y1 != null &&
               x3 != null && y3 != null)
            {
                rect.X = (int)x1;
                rect.Y = (int)y1;
                rect.Width = (int)(x3 - x1);
                rect.Height = (int)(y3 - y1);
            }
            return rect;
        }

        public int[] GetIntArrayFromRectangle(Rectangle r) =>
            new int[] { r.X, r.Y, r.X + r.Width, r.Y + r.Height };

        public IList<Vertex> GetVerticesFromRectangle(Rectangle rect)
        {
            var x1 = rect.X;
            var y1 = rect.Y;
            var x2 = rect.Width + rect.X;
            var y2 = rect.Height + rect.Y;
            return new List<Vertex>()
            {
                new Vertex()
                {
                    X = x1,
                    Y = y1,
                },
                new Vertex()
                {
                    X = x2,
                    Y = y1,
                },
                new Vertex()
                {
                    X = x2,
                    Y = y2,
                },
                new Vertex()
                {
                    X = x1,
                    Y = y2,
                },
            };
        }
    }
}
