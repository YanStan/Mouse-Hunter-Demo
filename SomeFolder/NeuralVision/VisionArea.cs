using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.Repositories;
using System.Collections.Generic;
using System.Drawing;

namespace Mouse_Hunter.NeuralVision
{
    public class VisionArea
    {
        public string AimName;
        public bool isAimFound;
        public Rectangle OriginAimCoords;
        public Rectangle OriginAreaCoords; //
        public Bitmap AreaBitmap; //
        public List<EntityAnnotation> AreaAnnos; //
        public string lastAreaLogFile;

        public void Log() 
        {
            AreaLogger logger = new AreaLogger();
            switch (isAimFound)
            {
                case true:
                    lastAreaLogFile = logger.LogThatSuccess(this);
                    break;
                case false:
                    lastAreaLogFile = logger.LogThatNotFound(this);
                    break;
            }
        }
    }
}
