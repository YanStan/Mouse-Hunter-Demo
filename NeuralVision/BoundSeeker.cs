using Google.Apis.Vision.v1.Data;
using MoreLinq;
using Mouse_Hunter.NeuralVision.GoogleOCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mouse_Hunter.NeuralVision
{
    public class BoundSeeker
    {
        public List<Rectangle> SearchLeftmostBounds(List<EntityAnnotation> annos)
        {
            if (annos != null)
            {
                var rC = new RectangleConverter(0);
                annos = annos.MinBy(x => x.BoundingPoly.Vertices[0].X).ToList();
                return annos.Select(x => rC.GetRectangleFromVertices(x.BoundingPoly.Vertices)).ToList();
            }
            else
                return null;
        }
        public List<Rectangle> SearchBoxesCoordsByDescription(List<EntityAnnotation> annos, string text, bool textContainsInWord,
            bool getOneMostSimilar = true)
        {
            if (annos != null)
            {
                List<EntityAnnotation> boxes;
                if (textContainsInWord)
                    boxes = annos.Where(x => ContainsMultiLang(x.Description, text) && x.Confidence == 1.0).ToList();
                else
                    boxes = annos.Where(x => EqualMultiLang(x.Description, text) && x.Confidence == 1.0).ToList();
                var maxBoxes = boxes.MaxBy(x => x.Confidence).ToList();
                if(getOneMostSimilar && maxBoxes.Count > 0 && maxBoxes[0].Topicality != null)
                    maxBoxes = maxBoxes.MaxBy(x => x.Topicality).ToList();
                List<Rectangle> allBounds = new List<Rectangle>();
                foreach (EntityAnnotation box in maxBoxes)
                {
                    if (box != null)
                    {
                        var vertices = box.BoundingPoly.Vertices;
                        var rC = new RectangleConverter(0);
                        if (vertices[0].X != null &&
                            vertices[0].Y != null &&
                            vertices[2].X != null &&
                            vertices[2].Y != null)
                            allBounds.Add(rC.GetRectangleFromVertices(vertices));
                    }
                }
                return allBounds;
            }
            return null;
        }

        public static bool ContainsMultiLang(string description, string innerStr)
        {
            bool result = description.Contains(innerStr);
            if(!result)
            {
                if (description.Length > innerStr.Length)
                {
                    result = description.IndexOf(innerStr, StringComparison.InvariantCulture) >= 0;
                    if (!result)
                    {
                        var mutStr = LangConverter.ConvertToCyrillic(innerStr);
                        var num = description.IndexOf(mutStr
                            , StringComparison.InvariantCulture);
                        result = num >= 0;
                        if (!result)
                        {
                            mutStr = LangConverter.ConvertToLatin(innerStr);
                            num = description.IndexOf(mutStr
                               , StringComparison.InvariantCulture);
                            result = num >= 0;
                        }
                    }
                }
                else
                    return false;
            }
            return result;
        }
        public static bool EqualMultiLang(string description, string innerStr)
        {
            bool result = description == innerStr;
            if (!result)
            {
                result = description.Equals(innerStr, StringComparison.InvariantCulture); //InvariantCultureIgnoreCase
                if (!result)
                {
                    result = description.Equals(
                        LangConverter.ConvertToCyrillic(innerStr), StringComparison.InvariantCulture);
                    if (!result)
                        result = description.Equals(
                            LangConverter.ConvertToLatin(innerStr), StringComparison.InvariantCulture);
                }
            }
            return result;
        }
    }
}
