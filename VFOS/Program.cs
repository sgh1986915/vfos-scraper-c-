using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VFPO
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "Install") // during installation clear previous cached system settings
                {
                    SystemSettings.Default.CONFIGURED = "0";
                    SystemSettings.Default.DOWNLOADINTERVAL = "1";
                    SystemSettings.Default.PRINTERNAME = string.Empty;
                    SystemSettings.Default.PRINTNOCOPIES = "1";
                    SystemSettings.Default.PRINTTEMPLATE = "1";
                    SystemSettings.Default.STOREID = string.Empty;
                    SystemSettings.Default.STORESECURITYKEY = string.Empty;
                    SystemSettings.Default.XMLRPCAPIURL = string.Empty;
                    SystemSettings.Default.Save();
                    Application.Exit();
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (SystemSettings.Default["CONFIGURED"].ToString() == "0")
            {               
                Application.Run(new ConfigWizard());
            }
            else
            {
                Application.Run(new OrderProcessClient());
            }    
        }
    }
}
