using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.NeuralVision.EmguCV;
using Mouse_Hunter.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter
{
    public class BotActionFinisher
    {
        public UniversalGUIClicker univClicker;
        public BotActionFinisher(UniversalGUIClicker univClicker) =>
            this.univClicker = univClicker;

        public VisionArea Finish(string eventName,
            SearchArgsWrapper wrap,
            int preClickDelay = 2, int preCycleDelay = 0, bool shouldLogError = true)
        {
            var area = univClicker.WaitNClick(wrap, preCycleDelay, preClickDelay);
            //LOGGING
            if (!area.isAimFound && shouldLogError)
                MySeriLogger.LogError(area, eventName);
            return area;
        }
        public VisionArea PasteNFinish(string eventName,
            SearchArgsWrapper wrap, string input, 
            int preClickDelay = 2, int[] offsetCoords = null, int preCycleDelay = 0, int clickCount = 1)
        {
            var area = univClicker.WaitNPaste(
                wrap.shouldBitmapLive, wrap, input, preClickDelay, offsetCoords, preCycleDelay, clickCount
                );
            //LOGGING
            if (!area.isAimFound)
                MySeriLogger.LogError(area, eventName);
            return area;
        }
    }
}
