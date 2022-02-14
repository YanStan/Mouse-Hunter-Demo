using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.ScenarioWorkers.SystemWorkers;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers
{
    public class FullRegistrator : AbstractClickerWorker
    {
        private AccountProfile profile;
        public FullRegistrator(NamedPipeServerStream pipeStream)
            : base(pipeStream) { }

        public bool DoAllTogether(int repeatCountIfProfileBroken = 1)
        {
            bool result = false;
            var allProfiles = LaunchCompositeWorker();
            if (allProfiles == null)
                return false;
            var activeProfiles = new AccountProfilesAnalyser(allProfiles)
                .GetAllActiveProfiles("Да")
                .ToList();
            foreach (AccountProfile profile in activeProfiles)
            {
                this.profile = profile;
                //true, if at least 1 is successful
                result = result | MySeriLogger.LogTime(
                    FullRegOneProfile, "Всё время, проведённое на сайте: ", repeatCountIfProfileBroken
                    );
            }
            return result;
        }

        private bool FullRegOneProfile(int maxCount = 2) => TryFullRegRecursive(maxCount);


        private bool TryFullRegRecursive(int maxCount, int tryCounter = 0)
        {
            if (!TryFullReg() && tryCounter < maxCount)
                return TryFullRegRecursive(maxCount, tryCounter + 1);
            else if (tryCounter >= 3)
                return false;
            return true;
        }

        private bool TryFullReg()
        {
            var registrator = new CatcasinoRegistrator(PipeStream);
            registrator.SetCurrentProfile(profile);
            if (!MySeriLogger.LogTime(
                registrator.TryToReg, "Время, потраченное на регистрацию профиля: "
                ))
                return false;

            if (!MySeriLogger.LogTime(
                new JustWalker(PipeStream).TryToWalk, "Время, потраченное на просмотр сайта: "
                ))
                return false;

            if (!MySeriLogger.LogTime(
                new SlotActivityMaker(PipeStream).TryToWalk, "Время, потраченное на игру во все слоты: "
                ))
                return false;
            return true;
        }
    }
}
