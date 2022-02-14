using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using Mouse_Hunter.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows.Forms;

namespace Mouse_Hunter.NeuralVision.GoogleOCR
{
    public class GoogleOCRVisor
    {
        public string ApplicationName { get { return "Ocr"; } }
        //public string JsonResult { get; set; }
        //public string[] TextResult { get; set; }
        public string Error { get; set; }

        private string JsonKeypath
        {
            //get { return Application.StartupPath + "\\your file name.json"; }
            get { return Path.Combine(Environment.CurrentDirectory, @"..\..", "key.json"); }
        }

        private GoogleCredential _credential;
        private GoogleCredential CreateCredential()
        {
            if (_credential != null) return _credential;
            using (var stream = new FileStream(JsonKeypath, FileMode.Open, FileAccess.Read))
            {
                string[] scopes = { VisionService.Scope.CloudPlatform };
                var credential = GoogleCredential.FromStream(stream);
                credential = credential.CreateScoped(scopes);
                _credential = credential;
                return credential;
            }
        }

        private VisionService CreateService(GoogleCredential credential)
        {
            var service = new VisionService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                GZipEnabled = true,
            });
            return service;
        }
        public List<EntityAnnotation> GetOCRWords(int panelMode, bool IsInputMutated, string language)
        {
            ImageRepository repos = new ImageRepository();
            var iW = ImgPathWorkerSingleton.GetInstance();
            var filePath = iW.GetFullPath(panelMode, IsInputMutated);
            byte[] file = File.ReadAllBytes(filePath);
            var credential = CreateCredential();
            var service = CreateService(credential);
            service.HttpClient.Timeout = new TimeSpan(1, 1, 1);

            BatchAnnotateImagesRequest batchRequest = new BatchAnnotateImagesRequest();
            batchRequest.Requests = new List<AnnotateImageRequest>();
            batchRequest.Requests.Add(new AnnotateImageRequest()
            {
                Features = new List<Feature>() { new Feature() { Type = "DOCUMENT_TEXT_DETECTION", MaxResults = 1 }, },
                ImageContext = new ImageContext() { LanguageHints = new List<string>() { language } },
                Image = new Image() { Content = Convert.ToBase64String(file) }
            });

            var annotate = service.Images.Annotate(batchRequest);
            BatchAnnotateImagesResponse batchAnnotateImagesResponse = annotate.Execute();
            if (batchAnnotateImagesResponse.Responses.Any())
            {
                AnnotateImageResponse annotateImageResponse = batchAnnotateImagesResponse.Responses[0];
                if (annotateImageResponse.Error != null)
                {
                    if (annotateImageResponse.Error.Message != null)
                        Error = annotateImageResponse.Error.Message;
                }
                else
                {
                    if (annotateImageResponse.TextAnnotations != null && annotateImageResponse.TextAnnotations.Any())
                    {
                        //TextResult = annotateImageResponse.TextAnnotations[0].Description.Replace("\n", "\r\n");
                        var wordsAnno = annotateImageResponse.TextAnnotations.Where(x => x.Description.Length > 1).ToList();
                        wordsAnno.RemoveAt(0);
                        for (int i = 0; i < wordsAnno.Count(); i++)
                        {   
                            while (i < wordsAnno.Count() - 1 &&
                                wordsAnno[i].BoundingPoly.Vertices[1].X != null &&
                                wordsAnno[i + 1].BoundingPoly.Vertices[0].X != null &&
                                wordsAnno[i].BoundingPoly.Vertices[1].Y != null &&
                                wordsAnno[i + 1].BoundingPoly.Vertices[0].X != null &&
                                //while space between boxes.X less than 15 px
                                Math.Abs((decimal)
                                    (wordsAnno[i].BoundingPoly.Vertices[1].X - wordsAnno[i + 1].BoundingPoly.Vertices[0].X)) < 15 &&
                                //while space between boxes.Y less than 7 px
                                Math.Abs((decimal)
                                    (wordsAnno[i].BoundingPoly.Vertices[1].Y - wordsAnno[i + 1].BoundingPoly.Vertices[0].Y)) < 7
                                )
                            {
                                //merge text
                                wordsAnno[i].Description += $" {wordsAnno[i + 1].Description}";
                                //merge boxes
                                wordsAnno[i].BoundingPoly.Vertices[1] = wordsAnno[i + 1].BoundingPoly.Vertices[1];
                                wordsAnno[i].BoundingPoly.Vertices[2] = wordsAnno[i + 1].BoundingPoly.Vertices[2];
                                wordsAnno.RemoveAt(i + 1);
                            }
                            wordsAnno[i].Confidence = 1;
                        }
                        var options = new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                            WriteIndented = true
                        };
                        //TextResult = wordsAnno[0].Description.Split('\n');
                        //JsonResult = JsonSerializer.Serialize(wordsAnno, options);
                        //TODO CHANGE
                        //File.WriteAllText(@"D:\result.json", JsonResult);
                        return wordsAnno;
                    }
                    return null;
                }
            }
            return null;
            //return TextResult;

        }
    }
}
