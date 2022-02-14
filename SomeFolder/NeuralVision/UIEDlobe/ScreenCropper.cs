using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.NeuralVision.GoogleOCR;
using Mouse_Hunter.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class ScreenCropper
    {
        public VisionArea Crop(VisionArea visionArea, float[] percentages, int panelMode)
        {
            //We must have panelMod here
            //for converting crop percentages inside some Panel  -  in original screen coords
            ImageRepository repos = new ImageRepository();
            System.Drawing.Image screen = repos.GetImage(panelMode);
                int[] relCropCoords = new int[]
                {
                    (int)(percentages[0] / 100 * screen.Width),
                    (int)(percentages[1] / 100 * screen.Height),
                    (int)(percentages[2] / 100 * screen.Width),
                    (int)(percentages[3] / 100 * screen.Height)
                };
                var originCropCoords = CoordsOffset(panelMode, relCropCoords);
                return CropNSaveWithCoordsUpdate(screen, originCropCoords, true, panelMode, visionArea);
        }
        public VisionArea CropMainScreen(VisionArea visionArea, int[] originCropCoords)
        {
            int panelMode = 0;
            ImageRepository repos = new ImageRepository();
            System.Drawing.Image screen = repos.GetImage(panelMode);
                return CropNSaveWithCoordsUpdate(screen, originCropCoords, true, panelMode, visionArea);
        }

        public VisionArea CropBrowserToPanel(System.Drawing.Image screen, int height1, int height2, int panelMode)
        {
            int[] originCropCoords = new int[]
            {
                0, height1, screen.Width, height2
            };
            int cropWidth = screen.Width;
            int cropHeight = height2 - height1;
            int[] cropDimensions = new int[] { cropWidth, cropHeight };
            return CropNSave(screen, originCropCoords, cropDimensions, false, panelMode);
        }

        private VisionArea CropNSave(System.Drawing.Image screen, int[] originCropCoords, int[] cropDimensions, 
            bool saveAsMutated,
            int panelMode = 1, VisionArea visionArea = null)
        {
            //
            var bitmap = CropImage(screen, originCropCoords[0], originCropCoords[1], cropDimensions[0], cropDimensions[1]);
            ImageRepository repos = new ImageRepository();
            repos.SaveImage(bitmap, panelMode, saveAsMutated);
            if (visionArea != null)
                visionArea.AreaBitmap = bitmap;
            return visionArea;
        }

        private VisionArea CropNSaveWithCoordsUpdate(System.Drawing.Image screen, int[] originCropCoords, bool saveAsMutated,
            int panelMode = 1, VisionArea visionArea = null)
        {
            var coordsForPanCropping = UpdateAreasCoords(ref originCropCoords, ref visionArea, panelMode);
            int cropWidth = coordsForPanCropping[2] - coordsForPanCropping[0];
            int cropHeight = coordsForPanCropping[3] - coordsForPanCropping[1];
            int[] cropDimensions = new int[] { cropWidth, cropHeight };
            return CropNSave(screen, coordsForPanCropping, cropDimensions, saveAsMutated, panelMode, visionArea);
        }
        private static int[] UpdateAreasCoords(ref int[] originCropCoords, ref VisionArea visionArea, int panelMode)
        {
            int x1 = originCropCoords[0];
            int y1 = originCropCoords[1];
            int x2 = originCropCoords[2];
            int y2 = originCropCoords[3];
            if (visionArea != null)
            {
                visionArea.OriginAreaCoords.X = x1;
                visionArea.OriginAreaCoords.Y = y1;
                visionArea.OriginAreaCoords.Width = x2 - x1;
                visionArea.OriginAreaCoords.Height = y2 - y1;
            }

            //reverse offset cause here we always crop one of panels, but must give original coords to all methods
            var coordsForPanCropping = ReverseCoordsOffset(panelMode, x1, y1, x2, y2);


            return coordsForPanCropping;
        }

        private static int[] ReverseCoordsOffset(int panelMode, int x1, int y1, int x2, int y2)
        {
            int[] originCropCoords;
            switch (panelMode)
            {
                //reverse offset cause here we always crop one of panels, but must give original coords to all methods
                case 1:
                    break;
                case 2:
                    y1 -= 34;
                    y2 -= 34;
                    break;
                case 0:
                    y1 -= 71;
                    y2 -= 71;
                    break;
                case 3:
                    var offset = Screen.PrimaryScreen.Bounds.Height - 40;
                    y1 -= offset;
                    y2 -= offset;
                    break;
                default:
                    break;
            }
            originCropCoords = new int[] { x1, y1, x2, y2 };
            return originCropCoords;
        }

        private static int[] CoordsOffset(int panelMode, int[] relCropCoords)
        {
            int x1 = relCropCoords[0];
            int y1 = relCropCoords[1];
            int x2 = relCropCoords[2];
            int y2 = relCropCoords[3];
            switch (panelMode)
            {
                //reverse offset cause here we always crop one of panels, but must give original coords to all methods
                case 1:
                    break;
                case 2:
                    y1 += 34;
                    y2 += 34;
                    break;
                case 0:
                    y1 += 71;
                    y2 += 71;
                    break;
                case 3:
                    var offset = Screen.PrimaryScreen.Bounds.Height - 40;
                    y1 += offset;
                    y2 += offset;
                    break;
            }
            var originCropCoords = new int[] { x1, y1, x2, y2 };
            return originCropCoords;
        }

        public Bitmap CropImage(System.Drawing.Image screen, int x1, int y1, int cropWidth, int cropHeight)
        {
            Rectangle crop = new Rectangle(x1, y1, cropWidth, cropHeight);
            var bitmap = new Bitmap(crop.Width, crop.Height);
            using (var graph = Graphics.FromImage(bitmap))
            {
                graph.DrawImage(screen, new Rectangle(0, 0, bitmap.Width, bitmap.Height), crop, GraphicsUnit.Pixel);
            }
            return bitmap;
        }
    }
}
