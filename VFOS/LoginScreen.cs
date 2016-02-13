using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Configuration;
using System.Security.Cryptography;

namespace VFPO
{
    public partial class LoginScreen : Form
    {
        IXMLRPCProxyAdmin adminProxy = (IXMLRPCProxyAdmin)XmlRpcProxyGen.Create(typeof(IXMLRPCProxyAdmin));
        XmlRpcStruct[] orderslist = { };
        private string USERNAME = SystemSettings.Default.USERNAME;
        private string PASSWORD = SystemSettings.Default.PASSWORD;
          
        public DataTable OfflineStores { set; get; }
        public LoginScreen()
        {
            InitializeComponent();            
        }
            
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        public byte[] GenerateTripleDesKey() { RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider(); byte[] tripleDesKey = new byte[24]; rng.GetBytes(tripleDesKey); for (var i = 0; i < tripleDesKey.Length; ++i) { int keyByte = tripleDesKey[i] & 0xFE; var parity = 0; for (int b = keyByte; b != 0; b >>= 1) 			parity ^= b & 1; tripleDesKey[i] = (byte)(keyByte | (parity == 0 ? 1 : 0)); } return tripleDesKey; }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string uName = string.Empty;
            string uPassword = string.Empty;
            uName = txtUserName.Text.Trim();
            uPassword = txtPassword.Text.Trim();
                       
            if (string.IsNullOrEmpty(uName))
            {
                MessageBox.Show(this, "Please enter User Name", "Validation", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(uPassword))
            {
                MessageBox.Show(this, "Please enter Password", "Validation", MessageBoxButtons.OK);
                return;
            }
            string encPassword = "";
            try
            {
                encPassword = CryptoLib.Encrypt(uPassword, SystemSettings.Default.DECRYPTKEY);
            }
            catch (Exception ex)
            {
                return;
            }
           
            if (uName == USERNAME && encPassword == PASSWORD)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Please enter valid User Name or Password", "Authentication Failed", MessageBoxButtons.OK);
                return;
            }

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
