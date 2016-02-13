using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VFPO
{
    public partial class HelpTips : UserControl
    {
        public HelpTips()
        {
            InitializeComponent();
        }

        private void HelpTips_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(HelpTips_Paint);
            
        }

        void HelpTips_Paint(object sender, PaintEventArgs e)
        {
            helpContainer.Width = Width-12;
            helpContainer.Height = Height-26;    
        }
     
    }
}
