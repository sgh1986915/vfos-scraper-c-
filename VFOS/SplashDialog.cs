using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace VFPO
{
    public partial class SplashDialog : Form
    {
        private Thread CloseSplashScreenThread;
        private delegate void CloseSplashScreenAction();
        public SplashDialog()
        {
            InitializeComponent();
        }
        private void CloseSplashAndLaunchApp()
        {
            this.Close();            
        }
        private void LaunchApplicationInThread()
        {
            CloseSplashScreenAction showSplash = new CloseSplashScreenAction(CloseSplashAndLaunchApp);
            IAsyncResult result = this.BeginInvoke(showSplash);
        }
        private void SplashDialog_Load(object sender, EventArgs e)
        {
            //System.Timers.Timer theTimer = new System.Timers.Timer();
            //theTimer.Interval = 3000;
            //theTimer.Elapsed += new System.Timers.ElapsedEventHandler(theTimer_Elapsed);
            //theTimer.Start();
        }

        void theTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Timers.Timer theTimer=sender as System.Timers.Timer;
            if(theTimer!=null)
            {
                theTimer.Stop();
            }
            CloseSplashScreenThread = new Thread(new ThreadStart(LaunchApplicationInThread));
            CloseSplashScreenThread.IsBackground = true;
            CloseSplashScreenThread.Start();                  
        }
    }
}
