using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using CookComputing.XmlRpc;

namespace VFPO
{
    public partial class SystemSettingsControl : UserControl
    {
        IXMLRPCProxy proxy = (IXMLRPCProxy)XmlRpcProxyGen.Create(typeof(IXMLRPCProxy));
        public SystemSettingsControl()
        {
            InitializeComponent();
        }
        private string ValidateAppSetttings(string storeId,string storeSecurityKey)
        {    string response=string.Empty;       
            try
            {
                response = proxy.SetStoreStatus(storeId, storeSecurityKey, "0");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        private void SaveAppSettings()
        {
            try
            {

                var settings = SystemSettings.Default;
                settings.STOREID = txtStoreID.Text.Trim();
                settings.STORESECURITYKEY = txtSecurityKey.Text.Trim();
                settings.XMLRPCAPIURL = txtXMLAPIURL.Text.Trim();
                
                settings.PRINTERNAME = cbxPrinterNames.Text;
                settings.PRINTNOCOPIES = txtPrintCopies.Text.Trim();
                settings.DOWNLOADINTERVAL = cbxInterval.Text.ToString();
                settings.PRINTTEMPLATE = (cbxPrintTemplates.SelectedIndex+1).ToString();
                if (!AppUtilities.CheckInternetConnection())
                {
                    MessageBox.Show("Internet connection is not available, please check your network settings", "Warning");
                    return;
                }
                if (cbxPrinterNames.SelectedIndex == 0)
                {
                    settings.PRINTERNAME = string.Empty;
                }
                proxy.Url = settings.XMLRPCAPIURL;
                try
                {
                    string response = proxy.SetStoreStatus(settings.STOREID, settings.STORESECURITYKEY, "0");
                    if (response != "OK")
                    {
                        if (response == "Authentication Error")
                        {
                            MessageBox.Show("Invalid Store API ID or Security Key", "Warning");
                        }
                        return;
                    }
                }
                catch (Exception ex2)
                {                   
                    MessageBox.Show("Invalid XML RPC API URL", "Warning");
                    return;
                }
                if (settings.CONFIGURED == "0")
                {
                    settings.CONFIGURED = "1";
                    settings.Save();
                    Application.Restart();
                    return;
                }
                else
                {
                   
                    settings.Save();
                    OrderProcessClient pForm = this.ParentForm as OrderProcessClient;
                    if (pForm != null)
                    {
                        pForm.LoadAppSettings();
                    }
                    
                }                
                    
                MessageBox.Show("Application settings has been saved successfully", "Settings");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unable to save application settings, please contact administrator", "Error");

            }
        }
        private void BindPrinterNames()
        {
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();
            string result = string.Empty;
            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string printerName = "";
            cbxPrinterNames.Items.Clear();
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString();
                cbxPrinterNames.Items.Add(printerName);
            }
            cbxPrinterNames.Items.Insert(0,"Default Printer");
        }
        private void SystemSettingsControl_Load(object sender, EventArgs e)
        {
            BindPrinterNames();
            var settings = SystemSettings.Default;
            if (settings["CONFIGURED"].ToString() == "1")
            {
                txtStoreID.Text = settings.STOREID;
                txtSecurityKey.Text = settings.STORESECURITYKEY;
                txtXMLAPIURL.Text = settings.XMLRPCAPIURL;
                if (string.IsNullOrEmpty(settings.DOWNLOADINTERVAL))
                {
                    cbxInterval.SelectedIndex = 0;
                }
                else
                {
                    cbxInterval.Text = settings.DOWNLOADINTERVAL;
                }
                int templateIndex = 0;
                int.TryParse(settings.PRINTTEMPLATE, out templateIndex);
                if (templateIndex > 0)
                {
                    cbxPrintTemplates.SelectedIndex = templateIndex - 1;
                }
                else
                {
                    cbxPrintTemplates.SelectedIndex = 0;
                }
                
                btnPreview.Enabled = true;
            }
            else
            {
                cbxInterval.SelectedIndex = 0;
                cbxPrintTemplates.SelectedIndex = 0;
                btnPreview.Enabled = false;
            }
            string currentPrinter = settings.PRINTERNAME;
            if (string.IsNullOrEmpty(currentPrinter))
            {
                cbxPrinterNames.SelectedIndex = 0;
            }
            else
            {
                for (int itemCount = 0; itemCount < cbxPrinterNames.Items.Count; itemCount++)
                {
                    if (cbxPrinterNames.Items[itemCount].ToString() == currentPrinter)
                    {
                        cbxPrinterNames.SelectedIndex = itemCount;
                    }
                }
            }
            
            txtPrintCopies.Text = settings.PRINTNOCOPIES;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStoreID.Text.Trim()))
            {
                MessageBox.Show("Please provide a valid Store Id", "Validation");
                return;
            }
            if (string.IsNullOrEmpty(txtSecurityKey.Text.Trim()))
            {
                MessageBox.Show("Please provide a valid Store Security Key", "Validation");
                return;
            }
            if (string.IsNullOrEmpty(txtXMLAPIURL.Text.Trim()))
            {
                MessageBox.Show("Please provide a valid XML RPC API URL", "Validation");
                return;
            }
            if (string.IsNullOrEmpty(txtPrintCopies.Text.Trim()))
            {

                MessageBox.Show("Please provide no of print copies required", "Validation");
                return;
            }
            else
            {
                int copies = 0;
                int.TryParse(txtPrintCopies.Text.Trim(), out copies);
                if (copies == 0)
                {
                    MessageBox.Show("Please provide valid number for No of print copies required", "Validation");
                    return;
                }
            }
            SaveAppSettings();

        }

       
        private void btnPreview_Click(object sender, EventArgs e)
        {
            string orderId = "1";
            string status = "Pending";
            try
            {
                IXMLRPCProxyAdmin adminProxy = (IXMLRPCProxyAdmin)XmlRpcProxyGen.Create(typeof(IXMLRPCProxyAdmin));
                string decPassword = CryptoLib.Decrypt(SystemSettings.Default.PASSWORD, SystemSettings.Default.DECRYPTKEY);
                XmlRpcStruct[] orderslist = adminProxy.GetOrders(SystemSettings.Default.USERNAME, decPassword, "0", "1",SystemSettings.Default.STOREID);
                if (orderslist != null)
                {
                    if (orderslist.Count() != 0)
                    {
                        orderId = orderslist.FirstOrDefault()["order_id"].ToString();
                        string printTemplateIndex=(cbxPrintTemplates.SelectedIndex+1).ToString();
                        status = orderslist.FirstOrDefault()["status"].ToString();
                        ViewOrder frmOrderView = new ViewOrder(orderId, true, status,printTemplateIndex);
                        frmOrderView.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to prepare order print preview,please try again or later", "Error");
            }
            
        }
    }
}
