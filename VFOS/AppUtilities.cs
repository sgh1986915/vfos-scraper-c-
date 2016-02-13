using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;
using System.Management;
using System.Net.NetworkInformation;

namespace VFPO
{
    public static class AppUtilities
    {
        /// <summary>
        /// Checks whether order is printing or not. if already printing then same job does not send to printer again
        /// </summary>
        /// <param name="jobName">print job name</param>
        /// <returns>print job status</returns>
        public static PrintJobStatus IsDocumentPrinting(string jobName)
        {
            PrintQueue pQueue = LocalPrintServer.GetDefaultPrintQueue();
            PrintJobInfoCollection jobs = pQueue.GetPrintJobInfoCollection();
            foreach (PrintSystemJobInfo printJob in jobs)
            {
                if (printJob.Name == jobName)
                {
                    return PrintJobStatus.Printing;
                }
            }
            return PrintJobStatus.None;

        }
        /// <summary>
        /// Check if internet connection is available or not
        /// </summary>
        /// <returns></returns>
        public static bool CheckInternetConnection()
        {
            bool pingStatus = false;

            using (Ping p = new Ping())
            {
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 90000;

                try
                {
                    PingReply reply = p.Send("google.com", timeout, buffer);
                    pingStatus = (reply.Status == IPStatus.Success);
                }
                catch (Exception)
                {
                    pingStatus = false;
                }
            }

            return pingStatus;

        }
     
        /// <summary>
        /// Checks whether configured printer is connected to computer or not and return status
        /// </summary>
        /// <returns>CONNECTED if printer is online or else message</returns>
        public static string PrinterStatus(string printerName)
        {
            // Set management scope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();
            string result = string.Empty;
            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
           
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(printerName))
                {
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        result = "Your Plug-N-Play printer is not connected.";
                        break;
                    }
                    else
                    {
                        // printer is not offline
                        result = "CONNECTED";
                        break;
                    }
                }
            }
            return result;
        }
    }
}
