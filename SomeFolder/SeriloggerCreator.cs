using Mouse_Hunter.NeuralVision.UIEDlobe;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_Hunter
{
    public static class SeriloggerCreator
    {
        public static void CreateWithSinkTextbox(TextBox textbox)
        {   //
            var relPath = ImgPathWorkerSingleton.GetInstance().imgPathes.relOutputFolderPath;
            var absoluteFolderPath = Path.Combine(Environment.CurrentDirectory, relPath, @"log\tracelog");
            var filePath = Path.Combine(absoluteFolderPath, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose() // ставим минимальный уровень в Verbose для теста, по умолчанию стоит Information 
                .WriteTo.Sink(new TextBoxSink(textbox))  // выводим данные v textBox
                .WriteTo.File(filePath, outputTemplate:
                "{Message:lj}{NewLine}{Exception}{NewLine}") // а также пишем лог файл, разбивая его по дате
                // есть возможность писать Verbose уровень в текстовый файл, а например, Error в Windows Event Logs
                .CreateLogger();
  
        }
    }
}
