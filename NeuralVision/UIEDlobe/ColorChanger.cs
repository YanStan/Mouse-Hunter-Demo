using System;
using System.IO;
using Mouse_Hunter.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class ColorChanger
    {
        struct MinMaxValues
        {
            public int iRMin;
            public int iRMax;
            public int iGMin;
            public int iGMax;
            public int iBMin;
            public int iBMax;
        }
        public System.Drawing.Bitmap ChangeColors(bool changeAllAnotherColorsMode,
            byte[] rgb, int tolerance,
            int panelMode,
            bool isOriginalMutated
            )
        {
            var oldnNewColors = FormOldnNewColors(rgb);
            MinMaxValues minMaxValues = GetValuesForTolerantedColors(oldnNewColors, tolerance);
            ImageRepository repos = new ImageRepository();
            Image input = repos.GetImageSharp(panelMode, isOriginalMutated);
                var inputArgb32 = input.CloneAs<Argb32>();
                for (int x = 0; x < input.Width; x++)
                {
                    for (int y = 0; y < input.Height; y++)
                    {
                        var pix = inputArgb32[x, y];
                        //Determinig Color Match
                        var isMatch = ((pix.R >= minMaxValues.iRMin && pix.R <= minMaxValues.iRMax) &&
                            (pix.G >= minMaxValues.iGMin && pix.G <= minMaxValues.iGMax) &&
                            (pix.B >= minMaxValues.iBMin && pix.B <= minMaxValues.iBMax)
                            );
                        if (changeAllAnotherColorsMode)
                        {
                            if (!isMatch)
                                inputArgb32[x, y] = oldnNewColors[1];
                        }
                        else
                        {
                            if (isMatch)
                                inputArgb32[x, y] = oldnNewColors[1];
                        }
                    }
                }
                repos.SaveImageSharp(inputArgb32, panelMode, isOriginalMutated);
                return (System.Drawing.Bitmap)repos.GetImage(panelMode, isOriginalMutated);
        }
      
        private static MinMaxValues GetValuesForTolerantedColors(Color[] oldnNewColors, int tolerance)
        {
            var pixelOld = oldnNewColors[0].ToPixel<Argb32>();
            MinMaxValues minMaxValues = new MinMaxValues();
            //RED
            minMaxValues.iRMin = Math.Max(pixelOld.R - tolerance, 0);
            minMaxValues.iRMax = Math.Min(pixelOld.R + tolerance, 255);
            //GREEN
            minMaxValues.iGMin = Math.Max(pixelOld.G - tolerance, 0);
            minMaxValues.iGMax = Math.Min(pixelOld.G + tolerance, 255);
            //BLUE
            minMaxValues.iBMin = Math.Max(pixelOld.B - tolerance, 0);
            minMaxValues.iBMax = Math.Min(pixelOld.B + tolerance, 255);
            return minMaxValues;
        }

        private static Color[] FormOldnNewColors(byte[] rgb, byte[] rgb2 = null)
        {
            rgb2 = rgb2 ?? new byte[] { 255, 255, 255 };
            Color colorOld = Color.FromRgb(rgb[0], rgb[1], rgb[2]);
            Color colorNew = Color.FromRgb(rgb2[0], rgb2[1], rgb2[2]);
            Color[] oldnNewColors = new Color[]
            {
                colorOld, colorNew
            };
            return oldnNewColors;
        }
    }
}
