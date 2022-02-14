using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using System;
using System.IO.Pipes;
using System.Linq;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class SlotСhooser : AbstractClickerWorker
    {
        public string searchOption;
        private string[] allSlotsArr = new string[]
        {
                "Infectious",
                "Gunfight",
                "Lady_Cat",
                "Sweet_Bonanza",
                "Gates_of_Olympus",
                "Book_Of_Fallen",
                "Book_Of_Dead",
                "Book_of_Wizard", "Queen_Of_The_Sun",
                "Minotaurus", "Hell_Hot",
                "Book_of_Shadows",

                "Coins_Of_Fortune", "Dead_Or_Alive",
                "Book_Of_Sun_Choice", "Majestic_King"
        };
        public SlotСhooser(NamedPipeServerStream pipeStream)
            : base(pipeStream) { }

        public int CountAllSlots() => allSlotsArr.Length;
        public string GetRandSlotName()
        {
            var slotName = allSlotsArr[random.Next(0, allSlotsArr.Length)];
            MySeriLogger.LogText($"Выбран слот: {slotName}");
            return slotName;
        }

        public SlotPlayer FormPlayerBySlotName(string slotName)
        {
            var mainArr = new string[]
            {
                "Infectious", "Gunfight", "Book_Of_Dead", "Book_Of_Fallen",
                "Lady_Cat", "Sweet_Bonanza",
                "Gates_of_Olympus",      "Coins_Of_Fortune", "Dead_Or_Alive"
            };
            var bCongoArr = new string[]
            {
                "Book_of_Wizard", "Queen_Of_The_Sun",
                                        "Book_Of_Sun_Choice", "Majestic_King"
            };
            var endorphinArr = new string[] { "Minotaurus", "Hell_Hot" };
            var shadowArr = new string[] { "Book_of_Shadows" }; 

            var player = new SlotPlayer("CatCasino", searchOption, PipeStream);
            player.Initialise(textWatcher, imgWatcher, uniClicker);

            if (mainArr.Contains(slotName))
                player.AddSettings(true, true, false, false);
            if (bCongoArr.Contains(slotName))
                player.AddSettings(true, true, false, true);
            if (endorphinArr.Contains(slotName))
                player.AddOpenCloseSameCoords();
            if (shadowArr.Contains(slotName))
                player.AddSettings(true, true, true, false);

            player.SetSlotName(slotName);
            return player;
        }
    }
}
