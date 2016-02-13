using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Reporting.WinForms;
using System.Globalization;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Reflection;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using CookComputing.XmlRpc;
using System.Data.SqlClient;
using System.Management;
using System.Printing;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace VFPO
{
    public partial class OrderProcessClient : Form
    {
        private const string AppName = "VFOS";

        private IList<Stream> m_streams;
        private int m_currentPageIndex;
        protected System.Timers.Timer scheduler;
        Thread ordersDownloadThread;
        Thread processingOrdersDelayedThread;
        IXMLRPCProxy proxy = (IXMLRPCProxy)XmlRpcProxyGen.Create(typeof(IXMLRPCProxy));
        IXMLRPCProxyAdmin adminProxy = (IXMLRPCProxyAdmin)XmlRpcProxyGen.Create(typeof(IXMLRPCProxyAdmin));
        XmlRpcStruct[] orderslist = { };
        XmlRpcStruct[] stores = { };
        private string STOREID = string.Empty;
        private string STORESECURITYKEY = string.Empty;
        private string XMLRPCAPIURL = string.Empty;
        private string PRINTERNAME = string.Empty;
        private string PRINTNOCOPIES = string.Empty;
        private string USERNAME = string.Empty;
        private string PASSWORD = string.Empty;
        private int DOWNLOADINTERVAL = 1;
        private delegate void LoadOrdersAction();
        private delegate void LoadOrderStatusAction();
        private Thread LoadOrderStatusThread;
        private bool INTERNET = false;
        XmlRpcStruct[] OrderStatuses = { };
        private bool IsAdmin { set; get; }
        private SplashScreen sp = null;
        private Thread StatusBarUpdateThread;
        private delegate void StatusBarUpdateAction(string status);
        private delegate void RefreshOrdersGridAction();
        private Thread LoadOrdersGridThread;
        private delegate void LoadOrdersGridAction();
        private Thread RefreshOrdersGridThread;
        private System.Timers.Timer gridRefresher;
        private Thread LoadAdminDashboardInThread;
        int CurrentTabIndex = 0;
        private delegate void LoadAdminDashboardAction();
        private bool WINDOWACTIVE = true;
        SplashDialog splash = new SplashDialog();

        #region "Methods"
        /// <summary>
        /// When user logged in as admin switch to admin display
        /// </summary>
        private void SwitchToAdminView()
        {
            IsAdmin = true;

            BindStoreNames();
            BindOrderStatuses();
            if (ordersDownloadThread != null && ordersDownloadThread.IsAlive)
            {
                ordersDownloadThread.Abort();
            }
            ordersDownloadThread = new Thread(new ThreadStart(RunLoadOrdersInThread));
            ordersDownloadThread.IsBackground = true;
            ordersDownloadThread.Start();
            if (scheduler != null)
            {
                scheduler.Stop();
            }
            scheduler = new System.Timers.Timer();
            scheduler.Interval = DOWNLOADINTERVAL * 60000;
            scheduler.Elapsed += new System.Timers.ElapsedEventHandler(scheduler_Elapsed);
            scheduler.Start();
        }
        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();         
            m_streams.Add(stream);
            return stream;
        }
        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report,double pageHeight)
        {
            
            string deviceInfo =
              @"<DeviceInfo>
                <MarginBottom>0in</MarginBottom>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginTop>0in</MarginTop>
                <StartPage>0</StartPage>
                <InteractiveHeight>0</InteractiveHeight>" +
                "<PageHeight>"+ pageHeight + "in</PageHeight>"  +        
                "<OutputFormat>EMF</OutputFormat>"  +           
            "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);
                     
            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
        /// <summary>
        /// do necessary stuff to print the given document
        /// </summary>
        /// <param name="docId"></param>
        private void Print(string docId)
        {
            if (m_streams == null || m_streams.Count == 0)
            {
                return;
                // throw new Exception("Error: no stream to print.");
            }
            PrintDocument printDoc = new PrintDocument();

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                PrintController printController = new StandardPrintController();
                if (!string.IsNullOrEmpty(PRINTERNAME)) // set specific printer name if default printer is not allowed
                {
                    printDoc.PrinterSettings.PrinterName = PRINTERNAME;
                }
                printDoc.PrintController = printController;

                printDoc.DocumentName = AppName + docId;
                short copies = 1;
                short.TryParse(PRINTNOCOPIES, out copies);
                printDoc.PrinterSettings.Copies = copies;               
                printDoc.Print();
                // Thread.Sleep(10000);
            }
        }
        // Create a local report for Report.rdlc, load the data,
        //    export the report to an .emf file, and print it.
        private void ProcessPrint(string orderId, string status)
        {
            XmlRpcStruct orderDetails = default(XmlRpcStruct);
            try
            {
                if (!IsAdmin)
                {
                    orderDetails = proxy.GetOrder(STOREID, STORESECURITYKEY, orderId);
                }
                else
                {
                    orderDetails = adminProxy.GetOrder(USERNAME, PASSWORD, orderId);
                }
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "Print Failed", ToolTipIcon.Error);
                return;
            }
            LocalReport report = new LocalReport();
            report.ReportPath = AppDomain.CurrentDomain.BaseDirectory + string.Format("PrintOrderTemplate{0}.rdlc", SystemSettings.Default.PRINTTEMPLATE);
            double noRows = 0;
            CommonMethods.PrepareOrderReport(ref report, orderDetails, status,ref noRows);
           
            double pageHeight=7;
            pageHeight = pageHeight + noRows;
            Export(report, pageHeight);
            Print(orderId);
            if (!IsAdmin)
            {
                proxy.UpdateOrder(STOREID, STORESECURITYKEY, orderId, OrderStatusConstants.Processing, "Processing started at store", "1");
            }
            else
            {
                string result = adminProxy.UpdateOrder(SystemSettings.Default.USERNAME, PASSWORD, orderId, OrderStatusConstants.Processing, "1", "Processing started at store");
            }

        }

        /// <summary>
        /// Get orders pending for processing and send each order to a printer 
        /// </summary>
        private void LoadTodaysOrders()
        {

            try
            {
                if (!IsAdmin)
                {
                    orderslist = proxy.GetOrders(STOREID, STORESECURITYKEY, OrderStatusConstants.Pending, "10");
                }
                else
                {

                    orderslist = adminProxy.GetOrders(USERNAME, PASSWORD, OrderStatusConstants.Pending, "10", STOREID);
                }
                toolStripStatusLabel2.Text = "| Last Fetch Time: " + DateTime.Now.ToLongTimeString();
                if (AppUtilities.PrinterStatus(PRINTERNAME) == "CONNECTED")
                {
                    notifyIcon1.ShowBalloonTip(1000 * 60, "VFOS Alert", "Printer is offline, please connect printer to your computer", ToolTipIcon.Warning);
                    //  MessageBox.Show("Printer is offline, please connect printer to your computer", "Warning");                    
                }
                else
                {
                    foreach (XmlRpcStruct od in orderslist)
                    {
                        string oid = od["order_id"].ToString();
                        string orderStatus = od["status"].ToString();
                        string jobName = AppName + oid;
                        if (AppUtilities.IsDocumentPrinting(jobName) == PrintJobStatus.None) // if not processed for print then do print
                        {
                            ProcessPrint(oid, orderStatus);
                        }
                        else
                        {
                            if (!IsAdmin)
                            {
                                proxy.UpdateOrder(STOREID, STORESECURITYKEY, oid, OrderStatusConstants.Processing, "Processing started at store", "1");
                            }
                            else
                            {
                                string result = adminProxy.UpdateOrder(SystemSettings.Default.USERNAME, PASSWORD, oid, OrderStatusConstants.Processing, "1", "Processing started at store");
                            }
                            notifyIcon1.ShowBalloonTip(1000 * 60, "VFOS Alert", "Order # \"" + oid + "\" already sent to printing, check printer queue", ToolTipIcon.Warning);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
            }
            finally
            {
                ordersDownloadThread.Abort();
            }

        }

        /// <summary>
        /// Get Color Code for specific order status. This Color code will be used for coloring status column in Orders Grid.
        /// </summary>
        /// <param name="orderStatusId">order status id</param>
        /// <returns>Color Code Object</returns>
        private Color GetColorCode(string orderStatusId)
        {
            switch (orderStatusId)
            {
                case "1": return Color.OrangeRed;
                case "2": return Color.LawnGreen;
                case "3": return Color.GreenYellow;
                case "4": return Color.Gold;
                case "5": return Color.Green;
                case "6": return Color.Gray;
                case "7": return Color.LemonChiffon;
                case "8": return Color.LightGoldenrodYellow;
                case "9": return Color.LightBlue;
                case "10": return Color.LightSeaGreen;
                case "11": return Color.LightSlateGray;
                case "12": return Color.LightSteelBlue;
                case "13": return Color.PaleVioletRed;
                case "14": return Color.PaleGoldenrod;
                case "15": return Color.Olive;
                case "16": return Color.PapayaWhip;
                case "17": return Color.Plum;
                case "18": return Color.Red;
                case "19": return Color.Silver;
                case "20": return Color.SpringGreen;
                default:
                    return Color.Wheat;
            }
        }
        /// <summary>
        /// This method reads all the application settings from System Settings Configuration
        /// </summary>
        public void LoadAppSettings()
        {
            try
            {

                var settings = SystemSettings.Default;
                STOREID = settings.STOREID;
                STORESECURITYKEY = settings.STORESECURITYKEY;
                XMLRPCAPIURL = settings.XMLRPCAPIURL;
                USERNAME = settings.USERNAME;
                PASSWORD = CryptoLib.Decrypt(settings.PASSWORD, SystemSettings.Default.DECRYPTKEY);
                PRINTERNAME = settings.PRINTERNAME;
                PRINTNOCOPIES = settings.PRINTNOCOPIES;
                proxy.Url = XMLRPCAPIURL;
                adminProxy.Url = XMLRPCAPIURL;
                int.TryParse(settings.DOWNLOADINTERVAL, out DOWNLOADINTERVAL);
                if (DOWNLOADINTERVAL < 1)
                {
                    DOWNLOADINTERVAL = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to retrieve application settings, please contact administrator", "Error");
                // MessageBox.Show(ex.Message, "Error");
                Application.Exit();
            }
        }
        /// <summary>
        /// Check if order is printing or not and update order status in orderlist
        /// </summary>
        /// <param name="ordersList">array of orders represented in XmlRpcStruct </param>
        /// <returns>updated array of orders represented in XmlRpcStruct</returns>
        private XmlRpcStruct[] CheckOrdersPrintingStatus(XmlRpcStruct[] ordersList)
        {
            // Select all the outstanding print jobs.
            string query = "SELECT * FROM Win32_PrintJob";
            ManagementObjectSearcher jobQuery =
              new ManagementObjectSearcher(query);
            ManagementObjectCollection jobs = jobQuery.Get();
            IEnumerator ol = ordersList.GetEnumerator();
            for (int count = 0; count < orderslist.Length; count++)
            {

                XmlRpcStruct order = (XmlRpcStruct)orderslist[count];
                order["status"] = AppUtilities.IsDocumentPrinting(AppName + order["order_id"]).ToString();
                orderslist[count] = order;

            }
            return ordersList;
        }
        #endregion

        #region "User Interface Rendering Methods"

        /// <summary>
        /// Format orders grid display by applying color codes to order status column
        /// </summary>
        private void FormatOrdersGrid()
        {
            gvOrdersList.EditMode = DataGridViewEditMode.EditProgrammatically;
            for (int rowCount = 0; rowCount < gvOrdersList.Rows.Count; rowCount++)
            {
                if (gvOrdersList.Rows[rowCount].Cells[2].Value != null)
                {
                    string orderStatusId = "0";
                    string name = gvOrdersList.Rows[rowCount].Cells[2].Value.ToString().Trim();
                    var status = (from s in OrderStatuses where s["name"].ToString() == name select s).ToList().FirstOrDefault();
                    orderStatusId = status["order_status_id"].ToString();
                    gvOrdersList.Rows[rowCount].Cells[2].Style.BackColor = GetColorCode(orderStatusId);
                }
            }

            gvOrdersList.Refresh();
        }
        /// <summary>
        /// Load Orders Grid for POS client Module
        /// </summary>
        private void LoadOrdersGrid()
        {
            if (OrderStatuses != null)
            {
                if (OrderStatuses.Count() == 0)
                {
                    BindOrderStatuses();
                }
            }
            string orderStatusId = "";
            if (cbxOrderStatus.SelectedValue is XmlRpcStruct)
            {
                orderStatusId = ((XmlRpcStruct)cbxOrderStatus.SelectedValue)["order_status_id"].ToString();
            }
            else
            {

                orderStatusId = cbxOrderStatus.SelectedValue.ToString();
            }
            if (cbxOrderStatus.SelectedIndex == 0)
            {
                orderStatusId = "0";
            }


            try
            {
                orderslist = proxy.GetOrders(STOREID, STORESECURITYKEY, orderStatusId, "0");
                PrepareGridDisplay(orderslist);
            }
            catch (Exception ex)
            {

                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
            }

        }
        /// <summary>
        /// Prepare Grid for Display with customizing columns, adding button columns and etc
        /// </summary>
        /// <param name="orderslist">array of orders representing in XmlRpcStruct</param>
        private void PrepareGridDisplay(XmlRpcStruct[] orderslist)
        {
            gvOrdersList.Columns.Clear();
            var ol = orderslist.Select(order => new
            {
                OrderId = order["order_id"],
                Customer = order["customer"],
                Status = order["status"],
                DateOrdered = order["date_added"],                
                Total = Utilities.TryGetCurrencySymbol(order["currency_code"].ToString()) + Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(order["total"].ToString()))).ToString(),
                CurrencyCode = order["currency_code"]
            }).ToList();
            gvOrdersList.DataSource = ol;

            DataGridViewButtonColumn updateButton = new DataGridViewButtonColumn();
            updateButton.FlatStyle = FlatStyle.Flat;
            updateButton.HeaderText = "Update";
            updateButton.Text = "Update";
            updateButton.UseColumnTextForButtonValue = true;
            updateButton.Name = "Update";
            gvOrdersList.Columns.Add(updateButton);
            DataGridViewButtonColumn printButton = new DataGridViewButtonColumn();
            printButton.FlatStyle = FlatStyle.Flat;
            printButton.HeaderText = "Print";
            printButton.Text = "Print";
            printButton.UseColumnTextForButtonValue = true;
            printButton.Name = "PrintColumn";
            gvOrdersList.Columns.Add(printButton);
            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.FlatStyle = FlatStyle.Flat;

            viewButton.HeaderText = "View";
            viewButton.Text = "View";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Name = "ViewColumn";
            gvOrdersList.Columns.Add(viewButton);

            gvOrdersList.Columns[0].HeaderText = "Order ID";
            gvOrdersList.Columns[1].HeaderText = "Customer";
            gvOrdersList.Columns[2].HeaderText = "Current Status";
            gvOrdersList.Columns[3].HeaderText = "Ordered Date";
            gvOrdersList.Columns[4].HeaderText = "Total";
            gvOrdersList.Columns[5].HeaderText = "Currency Code";
            gvOrdersList.Columns[2].Width = 150;
            gvOrdersList.Columns[3].Width = 150;
            gvOrdersList.Columns[5].Width = 120;
            gvOrdersList.Columns[1].Width = 210;
            gvOrdersList.Columns[4].Width = 100;
            gvOrdersList.Columns[5].Visible = false;

            gvOrdersList.CellClick -= new DataGridViewCellEventHandler(gvOrdersList_CellClick);
            gvOrdersList.CellClick += new DataGridViewCellEventHandler(gvOrdersList_CellClick);
            FormatOrdersGrid();
            gvOrdersList.Select();

        }
        /// <summary>
        /// Load Orders Grid for admin module
        /// </summary>
        private void LoadOrdersGridforAdminView()
        {
            string orderStatusId = "";
            string storeId = "";
            if (stores != null)
            {
                if (stores.Count() == 0)
                {
                    BindStoreNames();
                }
            }
            if (OrderStatuses != null)
            {
                if (OrderStatuses.Count() == 0)
                {
                    BindOrderStatuses();
                }
            }

            if (cbxStoreNames.SelectedValue is XmlRpcStruct)
            {
                storeId = ((XmlRpcStruct)cbxStoreNames.SelectedValue)["storeId"].ToString();
            }
            else
            {
                if (cbxStoreNames.SelectedIndex == -1)
                {
                    cbxStoreNames.SelectedIndex = 0;
                }
                storeId = cbxStoreNames.SelectedValue.ToString();

            }
            if (cbxOrderStatus.SelectedValue is XmlRpcStruct)
            {
                orderStatusId = ((XmlRpcStruct)cbxOrderStatus.SelectedValue)["order_status_id"].ToString();
            }
            else
            {
                try
                {
                    orderStatusId = cbxOrderStatus.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    orderStatusId = "";
                }
            }
            if (cbxOrderStatus.SelectedIndex == 0)
            {
                orderStatusId = "0";
            }
            gvOrdersList.Columns.Clear();

            try
            {
                orderslist = adminProxy.GetOrders(USERNAME, PASSWORD, orderStatusId, "0", storeId);
            }
            catch (Exception e1)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
                return;
            }

            if (OrderStatuses != null && OrderStatuses.Count() > 0)
            {
                PrepareGridDisplay(orderslist);
            }

        }

        /// <summary>
        /// Invoke admin store statistics dashboard in safe thread
        /// </summary>
        void RunAdminDashBoardInThread()
        {
            if (CurrentTabIndex != 1) return;
            if (!WINDOWACTIVE) return;
            SplashScreen sp = new SplashScreen();
            sp.TopLevel = true;
            sp.TopMost = true;
            sp.Show();
            IAsyncResult result = null;

            RefreshOrdersGridAction loadOrderStatusAsync = new RefreshOrdersGridAction(RefreshAdminDashboard);
            result = this.BeginInvoke(loadOrderStatusAsync);

            this.EndInvoke(result);
            if (result.IsCompleted)
            {
                if (sp != null)
                {
                    sp.Hide();
                    sp.Close();
                }
            }

        }
        /// <summary>
        /// Invoke showing Splash Screen in a safe thread
        /// </summary>
        void ShowSplashScreenInThread()
        {
            LoadOrdersAction loadOrdersAsync = new LoadOrdersAction(ShowSplashScreen);
            this.BeginInvoke(loadOrdersAsync);
        }
        /// <summary>
        ///  show splash screen
        /// </summary>
        void ShowSplashScreen()
        {
            sp = new SplashScreen();
            sp.Show();
        }

        /// <summary>
        /// This methods renders Store Statistics in Admin Screen"
        /// </summary>
        private void RefreshAdminDashboard()
        {
            XmlRpcStruct storeDefault = new XmlRpcStruct();
            storeDefault["name"] = "Deli Chez (Default)";
            storeDefault["store_id"] = "STORE1";
            var storesUpdated = stores.ToList();
            //  storesUpdated.Insert(0, storeDefault);
            var storeNames = storesUpdated.Select(store => new
            {
                Name = store["name"],
                ID = store["store_id"]
            }).ToList();

            DataTable dtOfflineStores = new DataTable("OfflineStores");
            dtOfflineStores.Columns.Add(new DataColumn("StoreName"));
            dtOfflineStores.Columns.Add(new DataColumn("Status"));
            dtOfflineStores.Columns.Add(new DataColumn("PendingOrdersCount"));
            dtOfflineStores.Columns.Add(new DataColumn("CompletedOrdersCount"));

            for (int count = 0; count < storeNames.Count; count++)
            {
                XmlRpcStruct[] orders = adminProxy.GetOrders(USERNAME, PASSWORD, "", "0", storeNames[count].ID.ToString());
                XmlRpcStruct[] comporders = adminProxy.GetOrders(USERNAME, PASSWORD, "5", "0", storeNames[count].ID.ToString());
                XmlRpcStruct[] pendorders = adminProxy.GetOrders(USERNAME, PASSWORD, "1", "0", storeNames[count].ID.ToString());
                DataRow dRow = dtOfflineStores.NewRow();
                var offlineStore = (from o in orders.ToList()
                                    where (o["status"].ToString() == "") && Convert.ToDateTime(o["date_modified"]) < DateTime.Now.AddMinutes(-10)
                                    select o).ToList();
                var completedOrders = (from o in comporders.ToList()
                                       where Convert.ToDateTime(o["date_modified"]) < DateTime.Now.AddDays(-1)
                                       select o).ToList();
                var pendingOrders1 = (from o in pendorders.ToList()
                                      where Convert.ToDateTime(o["date_modified"]) <= DateTime.Now.AddDays(-1)
                                      select o).ToList();
                var pendingOrders2 = (from o in orders.ToList()
                                      where Convert.ToDateTime(o["date_modified"]) <= DateTime.Now.AddDays(-1)
                                      select o).ToList();
                int pendingOrdersCount = pendingOrders1.Count() + pendingOrders2.Count();
                dRow["CompletedOrdersCount"] = completedOrders.Count().ToString();
                dRow["PendingOrdersCount"] = pendingOrdersCount;
                if (orders.Count() > 0 && offlineStore.Count() > 0)
                {

                    dRow["StoreName"] = storeNames[count].Name;
                    dRow["Status"] = "Offline";

                    dtOfflineStores.Rows.Add(dRow);
                    //  adminProxy.SetStoreStatus(storeNames[count].Name.ToString(), storeNames[count].ID.ToString(), "0");
                }
                else if (orders.Count() == 0)
                {

                    dRow["StoreName"] = storeNames[count].Name;
                    dRow["Status"] = "Not received any orders (idle) for the past 10 minutes";
                    dtOfflineStores.Rows.Add(dRow);
                    // adminProxy.SetStoreStatus(storeNames[count].Name.ToString(), storeNames[count].ID.ToString(), "2");
                }
                else if (orders.Count() > 0 && offlineStore.Count() == 0)
                {

                    dRow["StoreName"] = storeNames[count].Name;
                    dRow["Status"] = "Online";
                    dtOfflineStores.Rows.Add(dRow);
                    // adminProxy.SetStoreStatus(storeNames[count].Name.ToString(), storeNames[count].ID.ToString(), "1");
                }

            }
            DashBoard dbCtrl = tbOrdersList.TabPages[1].Controls[0] as DashBoard;

            dbCtrl.StoreStatusGrid.DataSource = dtOfflineStores;
            dbCtrl.StoreStatusGrid.Columns[0].HeaderText = "Store Name";
            dbCtrl.StoreStatusGrid.Columns[2].HeaderText = "Pending Orders";
            dbCtrl.StoreStatusGrid.Columns[1].Width = 280;
            dbCtrl.StoreStatusGrid.Columns[2].Width = 120;
            dbCtrl.StoreStatusGrid.Columns[3].Width = 140;
            dbCtrl.StoreStatusGrid.Columns[3].HeaderText = "Completed Orders";

            ordersDownloadThread.Abort();
        }
        /// <summary>
        /// Updates Status Bar of the User Interface Asynchronously in safe Thread
        /// </summary>
        /// <param name="status">what Status message to be displayed on Status Bar </param>
        void RunStatusBarUpdateThread(string status)
        {
            StatusBarUpdateAction sbAction = new StatusBarUpdateAction(UpdateStatusBar);
            this.BeginInvoke(sbAction, status);
        }
        void UpdateStatusBar(string status)
        {
            toolStripStatusLabel1.Text = status;
            if (status.Contains("Disconnected"))
            {
                notifyIcon1.ShowBalloonTip(3000, "VFOS Alert", "Lost internet connection, please connect internet", ToolTipIcon.Warning);
            }
            //else
            //{
            //    if (INTERNET)
            //    {
            //        notifyIcon1.ShowBalloonTip(3000, "VFOS Alert", "Connected to internet", ToolTipIcon.Warning);
            //    }
            //}
        }
        /// <summary>
        /// Load Orders Grid in safe thread
        /// </summary>
        void RunLoadOrderGridInThread()
        {
            if (CurrentTabIndex != 0) return;
            if (!WINDOWACTIVE) return; // if current window is not active do not refresh grid
            SplashScreen sp = new SplashScreen();

            sp.TopLevel = true;
            sp.TopMost = true;
            sp.Show();
            IAsyncResult result = null;

            if (IsAdmin)
            {
                RefreshOrdersGridAction loadOrderStatusAsync = new RefreshOrdersGridAction(LoadOrdersGridforAdminView);
                result = this.BeginInvoke(loadOrderStatusAsync);
            }
            else
            {

                RefreshOrdersGridAction loadOrderStatusAsync = new RefreshOrdersGridAction(LoadOrdersGrid);
                result = this.BeginInvoke(loadOrderStatusAsync);
            }
            this.EndInvoke(result);
            if (result.IsCompleted)
            {
                if (sp != null)
                {
                    sp.Hide();
                    sp.Close();
                }
            }

        }
        /// <summary>
        /// Load Orders Status in safe thread
        /// </summary>
        void RunLoadOrderStatusInThread()
        {
            LoadOrderStatusAction loadOrderStatusAsync = new LoadOrderStatusAction(BindOrderStatuses);
            this.BeginInvoke(loadOrderStatusAsync);
        }
        /// <summary>
        /// Load Orders for Admin in safe Thread
        /// </summary>
        void RunLoadOrdersforAdminViewInThread()
        {
            LoadOrdersAction loadOrdersAsync = new LoadOrdersAction(LoadOrdersGridforAdminView);
            this.BeginInvoke(loadOrdersAsync);
        }
        /// <summary>
        /// Bind Store Names
        /// </summary>
        private void BindStoreNames()
        {
            try
            {
                stores = adminProxy.GetStores(USERNAME, PASSWORD);

                var data = stores.Select(store => new
                {
                    Name = store["name"],
                    ID = store["store_id"]
                }).ToList();
                cbxStoreNames.DataSource = data;

                cbxStoreNames.DisplayMember = "Name";
                cbxStoreNames.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
                return;
            }

        }
        /// <summary>
        /// Invoke processing delayed orders in safe thread
        /// </summary>
        void RunProcessedDelayedOrdersInThread()
        {
            LoadOrdersAction loadOrdersAsync = new LoadOrdersAction(CheckProcessingDelayedOrders);
            this.BeginInvoke(loadOrdersAsync);

        }
        /// <summary>
        /// Check processing delayed orders and notify user
        /// </summary>
        void CheckProcessingDelayedOrders()
        {
            try
            {
                XmlRpcStruct[] pendingOrders = proxy.GetOrders(STOREID, STORESECURITYKEY, OrderStatusConstants.Processing, "20");
                var delayedOrders = (from o in pendingOrders.ToList()
                                     where Convert.ToDateTime(o["date_modified"]) <= DateTime.Now.AddMinutes(30)
                                     select o).ToList();

                if (delayedOrders.ToList().Count > 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "There are some orders not yet completed", ToolTipIcon.Info);
                }
                toolStripStatusLabel1.Text = "Status: Connected";
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
            }
            finally
            {
                processingOrdersDelayedThread.Abort();
            }
        }
        /// <summary>
        /// Invoke loading pending orders in a safe thread
        /// </summary>
        void RunLoadOrdersInThread()
        {
            LoadOrdersAction loadOrdersAsync = new LoadOrdersAction(LoadTodaysOrders);
            this.BeginInvoke(loadOrdersAsync);

        }
        /// <summary>
        /// Bind Order Statuses
        /// </summary>
        private void BindOrderStatuses()
        {
            cbxOrderStatus.SelectedIndexChanged -= new EventHandler(cbxOrderStatus_SelectedIndexChanged);
            try
            {
                OrderStatuses = proxy.GetOrderStatuses(STOREID, STORESECURITYKEY);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "Failed to download data from server, please check VFOS System Settings or contact administrator", "System Error", ToolTipIcon.Error);
                return;
            }
            XmlRpcStruct lastItem = new XmlRpcStruct();

            var statuses = OrderStatuses.ToList();

            lastItem["name"] = "All";
            lastItem["order_status_id"] = "0";

            statuses.Insert(0, lastItem);
            var data = statuses.Select(store => new
            {
                Name = store["name"],
                ID = store["order_status_id"]
            }).ToList();
            cbxOrderStatus.SelectedIndexChanged += new EventHandler(cbxOrderStatus_SelectedIndexChanged);
            cbxOrderStatus.DataSource = data;
            cbxOrderStatus.DisplayMember = "Name";
            cbxOrderStatus.ValueMember = "ID";
            try
            {
                cbxOrderStatus.SelectedIndex = 0;
            }
            catch (Exception e2)
            {

            }
            InternetStatusNotification("Status: Connected");
            if (LoadOrderStatusThread != null && LoadOrderStatusThread.IsAlive)
            {
                LoadOrderStatusThread.Abort();
            }

        }
        /// <summary>
        /// Invoke Internet Status Update Notification Thread
        /// </summary>
        /// <param name="status"></param>
        private void InternetStatusNotification(string status)
        {
            StatusBarUpdateThread = new Thread(() => RunStatusBarUpdateThread(status));
            StatusBarUpdateThread.IsBackground = true;

            StatusBarUpdateThread.Start();
        }
        /// <summary>
        /// Update Screen when internet connected automatically
        /// </summary>
        private void WhenInternectConnected()
        {
            InternetStatusNotification("Status: Connected");

            if (IsAdmin)
            {

                BindStoreNames();
            }

            BindOrderStatuses();

            scheduler = new System.Timers.Timer();
            scheduler.Interval = DOWNLOADINTERVAL * 60000;
            scheduler.Elapsed += new System.Timers.ElapsedEventHandler(scheduler_Elapsed);
            scheduler.Start();
            gridRefresher = new System.Timers.Timer();
            gridRefresher.Interval = DOWNLOADINTERVAL * 60000;
            gridRefresher.Elapsed += new System.Timers.ElapsedEventHandler(gridRefresher_Elapsed);
            gridRefresher.Start();
        }
        #endregion

        public OrderProcessClient()
        {
            InitializeComponent();
            // this.WindowState = FormWindowState.Minimized;
            // this.Hide();
            Opacity = 0;
        }
        #region "Form Events"

        void gridRefresher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (RefreshOrdersGridThread != null && RefreshOrdersGridThread.IsAlive)
            {
                RefreshOrdersGridThread.Abort();
                if (sp != null)
                {
                    sp.Close();
                }
            }
            RefreshOrdersGridThread = new Thread(new ThreadStart(RunLoadOrderGridInThread));
            RefreshOrdersGridThread.IsBackground = true;
            RefreshOrdersGridThread.Start();
        }

        private void OrderProcess_Load(object sender, EventArgs e)
        {

            splash.Show();
            this.Shown += new EventHandler(OrderProcessClient_Shown);
            this.Layout += new LayoutEventHandler(OrderProcessClient_Layout);
            cbxStoreStatus.SelectedIndex = 0;
            tbOrdersList.Width = Width - 280;
            tbOrdersList.Height = Height - 190;
            gvOrdersList.Height = tbOrdersList.Height - 80;
            gvOrdersList.Width = tbOrdersList.Width - 20;
           // rctTitle.Width = Width - 22;
            pnlUserModes.Left = Width - 925;
          //  lblAppTitle.Width = rctTitle.Width;
            pnlAdminCtrls.Left = tbOrdersList.Width - 585;
            helpTips1.Left = tbOrdersList.Width + 5;
            helpTips1.Height = tbOrdersList.Height - 10;
            notifyIcon1.Click += new EventHandler(notifyIcon1_Click);
            this.FormClosed += new FormClosedEventHandler(OrderProcessClient_FormClosed);
            this.FormClosing += new FormClosingEventHandler(OrderProcessClient_FormClosing);
            gvOrdersList.KeyDown += new KeyEventHandler(gvOrdersList_KeyDown);

            LoadAppSettings();
            cbxUserModes.SelectedIndexChanged -= new EventHandler(cbxUserModes_SelectedIndexChanged);
            cbxUserModes.SelectedIndex = 0;
            cbxUserModes.SelectedIndexChanged += new EventHandler(cbxUserModes_SelectedIndexChanged);
            IsAdmin = false;
            cbxStoreNames.Visible = false;
            lblStoreName.Visible = false;

            tbOrdersList.Selecting += new TabControlCancelEventHandler(tbOrdersList_Selecting);
            //  gvOrdersList.RowsAdded += new DataGridViewRowsAddedEventHandler(gvOrdersList_RowsAdded);
            proxy.Url = XMLRPCAPIURL;

            INTERNET = AppUtilities.CheckInternetConnection();
            if (!INTERNET)
            {
                toolStripStatusLabel1.Text = "Status: Disconnected";
                notifyIcon1.ShowBalloonTip(3000, "VFOS Alert", "Internet connection is not available, please check your network settings", ToolTipIcon.Warning);
                scheduler = new System.Timers.Timer();
                scheduler.Interval = 3000;
                scheduler.Elapsed += new System.Timers.ElapsedEventHandler(scheduler_Elapsed);
                scheduler.Start();

            }
            else
            {
                WhenInternectConnected();
            }
        }

        void OrderProcessClient_Shown(object sender, EventArgs e)
        {
            splash.Hide();
            splash.Close();
            //this.Show();
            Opacity = 100;
            // this.WindowState = FormWindowState.Normal;
        }

        void OrderProcessClient_Layout(object sender, LayoutEventArgs e)
        {
            // splash.Hide();
            // splash.Close();                      
        }

        void splashTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (LoadOrdersGridThread == null || !LoadOrdersGridThread.IsAlive)
            {
                LoadOrdersGridThread = new Thread(new ThreadStart(RunLoadOrderGridInThread));
                LoadOrdersGridThread.IsBackground = true;
                LoadOrdersGridThread.Start();

            }

        }

        void gvOrdersList_KeyDown(object sender, KeyEventArgs e)
        {

            if (gvOrdersList.SelectedRows.Count > 0)
            {
                int colIndex = 0;
                if (e.Control && e.KeyCode == Keys.U)
                {
                    colIndex = 6;
                }
                if (e.Control && e.KeyCode == Keys.P)
                {
                    colIndex = 7;
                }
                if (e.Control && e.KeyCode == Keys.V)
                {
                    colIndex = 8;
                }
                if (colIndex != 0)
                {
                    DataGridViewRow row = gvOrdersList.SelectedRows[0];
                    DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(colIndex, row.Index);
                    gvOrdersList_CellClick(gvOrdersList, args);
                }
            }
        }

        void OrderProcessClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (IsAdmin)
            {
                adminProxy.SetStoreStatus(STOREID, STORESECURITYKEY, "0");
            }
            else
            {
                proxy.SetStoreStatus(STOREID, STORESECURITYKEY, "0");
            }
        }

        void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        void OrderProcessClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        void gvOrdersList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {


        }

        void tbOrdersList_Selecting(object sender, TabControlCancelEventArgs e)
        {
            CurrentTabIndex = e.TabPageIndex;
            if (IsAdmin)
            {
                if (e.TabPageIndex == 1)
                {
                    if (LoadAdminDashboardInThread != null && LoadAdminDashboardInThread.IsAlive)
                    {
                        LoadAdminDashboardInThread.Abort();
                    }
                    LoadAdminDashboardInThread = new Thread(new ThreadStart(RunAdminDashBoardInThread));
                    LoadAdminDashboardInThread.IsBackground = true;
                    LoadAdminDashboardInThread.Start();

                }

            }
            else
            {
                if (e.TabPageIndex == 2 || e.TabPageIndex == 1)
                {
                    MessageBox.Show("Please login as administrator and try accessing admin functionality", "Warning", MessageBoxButtons.OK);
                    e.Cancel = true;

                }
            }
        }

        void scheduler_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool iStatus = INTERNET;
            INTERNET = AppUtilities.CheckInternetConnection();
            if (INTERNET)
            {
                InternetStatusNotification("Status: Connected");
                scheduler.Interval = DOWNLOADINTERVAL * 60000;
                if (OrderStatuses != null)
                {
                    if (OrderStatuses.Count() == 0)
                    {
                        BindOrderStatuses();
                    }
                }
                if (IsAdmin)
                {
                    if (stores != null)
                    {
                        if (stores.Count() == 0)
                        {
                            BindStoreNames();
                        }
                    }
                }
                if (iStatus != INTERNET)
                {
                    if (cbxOrderStatus.Items.Count == 0)
                    {


                        if (LoadOrderStatusThread != null && LoadOrderStatusThread.IsAlive)
                        {
                            return;
                        }
                        LoadOrderStatusThread = new Thread(new ThreadStart(RunLoadOrderStatusInThread));
                        LoadOrderStatusThread.IsBackground = true;
                        LoadOrderStatusThread.Start();
                    }
                    else
                    {
                        if (LoadOrdersGridThread != null && LoadOrdersGridThread.IsAlive)
                        {
                            return;
                        }
                        LoadOrdersGridThread = new Thread(new ThreadStart(RunLoadOrderGridInThread));
                        LoadOrdersGridThread.IsBackground = true;
                        LoadOrdersGridThread.Start();
                    }
                    return;
                }



                if (ordersDownloadThread != null && ordersDownloadThread.IsAlive)
                {
                    return;
                }
                ordersDownloadThread = new Thread(new ThreadStart(RunLoadOrdersInThread));
                ordersDownloadThread.IsBackground = true;
                ordersDownloadThread.Start();
                if (processingOrdersDelayedThread != null && processingOrdersDelayedThread.IsAlive)
                {
                    return;
                }
                processingOrdersDelayedThread = new Thread(new ThreadStart(RunProcessedDelayedOrdersInThread));
                processingOrdersDelayedThread.IsBackground = true;
                processingOrdersDelayedThread.Start();

            }
            else
            {

                InternetStatusNotification("Status: Disconnected");
                scheduler.Interval = 3000;
            }
        }

        void gvOrdersList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!INTERNET)
            {
                toolStripStatusLabel1.Text = "Status: Disconnected";
                notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "Lost Internet Connection", ToolTipIcon.Warning);
                return;
            }
            DataGridView gvOrdersList = sender as DataGridView;
            if (e.ColumnIndex < 0) return;
            if (e.RowIndex < 0) return;
            if (gvOrdersList.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != -1)
            {
                DataGridViewButtonColumn col = gvOrdersList.Columns[e.ColumnIndex] as DataGridViewButtonColumn;
                if (gvOrdersList.Rows.Count == 0)
                {
                    return;
                }
                if (gvOrdersList.Columns.Count < 5)
                {
                    return;
                }
                DataGridViewRow currRow = gvOrdersList.Rows[e.RowIndex];
                currRow.Cells[e.ColumnIndex].Selected = false;
                col.Selected = false;
                gvOrdersList.ClearSelection();
                gvOrdersList.Refresh();
                try
                {

                    string orderId = currRow.Cells[0].Value.ToString();
                    string oStatus = currRow.Cells[2].Value.ToString();

                    if (col.HeaderText == "Print")
                    {
                        ProcessPrint(orderId, oStatus);
                    }
                    if (col.HeaderText == "View")
                    {
                        WINDOWACTIVE = false;
                        ViewOrder form = new ViewOrder(orderId, IsAdmin, oStatus, "");
                        form.ShowDialog(this);
                        WINDOWACTIVE = true;
                    }
                    if (col.HeaderText == "Update")
                    {
                        WINDOWACTIVE = false;
                        frmUpdateOrder form = new frmUpdateOrder(orderId, OrderStatuses, oStatus, IsAdmin);
                        form.ShowDialog(this);
                        WINDOWACTIVE = true;
                        if (form.DialogResult == DialogResult.OK)
                        {

                            notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "Order status updated", ToolTipIcon.Info);
                            if (LoadOrdersGridThread != null || LoadOrdersGridThread.IsAlive)
                            {
                                LoadOrdersGridThread.Abort();
                            }
                            LoadOrdersGridThread = new Thread(new ThreadStart(RunLoadOrderGridInThread));
                            LoadOrdersGridThread.IsBackground = true;
                            LoadOrdersGridThread.Start();
                        }
                        else
                        {
                            gvOrdersList.ClearSelection();
                            gvOrdersList.Refresh();
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
        void gvOrdersList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbxOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (INTERNET)
            {

                if (LoadOrdersGridThread == null || !LoadOrdersGridThread.IsAlive)
                {
                    LoadOrdersGridThread = new Thread(new ThreadStart(RunLoadOrderGridInThread));
                    LoadOrdersGridThread.IsBackground = true;
                    LoadOrdersGridThread.Start();

                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Status: Disconnected";
                notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "Lost Internet Connection, please check network settings", ToolTipIcon.Warning);
            }
        }



        private void cbxStoreNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxOrderStatus_SelectedIndexChanged(cbxOrderStatus, new EventArgs());
        }

        private void cbxUserModes_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (scheduler != null)
            {
                scheduler.Stop();
            }
            if (ordersDownloadThread != null && ordersDownloadThread.IsAlive)
            {
                ordersDownloadThread.Abort();

            }

            if (processingOrdersDelayedThread != null && processingOrdersDelayedThread.IsAlive)
            {
                processingOrdersDelayedThread.Abort();

            }
            CurrentTabIndex = 0;
            if (cbxUserModes.SelectedIndex == 1)
            {
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.ShowDialog(this);
                if (loginScreen.DialogResult == DialogResult.No || loginScreen.DialogResult == DialogResult.None || loginScreen.DialogResult == DialogResult.Cancel)
                {
                    cbxUserModes.SelectedIndex = 0;
                    return;
                }
                TabPage tbPage2 = new TabPage(" Dashboard ");
                DashBoard dbCtrl = new DashBoard();
                dbCtrl.Width = tbOrdersList.Width;
                dbCtrl.Height = tbOrdersList.Height;
                tbPage2.Controls.Add(dbCtrl);
                tbOrdersList.TabPages.Add(tbPage2);
                TabPage tbPage3 = new TabPage(" Settings ");
                SystemSettingsControl ssCtrl = new SystemSettingsControl();
                ssCtrl.Width = tbOrdersList.Width;
                ssCtrl.Height = tbOrdersList.Height;
                tbPage3.Controls.Add(ssCtrl);
                tbOrdersList.TabPages.Add(tbPage3);
                SwitchToAdminView();
                cbxStoreNames.Visible = true;
                lblStoreName.Visible = true;

            }
            else
            {

                if (tbOrdersList.TabPages.Count == 3)
                {
                    tbOrdersList.TabPages.RemoveAt(1);
                    tbOrdersList.TabPages.RemoveAt(1);
                }

                IsAdmin = false;
                if (cbxOrderStatus.Items.Count > 0)
                {
                    cbxOrderStatus.SelectedIndex = 0;
                }

                cbxStoreNames.Visible = false;
                lblStoreName.Visible = false;
                tbOrdersList.SelectedIndex = 0;
                WhenInternectConnected();
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void cbxStoreStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string storeStatus = "1";
            string statusLabel = cbxStoreStatus.Text;
            switch (cbxStoreStatus.SelectedIndex)
            {
                case 1: storeStatus = "1"; // online
                    break;
                case 2: storeStatus = "0";//offline
                    break;
                case 3: storeStatus = "2"; //idle
                    break;
                case 4: storeStatus = "3";// busy
                    break;
            }
            try
            {
                if (IsAdmin)
                {
                    adminProxy.SetStoreStatus(STOREID, STORESECURITYKEY, storeStatus);
                }
                else
                {
                    proxy.SetStoreStatus(STOREID, STORESECURITYKEY, storeStatus);
                }
                notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "Store status has been set to " + statusLabel + " successfully", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1000, "VFOS Alert", "Failed to change store status, please try again or later", ToolTipIcon.Info);
            }

        }
        #endregion
    }
}


    


