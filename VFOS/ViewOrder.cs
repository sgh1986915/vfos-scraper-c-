using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CookComputing.XmlRpc;
using System.Configuration;
using System.Collections;
namespace VFPO
{
    public partial class ViewOrder : Form
    {
        IXMLRPCProxy proxy = (IXMLRPCProxy)XmlRpcProxyGen.Create(typeof(IXMLRPCProxy));
        IXMLRPCProxyAdmin adminProxy = (IXMLRPCProxyAdmin)XmlRpcProxyGen.Create(typeof(IXMLRPCProxyAdmin));

        private string STOREID = SystemSettings.Default.STOREID;
        private string STORESECURITYKEY = SystemSettings.Default.STORESECURITYKEY;
        private string XMLRPCAPIURL = SystemSettings.Default.XMLRPCAPIURL;
        private string USERNAME = SystemSettings.Default.USERNAME;
        private string PASSWORD = SystemSettings.Default.PASSWORD;
        private bool IsAdmin { set; get; }
        private string OrderId { set; get; }
        private string OrderStatus;
        private string PrintTemplateIndex = "1";
        public ViewOrder(string orderId,bool isAdmin,string orderStatus,string printTemplateIndex)
        {
            InitializeComponent();
            OrderId = orderId;
            IsAdmin = isAdmin;
            OrderStatus = orderStatus;
            PrintTemplateIndex = printTemplateIndex;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            XmlRpcStruct orderDetails = default(XmlRpcStruct);
            try
            {
                proxy.Url = XMLRPCAPIURL;
                if (!IsAdmin)
                {
                    orderDetails = proxy.GetOrder(STOREID, STORESECURITYKEY, OrderId);
                }
                else
                {
                    string decPassword = CryptoLib.Decrypt(PASSWORD, SystemSettings.Default.DECRYPTKEY);
                    orderDetails = adminProxy.GetOrder(USERNAME, decPassword, OrderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve order data from server, please contact system administrator","Error");
                return;
            }

          
         
            try
            {
                if (string.IsNullOrEmpty(PrintTemplateIndex))
                {
                    PrintTemplateIndex = SystemSettings.Default.PRINTTEMPLATE;
                }
                double noRows = 0;

                reportViewer1.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory + string.Format("PrintOrderTemplate{0}.rdlc", PrintTemplateIndex);
                LocalReport report = reportViewer1.LocalReport;
                CommonMethods.PrepareOrderReport(ref  report, orderDetails, OrderStatus, ref noRows);
                //reportViewer1.LocalReport.SetParameters(parameters);
                //Microsoft.Reporting.WinForms.ReportDataSource datasource = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
               // reportViewer1.LocalReport.DataSources.Add(datasource);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while presening order details report", "Error");
            }
        }
    }
}
