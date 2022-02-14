using Google.Apis.Vision.v1.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class PipeImageBoxParser
    {
        public VisionArea GetAnnosFromPipe(VisionArea visualArea, NamedPipeServerStream pipeStream, string pythonArgs)
        {
            var jsonString = GetJsonFromPipe(pipeStream, pythonArgs);
            if (jsonString != "")
            {
                var boxes = GetBoundBoxesFromJson(jsonString);
                var annos = GetAnnosFromBoxes(boxes);
                visualArea.AreaAnnos = annos;
            }
            return visualArea;
        }

        public string GetJsonFromPipe(NamedPipeServerStream pipeStream,
            string pythonArgs)
        {
            //if pythonArgs != null, use it. Otherwise use right side
            //pythonArgs = pythonArgs ?? new string[] { "PYTHON ARGS:" };
            WriteStrToPipe(pipeStream, pythonArgs);
            string jsonResponse = ReadStrFromPipe(pipeStream);
            return jsonResponse;
        }

        private static void WriteStrToPipe(NamedPipeServerStream server, string myString)
        {
            //TODO split string
            byte[] bytes = Encoding.Default.GetBytes(myString);
            myString = Encoding.UTF8.GetString(bytes);
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(myString);
                server.Write(stream.ToArray(), 0, stream.ToArray().Length);
            }
        }
        private static string ReadStrFromPipe(NamedPipeServerStream pipeStream)
        {
            var buffer = new byte[131072];
            var readBytes = pipeStream.Read(buffer, 0, buffer.Length);
            var receivedStr = Encoding.UTF8.GetString(buffer.Take(readBytes).ToArray());
            //Console.WriteLine("Received responce from python: " + receivedStr);
            return receivedStr;
        }

        public List<BoundBox> GetBoundBoxesFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<BoundBox[]>(jsonString).ToList();
        }
        public List<EntityAnnotation> GetAnnosFromBoxes(List<BoundBox> boundBoxes) =>
            boundBoxes.Select(x => x.ConvertThisBoundBoxToAnno()).ToList();
    }

    /*        static readonly object MyLockerObject = new object();
        static bool MustGo;*/

    //TODO change to .json at the end!!!
    /*        public List<EntityAnnotation> GetAnnosAfterJsonCreated(string jsonRelFolderPath, string jsonFileName = "splits.json")
            {
                var relPathtoJsonFile = Path.Combine(jsonRelFolderPath, jsonFileName);
                var fullPathtoJsonFile = Path.Combine(Environment.CurrentDirectory, @"..\..", relPathtoJsonFile);
                var fullPathtoJsonFolder = Path.Combine(Environment.CurrentDirectory, @"..\..", jsonRelFolderPath);
                CreateFileWatcher(fullPathtoJsonFolder);

                //new Thread(MonitorExistance).Start();
                List<EntityAnnotation> annos = null; // Used to store the return value
                var thread = new Thread(() => {
                    annos = MonitorAnnosAfterJsonCreated(fullPathtoJsonFile);
                });
                thread.Start();
                thread.Join();
                return annos;
            }*/

    /*        private static void CreateFileWatcher(string fullPathtoJsonFolder)
            {
                FileSystemWatcher fw = new FileSystemWatcher(fullPathtoJsonFolder);
                fw.NotifyFilter = NotifyFilters.Attributes
                         | NotifyFilters.CreationTime
                         | NotifyFilters.DirectoryName
                         | NotifyFilters.FileName
                         | NotifyFilters.LastAccess
                         | NotifyFilters.LastWrite
                         | NotifyFilters.Security
                         | NotifyFilters.Size;
                fw.Created += new FileSystemEventHandler(UnlockThread);
                fw.Changed += new FileSystemEventHandler(UnlockThread);
                fw.Renamed += new RenamedEventHandler(UnlockThread);
                fw.InternalBufferSize = 65536;
                fw.Filter = "*.*";
                fw.EnableRaisingEvents = true;
            }*/

    /*        private List<EntityAnnotation> MonitorAnnosAfterJsonCreated(string fullPathtoJsonFile)
            {
                //locking in the beginning and waiting
                lock (MyLockerObject)
                {
                    while (!MustGo)
                        Monitor.Wait(MyLockerObject);
                }
                //action when unlocked//PARSING JSON//
                return GetEntityAnnosFromBoxes(GetBoundBoxesFromJson(fullPathtoJsonFile));
            }*/
    /* private static void UnlockThread(object sender, FileSystemEventArgs e)
     {
         //Unlocking
         lock (MyLockerObject)
         {
             MustGo = true;
             Monitor.Pulse(MyLockerObject);
         }
     }*/
}
