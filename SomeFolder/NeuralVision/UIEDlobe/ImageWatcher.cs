using Mouse_Hunter.NeuralVision.EmguCV;
using Mouse_Hunter.Repositories;
using Mouse_Hunter.Wrappers;
using System.Drawing;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class ImageWatcher
    {
        public int BrowserWindRightX;
        protected static bool IsBrowserMode;
        private static NamedPipeServerStream PipeStream;
        protected static ScreenMaker ScreenMaker;
        protected static RectangleConverter RectConverter;
        public ImageWatcher(bool isBrowserMode = true)
        {
            ScreenMaker = new ScreenMaker(Screen.PrimaryScreen.Bounds.Width, isBrowserMode);
            if (isBrowserMode)
            {
                ScreenMaker.MakeScreenshot();
                FindBrowserWindRightX();
                IsBrowserMode = isBrowserMode;
            }
            else
                BrowserWindRightX = Screen.PrimaryScreen.Bounds.Width;
            ScreenMaker = new ScreenMaker(BrowserWindRightX, IsBrowserMode);
            RectConverter = new RectangleConverter(BrowserWindRightX);
        }
        public void Initialise(NamedPipeServerStream pipeStream) => PipeStream = pipeStream;

        public void FindBrowserWindRightX()
        {
            //Getting all GUI objects in first 34 Y of screen
            var wrap = new SearchArgsWrapper()
            {
                path = @"Chrome\close_browser_icon.jpg",
                threshhold = 0.9,
                panelN = 1 
            };

            var closeBrowserArea = new EmguWatcher(false).GetArea(false, wrap);
            var allBounds = new BoundSeeker().SearchLeftmostBounds(closeBrowserArea.AreaAnnos);
            //looking for close_browser_window icon coords list
            if (allBounds != null && allBounds.Count != 0 && allBounds[0].Width != 0)
                BrowserWindRightX = allBounds[0].X + allBounds[0].Width + 20;
            else
                BrowserWindRightX = Screen.PrimaryScreen.Bounds.Width;
                Thread.Sleep(5);
        }

        public VisionArea GetAnnosFromImage(ScreenConfiguration sc)
        {
            ScreenMaker.MakeScreenshot();
            return sc.Setup();
        }

        public VisionArea GetAnnosFromImage(int searchPanelMode = 0, string pythonArgs = "10 50 30 False")//"10 5 600 False"
        {
            var sc = new ScreenConfiguration(PipeStream, searchPanelMode)
            {
                pythonArgs = pythonArgs,
                VisArea = new VisionArea()
                {
                    AreaBitmap = GetPanelBitmapByMode(searchPanelMode),
                    OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(searchPanelMode)
                }
            };
            ScreenMaker.MakeScreenshot();
            return sc.Setup();
        }
            
        public VisionArea GetAnnosFromImage(byte[] rgb, int searchPanelMode = 0)
        {
            var sc = new ScreenConfiguration(PipeStream, searchPanelMode)
            {
                shouldChangeColor = true,
                pythonArgs = "10 50 30 False",
                VisArea = new VisionArea()
                {
                    AreaBitmap = GetPanelBitmapByMode(searchPanelMode),
                    OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(searchPanelMode)
                }
            };
            sc.colorConfig.rgb = rgb;
            ScreenMaker.MakeScreenshot();
            return sc.Setup();
        }

        public VisionArea GetAnnosFromImage(float[] cropPercentages, int searchPanelMode = 0)
        {
            var sc = new ScreenConfiguration(PipeStream, searchPanelMode)
            {
                shouldCrop = true,
                pythonArgs = "10 50 30 False",
                VisArea = new VisionArea()
                {
                    OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(searchPanelMode)
                }
            };
            sc.cropConfig.cropPercentages = cropPercentages;
            ScreenMaker.MakeScreenshot();
            return sc.Setup();
        }

        public VisionArea GetAnnosFromImage(float[] cropPercentages, byte[] rgb, int searchPanelMode = 0)
        {
            var sc = new ScreenConfiguration(PipeStream, searchPanelMode)
            {
                shouldCrop = true,
                shouldChangeColor = true,
                pythonArgs = "10 50 30 False", //"10, 5, 600, False"
                VisArea = new VisionArea()
                {
                    OriginAreaCoords = RectConverter.GetPanelAreaRectFromPanelMode(searchPanelMode)
                }
            };
            sc.cropConfig.cropPercentages = cropPercentages;
            sc.colorConfig.rgb = rgb;
            ScreenMaker.MakeScreenshot();
            return sc.Setup();
        }

        protected static Bitmap GetPanelBitmapByMode(int panelMode)
        {
            ImageRepository repos = new ImageRepository();
            var image = repos.GetImage(panelMode);
            return (Bitmap)image;
        }

        protected static bool CropIfShould(ref VisionArea visionArea, SearchArgsWrapper wrap)
        {
            //If we should crop
            bool shouldCrop = wrap.originCropCoords != null || wrap.cropPercents != null;
            if (shouldCrop)
            {
                //string relMutatedFilePath = ImgPathWorkerSingleton.GetInstance().imgPathes.mutatedScreenFileName;
                if (wrap.originCropCoords != null)
                    visionArea = new ScreenCropper().CropMainScreen(visionArea, wrap.originCropCoords);
                if (wrap.cropPercents!= null)
                    visionArea = new ScreenCropper().Crop(visionArea, wrap.cropPercents, wrap.panelN);
            }
            else
                visionArea.AreaBitmap = GetPanelBitmapByMode(wrap.panelN);
            return shouldCrop;
        }
        public VisionArea FixCropAnnosToOriginal(VisionArea area)
        {
            if (area.AreaAnnos != null)
                area.AreaAnnos.ForEach(x =>
                {
                    var v = x.BoundingPoly.Vertices;
                    for (int i = 0; i < 4; i++)
                    {
                        v[i].X += area.OriginAreaCoords.X;
                        v[i].Y += area.OriginAreaCoords.Y;
                    }
                });
            return area;
        }
    }
}
