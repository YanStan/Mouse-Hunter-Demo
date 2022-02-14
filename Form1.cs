using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mouse_Hunter.AccountsSheetModels;
using Mouse_Hunter.Clickers;
using Mouse_Hunter.MovementSimulators;
using Mouse_Hunter.NeuralVision;
using Mouse_Hunter.NeuralVision.EmguCV;
using Mouse_Hunter.NeuralVision.GoogleOCR;
using Mouse_Hunter.NeuralVision.UIEDlobe;
using Mouse_Hunter.ScenarioWorkers.BrowserWorkers.CatCasinoWorkers;
using Serilog;
using Serilog.Events;

namespace Mouse_Hunter
{

    public partial class Form1 : Form
    {
        private Button button1;
        private TextBox textBox1;
        private Button button3;
        private Button button2;
        private Button button4;
        private NamedPipeServerStream PipeStream;
        public Form1()
        {
            //var connector = PythonPipesConnector.GetInstance();
            //PipeStream = connector.ConnectToPUIED();
            InitializeComponent();
            Shown += Form1_Shown;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
     
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            SeriloggerCreator.CreateWithSinkTextbox(textBox1);
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 67);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(249, 320);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(186, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Play Slots";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "JustWalk";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 41);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(249, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "All together";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(282, 402);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public void AppendText(string text) => textBox1.AppendText(text + Environment.NewLine);


        //TODO NO BUSINESS LoGIC IN FORM!!
        private void LogTaskIfFailed(Predicate<int> method, string preMessage, int repeatCount)
        {
            Task.Run(() =>
                MySeriLogger.LogTime(method, preMessage, repeatCount)
            ).ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    MySeriLogger.LogText(t.Exception.ToString());
            });


        }
        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
       

       

        /////////////////////////////////VISUAL ELEMENTS//////////////////////////////////////
        
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) =>
           LogTaskIfFailed(new CatcasinoRegistrator(PipeStream).TryToReg,
               "Время, потраченное на регистрацию профиля: ", 10);

        private void button2_Click_1(object sender, EventArgs e) =>
            LogTaskIfFailed(new JustWalker(PipeStream).TryToWalk,
                "Время, потраченное на просмотр сайта: ", 10);

        private void button3_Click(object sender, EventArgs e) =>
            LogTaskIfFailed(new SlotActivityMaker(PipeStream).TryToWalk,
                "Время, потраченное на игру во все слоты: ", 10);

        private void button4_Click(object sender, EventArgs e) =>
            LogTaskIfFailed(new FullRegistrator(PipeStream).DoAllTogether,
                "Все профили выполнены за время: ", 1);
        //TODO no business logic in form!
        













        /*            


                       if (pipeStream != null)
                           textBox1.AppendText("pipeStream.IsConnected: " + pipeStream.IsConnected + Environment.NewLine);
                       ImageWatcher watcher = new ImageWatcher(pipeStream, true);

                       var snd_pan_annos = watcher.GetAnnosFromImage(2, "10 50 30 False");

                       var answer = boxClicker.GoToTitle(snd_pan_annos, "address_bar", 1, true);
                       textBox1.AppendText(answer + Environment.NewLine);

                       answer = boxClicker.GoToTitle(snd_pan_annos, "refresh_page", 1);
                       textBox1.AppendText(answer + Environment.NewLine);

                       var fst_pan_annos = watcher.GetAnnosFromImage(1, "10 50 30 False");

                       answer = boxClicker.GoToTitle(fst_pan_annos, "close_browser_window", 1);
                       textBox1.AppendText(answer + Environment.NewLine);

           */
        /*
                    var answer = boxClicker.GoToTitle(GUICropannos, "add_tab", 1);
                    textBox1.AppendText(answer + Environment.NewLine);

                    answer = boxClicker.GoToTitle(GUICropannos, "close_browser_window", 1);
                    textBox1.AppendText(answer + Environment.NewLine);

                    answer = boxClicker.GoToTitle(GUICropannos, "close_tab", 1);
                    textBox1.AppendText(answer + Environment.NewLine);

                    answer = boxClicker.GoToTitle(GUICropannos, "maximize_window", 1);
                    textBox1.AppendText(answer + Environment.NewLine);

                    answer = boxClicker.GoToTitle(GUICropannos, "minimize_window", 1);
                    textBox1.AppendText(answer + Environment.NewLine);

                    answer = boxClicker.GoToTitle(GUICropannos, "reduce_window", 1);
                    textBox1.AppendText(answer + Environment.NewLine);*/
        /*            
        var GUICropannos = watcher.GetAnnosFromImage(pipeStream, cropPercentages);
        var answer = boxClicker.GoToTitle(GUICropannos, "reuse_slot_biggreen_btn", 2);
        textBox1.AppendText(answer + Environment.NewLine);

        var GUIannos = watcher.GetAnnosFromImage(new ScreenConfiguration(pipeStream));
        answer = boxClicker.GoToTitle(GUIannos, "reuse_slot_biggreen_btn", 2);
        textBox1.AppendText(answer + Environment.NewLine);*/

        /*            float[] cropPercentages = new float[] { 60, 50, 80, 70 };
                    byte[] rgb_reuse_btn = new byte[] { 68, 200, 90 };*/
        /*            byte[] rgb_reuse_btn = new byte[] { 68, 200, 90 };
                    var reuse_btn_GUIannos = watcher.GetAnnosFromImage(pipeStream, rgb_reuse_btn, true);
                    var answer = boxClicker.GoToTitle(reuse_btn_GUIannos, "reuse_slot_biggreen_btn", 2);
                    textBox1.AppendText(answer + Environment.NewLine);*/



        // @"GUIEDP\data\output\splits\splitjson" 
        /*            answer = boxClicker.GoToTitle(GUIannos, "minimize_window", 2);
        textBox1.AppendText(answer + Environment.NewLine);*/
        /*          answer = boxClicker.GoToTitle(GUIannos, "yellow_dollar", 2);   
                    textBox1.AppendText(answer + Environment.NewLine);*/

        /*            answer = boxClicker.GoToTitle(GUIannos, "reduce_window", 2);
                    textBox1.AppendText(answer + Environment.NewLine);
                    answer = boxClicker.GoToTitle(GUIannos, "add_tab", 2);
                    textBox1.AppendText(answer + Environment.NewLine);*/

        /*        private void button3_Click(object sender, EventArgs e) //open browser
                {
                    var browser = new BrowserCreator().Create();
                    //SelenInstructor = new SeleniumSerferInstructor(browser);
                }
                private void button2_Click(object sender, EventArgs e) => //close browser
                    //SelenInstructor.Quit();

                private void button4_Click(object sender, EventArgs e) // Симулятор пользовательской регистрации в Министерстве
                {
                    Task.Run(() =>
                    {
                        for (int i = 0; i < 17; i++)
                            //SelenInstructor.RegistrateOnHappyMin(Progress);
                    });
                }*/


        /*            Browser.Navigate().GoToUrl("https://browserleaks.com/webgl");
                    textBox1.AppendText("Бот перешел на сайт проверки.\r\n");

                    IJavaScriptExecutor js = (IJavaScriptExecutor)Browser;
                    js.ExecuteScript("window.key = \"serg_sienk\";");

                    Random random = new Random();
                    var delayMs = random.Next(15000, 25200);
                    Thread.Sleep(delayMs);

                    for (int i = 0; i < 0; i++)
                    {
                        delayMs = random.Next(2000, 5200);
                        Thread.Sleep(delayMs);
                        Browser.FindElement(By.CssSelector("._1cnjm")).Click();
                    }*/
    }

    /*        private async void button5_ClickAsync(object sender, EventArgs e) //Multilogin test
            {
                //TODO replace with existing profile ID. Define the ID of the browser profile, where the code will be executed.
                string profileId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

                //Define DesiredCapabilities
                //DriverOptions options = new DriverOptions();
                DesiredCapabilities dc = new DesiredCapabilities();
                BrowserProfile bp = new BrowserProfile();
                //Instantiate the Remote Web Driver to connect to the browser profile launched by startProfile method
                RemoteWebDriver driver = new RemoteWebDriver(new Uri(await bp.StartProfile(profileId)), dc);
                //Perform automation
                driver.Navigate().GoToUrl("https://multilogin.com/");

                Assert.AreEqual("Multilogin - Replace Multiple Computers With Virtual Browser Profiles - Multilogin", driver.Title);
                driver.Close();
            }*/



    /*        private void button1_Click(object sender, EventArgs e) =>
                Task.Run(() => BrowserInstructor.WalkOnWilliamHill(Progress));

            private void button6_Click(object sender, EventArgs e) =>
                Task.Run(() => BrowserInstructor.WalkOnStubHub(Progress));*/

    /*        private void button7_Click(object sender, EventArgs e) =>
                Task.Run(() => BrowserInstructor.RegisterOnWilliamHill(Progress));*/

    //TODO REFACTOR. NO BUSINESS LOGIC HERE!
    /*        private void button8_Click(object sender, EventArgs e)
            {
                var path = @"russian_surnames.txt";
                var linesArr = File.ReadAllLines(path, Encoding.UTF8);
                foreach (string line in linesArr)
                {
                    int len = line.Length; //    
                    var ending = line.Substring(len - 2, 2);
                    if (ending == "ОВ" || ending == "ЕВ" || ending == "ИН" || ending == "ИЙ" || ending == "ОЙ")

                    {
                        File.AppendAllText(@"filtered_russian_surnames.txt", line + "\r\n", Encoding.UTF8);
                        textBox1.AppendText(line + "...\r\n");
                    };
                }
                textBox1.AppendText("Все строки исправлены!\r\n");
            }*/

    /*        private void button9_Click(object sender, EventArgs e) =>
                Task.Run(() => BrowserInstructor.ParseUaData(Progress));


            private void button11_Click(object sender, EventArgs e) =>
                Task.Run(() => BrowserInstructor.ParseRusData(Progress));*/

    /*        private void button10_Click(object sender, EventArgs e) =>
                StartSTATask(() => ClickInstructor.RegisterRamblers(Progress));*/

    /*        private void button12_Click(object sender, EventArgs e) =>
                StartSTATask(() => ClickInstructor.RegisterSkrills(Progress));*/

    //Resident schema
    /*        private void button13_Click(object sender, EventArgs e) =>
                StartSTATask(() => ClickInstructor.RegisterByResidentScheme(Progress));*/


    // williamhill reg test
    /*        private void button7_Click(object sender, EventArgs e) =>
                StartSTATask(() => ClickInstructor.RegisterByGmailNPayRotationScheme(Progress));*/

}

