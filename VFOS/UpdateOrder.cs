using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using CookComputing.XmlRpc;

namespace VFPO
{
    public partial class frmUpdateOrder : Form
    {
        public string OrderId;     
        public XmlRpcStruct[] OrderStatuses;
        public string Status;
        public bool IsAdmin = false;
        public frmUpdateOrder(string oId,XmlRpcStruct[] orderStatuses,string status,bool isAdmin)
        {
            InitializeComponent();
            OrderId = oId;     
            OrderStatuses = orderStatuses;
            Status = status;
            IsAdmin = isAdmin;
        }

        private void frmUpdateOrder_Load(object sender, EventArgs e)
        {
            XmlRpcStruct newItem = new XmlRpcStruct();            
            var os = OrderStatuses.ToList();           
            var data = os.Select(store => new
            {
                Name = store["name"],
                ID = store["order_status_id"]
            }).ToList();
            cbxOrderStatuses.DataSource = data;
            cbxOrderStatuses.ValueMember = "ID";
            cbxOrderStatuses.DisplayMember = "Name";
            try
            {
                cbxOrderStatuses.Text = Status;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string comments = txtComments.Text.Trim();
            string STOREID = SystemSettings.Default.STOREID;

            string STORESECURITYKEY = SystemSettings.Default.STORESECURITYKEY;
            string XMLRPCAPIURL = SystemSettings.Default.XMLRPCAPIURL;
            IXMLRPCProxy proxy = (IXMLRPCProxy)XmlRpcProxyGen.Create(typeof(IXMLRPCProxy));
            string notify = "0";
            if (cbkNotify.Checked)
            {
                notify = "1";
            }
            string orderStatusUd = "1";
            if (cbxOrderStatuses.SelectedValue != null)
            {
                orderStatusUd = cbxOrderStatuses.SelectedValue.ToString();
            }
            try
            {
                if (!IsAdmin)
                {
                    string result = proxy.UpdateOrder(STOREID, STORESECURITYKEY, OrderId, orderStatusUd, comments, notify);
                }
                else
                {
                    IXMLRPCProxyAdmin adminProxy = (IXMLRPCProxyAdmin)XmlRpcProxyGen.Create(typeof(IXMLRPCProxyAdmin));
                    string decPassword = CryptoLib.Decrypt(SystemSettings.Default.PASSWORD, SystemSettings.Default.DECRYPTKEY);
                    string result = adminProxy.UpdateOrder(SystemSettings.Default.USERNAME,decPassword, OrderId, orderStatusUd,notify,comments);
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.No;
            }
                       
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
