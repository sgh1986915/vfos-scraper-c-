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
namespace OrderProcessor
{
    public partial class OrdersList : Form
    {
        private string m_printerName;
        private string m_server;
        private string m_path;
        private string m_name;
        private Dictionary<string, string> m_parameters;
        private SizeF m_pageSize;
        private float m_marginLeft;
        private float m_marginTop;
        private float m_marginRight;
        private float m_marginBottom;
        private short m_copies;
        private IList<Stream> m_streams;
        private int m_currentPageIndex;
        private List<Stream> m_reportStreams;
        protected System.Timers.Timer scheduler;
        Thread thread;
        ITakeAwayOrders proxy = (ITakeAwayOrders)XmlRpcProxyGen.Create(typeof(ITakeAwayOrders));
        public OrdersList()
        {
            InitializeComponent();           
        }
        private DataTable LoadSalesData()
        {
            // Create a new DataSet and read sales data file 
            //    data.xml into the first DataTable.
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(@"..\..\data.xml");
            return dataSet.Tables[0];
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
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
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

        private void Print()
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

                printDoc.PrintController = printController;

                printDoc.DocumentName = "Take Away Order #";
                printDoc.PrinterSettings.Copies = 1;
                printDoc.Print();

            }
        }
        // Create a local report for Report.rdlc, load the data,
        //    export the report to an .emf file, and print it.
        private void Run()
        {
            LocalReport report = new LocalReport();
            report.ReportPath = @"..\..\PrintOrder.rdlc";
            // report.DataSources.Add(new ReportDataSource("TakeAwayOrders", LoadSalesData()));
            Export(report);
            Print();

            thread.Abort();

        }

    

        void scheduler_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // write code to poll Take Away WebSite
            //  toolStripProgressBar1.ProgressBar.Step = 0;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(new ThreadStart(Run));

                place(ref thread, 1);
                thread.Start();
            }

        }

        public void place(ref Thread p, int x)
        {
            switch (x)
            {
                case 0:
                    p.Priority = ThreadPriority.Lowest;
                    break;

                case 1:
                    p.Priority = ThreadPriority.BelowNormal;
                    break;

                case 2:
                    p.Priority = ThreadPriority.Normal;
                    break;

                case 3:
                    p.Priority = ThreadPriority.AboveNormal;
                    break;

                case 4:
                    p.Priority = ThreadPriority.Highest;
                    break;
            }
        }

        private void todaysOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void processedOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OrdersList_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'testData.Orders' table. You can move, or remove it, as needed.
            this.ordersTableAdapter.Fill(this.testData.Orders);
           
          //  proxy.AttachLogger(new XmlRpcDebugLogger());

            Order[] orderslist = { };
            Cursor = Cursors.WaitCursor;
            try
            {
                orderslist = proxy.GetOrders("STORE1", "WkVNVUIwWFdBOVAzRkxQQ1kyMERTVE9SRTE=", "0", "0");
                gvOrdersList.DataMember = "OrderProcessor.Order";
                gvOrdersList.DataSource = orderslist;
               
              //  MessageBox.Show();
                //
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");

               // HandleException(ex);
            }
            Cursor = Cursors.Default;
        }

        private void gvOrdersList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvOrdersList.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != -1)
            {
                DataGridViewRow currRow = gvOrdersList.Rows[e.RowIndex];
                try
                {
                    proxy.GetOrder("STORE1", "WkVNVUIwWFdBOVAzRkxQQ1kyMERTVE9SRTE=", "1");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
    }
}
