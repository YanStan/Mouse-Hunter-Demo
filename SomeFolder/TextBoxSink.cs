using Mouse_Hunter.Extensions;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mouse_Hunter
{
    public class TextBoxSink : ILogEventSink
    {
        private readonly TextBox textBox;
        private readonly LogEventLevel minLevel;
        private readonly ITextFormatter formatter;

        public TextBoxSink(TextBox textBox)
        {
            string outputTemplate = "{NewLine}{Message:lj}{NewLine}";
            this.textBox = textBox;
            minLevel = LogEventLevel.Verbose;
            formatter = new MessageTemplateTextFormatter(outputTemplate);
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (logEvent.Level < minLevel) return;

            var sw = new StringWriter();
            formatter.Format(logEvent, sw);
            textBox.InvokeIfRequired(() => textBox.AppendText(sw.ToString()));
        }
    }
}
