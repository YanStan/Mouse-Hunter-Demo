
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.NeuralVision.EmguCV;
using Mouse_Hunter.Wrappers;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Mouse_Hunter
{
    public static class MySeriLogger
    {
        public static void LogText(string text) => Log.Logger.Debug(text);

        public static bool LogTime(Predicate<int> method, 
            string preMessage = "Время, потраченное на регистрацию профиля: ",
            int repeatCount = 1)
        {
            bool result = true;
            for (int i = 0; i < repeatCount; i++)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                result = method(1);

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}",
                    ts.Hours, ts.Minutes, ts.Seconds);
                LogText(preMessage + elapsedTime);
            }
            return result;
        }
        public static void LogInfo(VisionArea visArea) =>
            Log.Logger.Debug(
                $"{visArea.AimName} найден: {visArea.isAimFound}" + Environment.NewLine +
                $"logfile: {visArea.lastAreaLogFile}"
                );
        public static void LogError(VisionArea visArea, string methodName)
        {
            Log.Logger.Debug(
                $"======================================================" + Environment.NewLine +
                "ERROR: " + Environment.NewLine +
                $"{visArea.AimName} найден: {visArea.isAimFound}" + Environment.NewLine +
                $"Событие: {methodName}" + Environment.NewLine +
                "logfile:" + Environment.NewLine +
                visArea.lastAreaLogFile
                );
        }
        public static void LogConnectionError(string methodName)
        {
            LogText("======================================================" + Environment.NewLine +
            "ERROR:" + Environment.NewLine +
            "ИНТЕРНЕТ СОЕДИНЕНИЕ НЕ ОБНАРУЖЕНО." + Environment.NewLine +
            "ПРОВЕРЬТЕ ПОДКЛЮЧЕНИЕ." + Environment.NewLine +
            $"Событие: {methodName}" + Environment.NewLine +
            DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"));
        }
    }
}
