using Emgu.CV.Structure;
using Newtonsoft.Json;
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using System;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.Repositories;
using Mouse_Hunter.Wrappers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Mouse_Hunter.Repositories
{
    public class AreaLogger
    {
        public string LogThatNotFound(VisionArea visionArea) => Log(visionArea, @"log\lostlog");

        public string LogThatSuccess(VisionArea visionArea) => Log(visionArea, @"log\successlog");
        public string Log(VisionArea visionArea, string subFolder)
        {
            visionArea.AimName = visionArea.AimName.Replace("/", "");
            var relPath = ImgPathWorkerSingleton.GetInstance().imgPathes.relOutputFolderPath;
            var absoluteFolderPath = Path.Combine(Environment.CurrentDirectory, relPath);
            var todayFolderPath = Path.Combine(absoluteFolderPath, subFolder, DateTime.UtcNow.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(todayFolderPath);
            var elementNameFolderPath = Path.Combine(todayFolderPath, visionArea.AimName);
            Directory.CreateDirectory(elementNameFolderPath);

            var relCoords = visionArea.OriginAimCoords;
            relCoords.Y -= visionArea.OriginAreaCoords.Y;
            using (var graph = Graphics.FromImage(visionArea.AreaBitmap))
            {
                graph.DrawRectangle(new Pen(Brushes.Red, 3.0F), relCoords);
            }

            //saving area's Bitmap
            var fileName = visionArea.AimName + "-" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
            var filePath = Path.Combine(elementNameFolderPath, fileName);
            try 
            {
                visionArea.AreaBitmap.Save(filePath + ".jpg", ImageFormat.Jpeg);
            }
            catch (ExternalException ex)
            {
                if (visionArea.AreaBitmap != null)
                {
                    MySeriLogger.LogText(
                        "Ошибка GDI была успешно поймана: " + Environment.NewLine +
                        $"filepath: {filePath}.jpg" + Environment.NewLine +
                        $"AreaBitmap width: {visionArea.AreaBitmap.Size.Width}" + Environment.NewLine +
                        $"AreaBitmap height: {visionArea.AreaBitmap.Size.Height}" + Environment.NewLine +
                        ex.ToString()
                        );
                }
                else
                    MySeriLogger.LogText("visionArea.AreaBitmap == null!");

            }

            //saving area info
            string JSONString = JsonConvert.SerializeObject(visionArea, Formatting.Indented);
            File.WriteAllText(filePath + ".json", JSONString);

            //JsonResult = JsonSerializer.Serialize(wordsAnno, options);
            //TODO CHANGE
            //File.WriteAllText(@"D:\result.json", JsonResult);
            return fileName;
        }
    }
}
