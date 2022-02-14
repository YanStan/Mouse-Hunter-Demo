using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.NeuralVision.UIEDlobe
{
    public class ImgPathWorkerSingleton
    {
        public ImgPathes imgPathes { get; private set; }

        private static ImgPathWorkerSingleton instance;
        public static ImgPathWorkerSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new ImgPathWorkerSingleton();
                SetImgPathConfigToInstance();
            }
            return instance;
        }
        private static void SetImgPathConfigToInstance()
        {
            var fullPathToJson = Path.Combine(Environment.CurrentDirectory, @"..\..\img_pathes.json");
            string json = File.ReadAllText(fullPathToJson);
            instance.imgPathes = JsonConvert.DeserializeObject<ImgPathes>(json);
        }


        public string GetFullPath(int panelMode, bool isMutated)
        {
            var relPath = GetRelativePath(panelMode, isMutated);
            return Path.Combine(Environment.CurrentDirectory, relPath);
        }
        public string GetFullTemplatePath(string subPathToFile)
        {
            string fPath = instance.imgPathes.relInputFolderPath;
            return Path.Combine(Environment.CurrentDirectory, fPath, "Templates", subPathToFile);
        }
        public string GetRelativePath(int panelMode, bool isMutated, bool fileNameOnly = false)
        {
            string fPath = instance.imgPathes.relInputFolderPath;
            string relAreaFilePath = instance.imgPathes.screenFileName;
            if (isMutated)
            {
                relAreaFilePath = instance.imgPathes.mutatedScreenFileName;
            }
            else
            {
                switch (panelMode)
                {
                    case 1:
                        relAreaFilePath = instance.imgPathes.fstPanelFileName;
                        break;
                    case 2:
                        relAreaFilePath = instance.imgPathes.sndPanelFileName;
                        break;
                    case 3:
                        relAreaFilePath = instance.imgPathes.thdPanelFileName;
                        break;
                }
            }
            if (!fileNameOnly)
                relAreaFilePath = fPath + relAreaFilePath;
            return relAreaFilePath;
        }
    }

    public class ImgPathes
    {
        public string relInputFolderPath;
        public string relOutputFolderPath;
        public string screenFileName;
        public string mutatedScreenFileName;
        public string fstPanelFileName;
        public string sndPanelFileName;
        public string thdPanelFileName;
    }
}
