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
    public partial class ConfigWizard : Form
    {
        private Thread ShowLogonThread;
        private delegate void ShowLogonAction();
        public ConfigWizard()
        {
            InitializeComponent();
        }

        private void ConfigWizard_Load(object sender, EventArgs e)
        {
                   
        }
            
    }
}
