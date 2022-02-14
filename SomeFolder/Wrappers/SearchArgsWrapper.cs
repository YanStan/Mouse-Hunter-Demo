
namespace Mouse_Hunter.Wrappers
{
    public class SearchArgsWrapper
    {
        public string path;
        public string text;
        public int panelN;
        public float[] cropPercents;
        public int[] originCropCoords;
        public double threshhold;
        public bool textContainsInWord;
        public int elNum;
        public int preDelayPower;
        public bool moveWhileDelay;
        public int clickCount;
        public bool shouldBitmapLive;
        public bool shouldMoveToAim;
        public int maxWaitCount;
        public bool randOccurance;
        public bool getBiggestEl;
        public bool getOneMostSimilar;
        public bool shouldPressEnter;
        public bool pasteWithBackspace;
        //public SearchArgsWrapper instanse;
        public SearchArgsWrapper()
        {
            panelN = 0;
            threshhold = 0.6;
            textContainsInWord = false;
            elNum = 1;
            preDelayPower = 2;
            moveWhileDelay = true;
            clickCount = 1;
            shouldBitmapLive = false;
            maxWaitCount = 10;
            shouldMoveToAim = true;
            randOccurance = false;
            getBiggestEl = true;
            getOneMostSimilar = true;
            shouldPressEnter = true;
            pasteWithBackspace = false;

        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
