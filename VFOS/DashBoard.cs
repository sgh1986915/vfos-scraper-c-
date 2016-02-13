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
    public partial class DashBoard : UserControl
    {
        public DataGridView StoreStatusGrid
        {           
            get
            {
                return gvStoreStatus;
            }
        }
        public DashBoard()
        {
            InitializeComponent();
        }
    }
}
