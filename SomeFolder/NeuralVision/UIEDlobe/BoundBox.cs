using Google.Apis.Vision.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class BoundBox
    {
        public string description { get; set; }
        public string image_label { get; set; }
        public float confidence { get; set; }
        public int[] coords { get; set; }

        public EntityAnnotation ConvertThisBoundBoxToAnno() =>
           BuildAnnoFromArgs(description, image_label, confidence, coords);


        public EntityAnnotation BuildAnnoFromArgs(string description, string imageLabel, float confidence, int[] coords)
        {
            var box = new EntityAnnotation()
            {
                Description = description,
                ETag = imageLabel,
                Confidence = confidence,
                BoundingPoly = new BoundingPoly()
                {
                    Vertices = new List<Vertex>()
                    {
                        new Vertex()
                        {
                            X = coords[0],
                            Y = coords[1]
                        },
                        new Vertex()
                        {
                            X = coords[2],
                            Y = coords[1]
                        },
                        new Vertex()
                        {
                            X = coords[2],
                            Y = coords[3]
                        },
                        new Vertex()
                        {
                            X = coords[0],
                            Y = coords[3]
                        },
                    }              
                }
            };
            return box;
        }
    }
}
