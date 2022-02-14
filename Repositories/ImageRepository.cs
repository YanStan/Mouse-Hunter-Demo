using Mouse_Hunter.NeuralVision.UIEDlobe;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Mouse_Hunter.Repositories
{
    public class ImageRepository
    {
        private ImgPathWorkerSingleton i = ImgPathWorkerSingleton.GetInstance();
        public ImageRepository()
        { }


        public System.Drawing.Image GetImage(int panelMode, bool isMutated = false)
        {
            var fullInputPath = i.GetFullPath(panelMode, isMutated);
            return FromFile(fullInputPath);
        }

        public SixLabors.ImageSharp.Image GetImageSharp(int panelMode, bool isMutated = false)
        {
            var fullInputPath = i.GetFullPath(panelMode, isMutated);
            return FromStream(fullInputPath);
        }

        public Image<Bgr, byte> GetEmguImage(string filePath) =>  new Image<Bgr, byte>(filePath);

        public void SaveEmguImage(Image<Bgr, byte> imageToSave, string filePath) =>
            imageToSave.Save(filePath);

        public void SaveImage(Bitmap bitmap, int panelMode, bool isMutated)
        {
            var fullOutputPath = i.GetFullPath(panelMode, isMutated);
            bitmap.Save(fullOutputPath, ImageFormat.Jpeg);
        }

        public void SaveImageSharp(Image<Argb32> inputArgb32, int panelMode, bool isMutated)
        {
            var fullOutputPath = i.GetFullPath(panelMode, isMutated);
            inputArgb32.Save(fullOutputPath);
        }




        public static System.Drawing.Image FromFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        public static SixLabors.ImageSharp.Image FromStream(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = SixLabors.ImageSharp.Image.Load(ms);
            return img;
        }
    }
}
