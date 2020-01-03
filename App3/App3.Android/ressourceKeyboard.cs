//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Windows.Forms;

//using System.Runtime.InteropServices;
//using System.Diagnostics;
//using System.Threading;

//namespace WebApplication1
//{

//    public partial class _Default : Page
//    {
//        Process myProcess;

//        String processKodi = "F:\\Program Files\\Kodi\\kodi.exe"; 
//        String processNetflix = "Netflix:";

//        cpuLoadClass cLC;


//        [DllImport("user32.dll", SetLastError = true)]
//        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

//        [DllImport("user32.dll")]
//        static extern uint GetWindowThreadProcessId(IntPtr hWnd, [Out] uint lpdwProcessName);


//        //---------------------------------------------------------------------//


//        protected void Page_Load(object sender, EventArgs e)
//        {
//            cLC = new cpuLoadClass();
//            CPULoadText.Text = "Page_load";
//            //updateCpuLoad("11111");
//        }

//        void OnStart()
//        {

//        }

//        internal void updateCpuLoad(String loadString)
//        {
//            //CPULoadText.Text = loadString;
//            CPULoadText.Text = "updateCpuLoad";

//            //System.Diagnostics.Debug.WriteLine("**** Debug: " + this.FindControl("CPULoadText"));
//        }


//        protected void VolumeDownBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.VolumeDown, 0, 0, 0); // decrease volume 
//        }

//        protected void VolumeUpBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.VolumeUp, 0, 0, 0); // decrease volume 
//        }

//        protected void playPauseBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.MediaPlayPause, 0, 0, 0);
//        }

//        protected void enterBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Enter, 0, 0, 0); //Key down
//            keybd_event((byte)Keys.Enter, 0, 0x0002, 0); //Key up
//        }

//        protected void upBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Up, 0, 0, 0);
//        }

//        protected void downBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Down, 0, 0, 0);
//        }

//        protected void leftBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Left, 0, 0, 0);
//        }

//        protected void rightBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Right, 0, 0, 0);
//        }

//        protected void returnBtn_Click(object sender, EventArgs e)
//        {
//            keybd_event((byte)Keys.Back, 0, 0, 0); //Key down
//            keybd_event((byte)Keys.Back, 0, 0x0002, 0); //Key up
//        }

//        protected void kodiBtn_Click(object sender, EventArgs e)
//        {
//            foreach (var process in Process.GetProcessesByName("WWAHost"))
//            {
//                process.Kill();
//            }
//            myProcess = Process.Start(processKodi);
//        }

//        protected void netflixBtn_Click(object sender, EventArgs e)
//        {
//            foreach (var process in Process.GetProcessesByName("Kodi"))
//            {
//                process.Kill();
//            }
//            myProcess = Process.Start(processNetflix); 
//        }
//    }
//}
