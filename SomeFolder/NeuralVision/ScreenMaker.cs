using Mouse_Hunter.NeuralVision.UIEDlobe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision
{
    public class ScreenMaker
    {

        protected static int BrowserWindRightX;
        private static bool IsBrowserMode;

        public ScreenMaker(int browserWindRightX, bool isBrowserMode)
        {
            BrowserWindRightX = browserWindRightX;
            IsBrowserMode = isBrowserMode;
        }
        public void MakeScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                var c = new ScreenCropper();
                if (IsBrowserMode)
                    //Crop to browser window
                    using (Bitmap browserBitmap = c.CropImage(bitmap, 0, 0, BrowserWindRightX, bitmap.Height))
                        CutOnPanels(browserBitmap, c);
                else
                    CutOnPanels(bitmap, c);

            }
        }

        private static void CutOnPanels(Bitmap bitmap, ScreenCropper c)
        {
            var h = bitmap.Height;
            c.CropBrowserToPanel(bitmap, 0, 34, 1);
            c.CropBrowserToPanel(bitmap, 34, 71, 2);
            c.CropBrowserToPanel(bitmap, 71, h - 40, 0);
            c.CropBrowserToPanel(bitmap, h - 40, h, 3);
        }
    }
}
