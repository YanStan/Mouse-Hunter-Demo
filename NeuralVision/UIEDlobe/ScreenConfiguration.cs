using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipes;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class ScreenConfiguration
    {
        public NamedPipeServerStream PipeStream;
        public int SearchPanelMode;
        public ColorConfiguration colorConfig = new ColorConfiguration();
        public CropConfiguration cropConfig = new CropConfiguration();
        public string pythonArgs;
        public bool shouldChangeColor = false;
        public bool shouldCrop = false;
        public VisionArea VisArea;

        public ScreenConfiguration(NamedPipeServerStream pipeStream, int searchPanelMode)
        {
            PipeStream = pipeStream;
            SearchPanelMode = searchPanelMode;
        }
           
        public VisionArea Setup()
        {
            SetupInputImage();
            bool isInputMutated = shouldCrop && shouldChangeColor;
            SetupPythonArgs(isInputMutated);
            if (shouldCrop)
            {
                //TODO what if not should crop
                VisArea = new ScreenCropper().Crop(VisArea, cropConfig.cropPercentages, SearchPanelMode);
                cropConfig.cropStartPos.X += VisArea.OriginAreaCoords.X;
                cropConfig.cropStartPos.Y += VisArea.OriginAreaCoords.Y;
            }
            if (shouldChangeColor)
                VisArea = ChangeColor(VisArea, isInputMutated);


            //if VisArea != null, use it. Otherwise use right side
            VisArea = VisArea ?? new VisionArea
            {

            };
            VisArea = new PipeImageBoxParser().GetAnnosFromPipe(VisArea, PipeStream, pythonArgs);
            if (VisArea.AreaAnnos != null && (SearchPanelMode != 1 || shouldCrop))
                VisArea.AreaAnnos = cropConfig.FixCropAnnosToOriginal(VisArea.AreaAnnos, cropConfig.cropStartPos);

            return VisArea;
        }
        private void SetupInputImage()
        {
            switch (SearchPanelMode)
            {
                case 0:
                    cropConfig.cropStartPos.Y += 71;
                    break;
                case 1:
                    break;
                case 2:
                    cropConfig.cropStartPos.Y += 34;
                    break;
                case 3:
                    var cropStartPosY = Screen.PrimaryScreen.Bounds.Height - 40;
                    cropConfig.cropStartPos.Y += cropStartPosY;
                    break;
            }
        }
        private void SetupPythonArgs(bool IsInputMutated)
        {
            ImgPathWorkerSingleton i = new ImgPathWorkerSingleton();
            var mutatedFileName = i.GetRelativePath(1, true, fileNameOnly: true);
            var workFileName = i.GetRelativePath(SearchPanelMode, IsInputMutated, fileNameOnly: true);
            if (pythonArgs == null && SearchPanelMode != 0)
                pythonArgs = "10 50 30 False";
            if (shouldCrop || shouldChangeColor)
            {
                pythonArgs = $"{mutatedFileName} {SearchPanelMode}:" + pythonArgs;
            }
            else
                pythonArgs = $"{workFileName} {SearchPanelMode}:" + pythonArgs;
        }

        private VisionArea ChangeColor(VisionArea visionrea, bool isOriginalMutated)
        {
            visionrea.AreaBitmap = new ColorChanger().ChangeColors(colorConfig.changeAllAnotherColorsMode,
            colorConfig.rgb, colorConfig.tolerance,
            SearchPanelMode, isOriginalMutated);
            return visionrea;
        }
    }

    public class ColorConfiguration
    {
        public byte[] rgb;
        public bool changeAllAnotherColorsMode = true; 
        public int tolerance = 40;
    }
    public class CropConfiguration
    {
        public Point cropStartPos = new Point() { X = 0, Y = 0};
        public float[] cropPercentages;
        public List<EntityAnnotation> FixCropAnnosToOriginal(List<EntityAnnotation> annos, Point cropStartPos)
        {
            annos.ForEach(x =>
            {
                var v = x.BoundingPoly.Vertices;
                for (int i = 0; i < 4; i++)
                {
                    v[i].X += cropStartPos.X;
                    v[i].Y += cropStartPos.Y;
                }
            });
            return annos;
        }
    }
}
