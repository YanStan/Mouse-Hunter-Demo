using Mouse_Hunter.Resources.AntiCaptcha;
using Mouse_Hunter.Clickers;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Mouse_Hunter.NeuralVision;
using System.IO.Pipes;
using Mouse_Hunter.NeuralVision.GoogleOCR;
using Mouse_Hunter.NeuralVision.EmguCV;
using Serilog;

namespace Mouse_Hunter.ScenarioWorkers.SystemWorkers
{
    public class AbstractClickerWorker
    {
        protected NamedPipeServerStream PipeStream;
        protected string FullPath;
        protected Random random;
        protected RectangleConverter RectConverter;

        protected BoundingBoxClicker boxClicker;
        protected Clicker clicker;
        protected TextWatcher textWatcher;
        protected EmguWatcher imgWatcher;
        protected UniversalGUIClicker uniClicker;
        protected BotActionFinisher Baf;

        public AbstractClickerWorker(NamedPipeServerStream pipeStream)
        {
            var path = "SikuliImages";
            FullPath = Path.GetFullPath(path);
            PipeStream = pipeStream;
            random = new Random();
            RectConverter = new RectangleConverter(0);

            var mover = new MouseOperator(1, 3000);
            clicker = new Clicker(mover);
            boxClicker = new BoundingBoxClicker(mover);
        }
        public void Initialise(TextWatcher textWatcher = null, EmguWatcher imgWatcher = null,
            UniversalGUIClicker uniClicker = null, BotActionFinisher Baf = null,
            bool isBrowserMode = true)
        {        
            this.textWatcher = textWatcher ?? new TextWatcher(isBrowserMode);
            this.imgWatcher = imgWatcher ?? new EmguWatcher(isBrowserMode);
            this.uniClicker = uniClicker ?? new UniversalGUIClicker(isBrowserMode);
            this.Baf = Baf ?? new BotActionFinisher(this.uniClicker);
            Log.Logger.Debug($"Start {GetType().Name}");
        }

        public IEnumerable<AccountProfile> LaunchCompositeWorker()
        {
            //set land to En
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;


            var reposOfSheets = new GoogleSheetsRepository("1eAUgvNhsBh7aKcTQRBPM-CgsDuE46Sx3HV8d8RFVwpQ");
            var sheetRows = reposOfSheets.TryGetRowsRecursive("Авторегистратор!A2:U");
            if (sheetRows.Count == 0)
                return null;

            var allProfiles = sheetRows.Where(x => x.Count >= 16).Select(x => new AccountProfile(x));
            return allProfiles;

            //sikuli part
            //APILauncher launcher = new APILauncher(true);
            //launcher.Start();

        }



        protected static void ChooseElementInDropList(int count)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(70, 300));
            for (int i = 0; i < count; i++)
            {
                SendKeys.SendWait("{DOWN}");
                Thread.Sleep(random.Next(70, 300));
            }
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(random.Next(70, 300));
        }
        protected static bool GoThroughRecaptcha2Proxyless(IProgress<string> progress, string siteUrl, string sitekey)
        {
            CaptchaSender sender = new CaptchaSender();
            string token = sender.DoRecaptcha2Proxyless(siteUrl, sitekey);
            if (token != null)
            {
                PasteF12ReCaptcha2Data(token);
                progress.Report("Recaptcha2 пройдена!\r\n");
                return true;
            }
            else
            {
                progress.Report("Сервису Антикапча не удалось пройти Recaptcha2!\r\n");
                return false;
            }
        }
        protected static void GoThroughHCatchaProxyless(IProgress<string> progress, string siteUrl, string sitekey)
        {
            CaptchaSender sender = new CaptchaSender();
            string token = sender.DoHCaptchaProxyless(siteUrl, sitekey);
            if (token != null)
            {
                PasteF12HCaptchaData(token);
                progress.Report("h-captcha пройдена!\r\n");
            }
            else
            {
                progress.Report("Сервису Антикапча не удалось пройти h-captcha!\r\n");
            }
        }

        //document.querySelector('[id^="h-captcha-response"]')
        protected static void PasteF12ReCaptcha2Data(string token)
        {
            Random random = new Random();
            PasteTextFromBuffer(random,
                $"document.getElementsByClassName('g-recaptcha-response')[0].innerHTML='{token}'");
            Thread.Sleep(random.Next(600, 800));
            PasteTextFromBuffer(random,
                $"___grecaptcha_cfg.clients[0].S.S.callback('{token}')");//G.G//B.B
        }
        protected static void PasteF12HCaptchaData(string token)
        {
            Random random = new Random();
            PasteTextFromBuffer(random,
                $"document.querySelector('[id^=\"h-captcha-response\"]').innerHTML='{token}'");
            Thread.Sleep(random.Next(600, 800));
            PasteTextFromBuffer(random,
                $"document.querySelector('[id^=\"g-recaptcha-response\"]').innerHTML='{token}'");
            Thread.Sleep(random.Next(600, 800));
            PasteTextFromBuffer(random,
                $"document.querySelector('[class^=\"rui-Button-button\"]').removeAttribute(\"disabled\")");
            
        }

        protected static void PasteTextFromBuffer(Random random, string text)
        {
            Clipboard.SetData(DataFormats.Text, text);
            SendKeys.SendWait("^{v}");
            Thread.Sleep(random.Next(450, 550));
            SendKeys.SendWait("{ENTER}");
        }
    }
}
