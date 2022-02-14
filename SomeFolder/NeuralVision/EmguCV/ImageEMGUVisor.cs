using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using Mouse_Hunter.Repositories;
using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.NeuralVision.EmguCV
{
    public class ImageEMGUVisor
    {
        private RectangleConverter RectConverter;
        public ImageEMGUVisor(RectangleConverter rectConverter) => 
            RectConverter = rectConverter;

        public VisionArea WatchArea(bool isInputMutated, SearchArgsWrapper wrap, ref VisionArea area)
        {
            var i = ImgPathWorkerSingleton.GetInstance();
            var sourcePath = i.GetFullPath(wrap.panelN, isInputMutated);
            var templatePath = i.GetFullTemplatePath(wrap.path);
            var savePath = Path.Combine(Environment.CurrentDirectory, i.imgPathes.relOutputFolderPath, "LAST_OUTPUT.jpg");

            //Getting Template and screenshot
            var repos = new ImageRepository();
            Image<Bgr, byte> template = repos.GetEmguImage(templatePath);
            Image<Bgr, byte> source = repos.GetEmguImage(sourcePath);

            //Preparing for resursionMethod
            var origAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(wrap.panelN);
            Image<Bgr, byte> imageToSave = source.Copy();
            var subPathArr = wrap.path.Split('\\');
            var filename = subPathArr[subPathArr.Length - 1];
            var aimTitle = filename.Split('.')[0];

            //Watch many template objects on 1 screen by recursive method
            area = GetManyOccurencies(aimTitle, template, source, wrap.threshhold, area);
            var allPositions = area.AreaAnnos.Select(x =>
                RectConverter.GetRectangleFromVertices(x.BoundingPoly.Vertices)).ToArray();
            area.AimName = aimTitle;
            area.isAimFound = area.AreaAnnos.Count > 0;

/*            area.OriginAimCoords = allPositions[0];
            area.OriginAreaCoords = origAreaCoords;*/

            area.AreaBitmap = imageToSave.ToBitmap();

            //Draw all on screen
            foreach (Rectangle matchRect in allPositions)
            {
                imageToSave.Draw(matchRect, new Bgr(System.Drawing.Color.Red), 3);
            }
            repos.SaveEmguImage(imageToSave, savePath);
            return area;
        }

        private VisionArea GetManyOccurencies(string aimTitle, Image<Bgr, byte> template, Image<Bgr, byte> source,
            double threshhold, VisionArea visionArea, int counter = 0)
        {
            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                if (maxValues[0] >= threshhold && counter < 10)
                {
                    //If search very small gui with th = 1, we will find infinite obj even with th = 1
                    if(threshhold == 1)
                        counter++;
                    // Match success
                    var matchRect = new Rectangle(maxLocations[0], template.Size);
                    visionArea.AreaAnnos.Add(
                        new EntityAnnotation()
                        {
                            BoundingPoly = new BoundingPoly()
                            {
                                Vertices = RectConverter.GetVerticesFromRectangle(matchRect)
                            },
                            Confidence = 1.0f,
                            Topicality = (float?)maxValues[0],
                            Description = aimTitle
                        });
                    //masking
                    source.Draw(matchRect, new Bgr(System.Drawing.Color.White), -1);
                    visionArea = GetManyOccurencies(aimTitle, template, source, threshhold, visionArea, counter);
                    // do stuff with match
                    // etc..
                }
                return visionArea;
            }
        }
    }
}
