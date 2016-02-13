using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WinForms;
using CookComputing.XmlRpc;
using System.Data;

namespace VFPO
{
    public static class CommonMethods
    {
        public static void PrepareOrderReport(ref LocalReport report, XmlRpcStruct orderDetails,string status,ref double noOfRows)
        {
            DataTable dt = new DataTable("DataSet1");
            dt.Columns.Add("name");
            dt.Columns.Add("model");
            dt.Columns.Add("quantity");
            dt.Columns.Add("price");
            dt.Columns.Add("total");
            dt.Columns.Add("optionname");
            dt.Columns.Add("optionvalue");
            string currencySymbol = Utilities.TryGetCurrencySymbol(orderDetails["currency_code"].ToString());
            if (orderDetails["order_products"] != null)
            {
                // for (int i = 0; i < 4; i++) // for testing page height in print for big orders
                {
                    foreach (XmlRpcStruct product in (Object[])orderDetails["order_products"])
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = product["name"];
                        dr[1] = product["model"];
                        dr[2] = product["quantity"];
                        string price = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(product["price"]))).ToString();
                        string total = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(product["total"]))).ToString();
                        dr[3] = currencySymbol + price;
                        dr[4] = currencySymbol + total;
                        Object[] options = (Object[])product["option"];

                        foreach (XmlRpcStruct option in options)
                        {
                            dr = dt.NewRow();
                            dr[0] = product["name"];
                            dr[1] = product["model"];
                            dr[2] = product["quantity"];
                            
                            dr[3] = currencySymbol + price;
                            dr[4] = currencySymbol + total;
                            dr[5] = option["name"];
                            dr[6] = option["value"];
                            dt.Rows.Add(dr);
                        }
                    }
                }

            }


            ReportParameterCollection parameters = new ReportParameterCollection();
            parameters.Add(new ReportParameter("pOrderedDate", "Order Date: " + orderDetails["date_added"]));
            parameters.Add(new ReportParameter("pDeliveryMethod", "Delivery Method: " + orderDetails["shipping_method"] + "\r"));
            parameters.Add(new ReportParameter("pPaymentMethod", "Payment Method: " + orderDetails["payment_method"] + "\r"));
            parameters.Add(new ReportParameter("pOrderId", "Order Id: " + orderDetails["order_id"] + "\r"));
            parameters.Add(new ReportParameter("pEmail", "Email: " + orderDetails["email"] + "\r"));
            parameters.Add(new ReportParameter("pTelephone", "Telephone: " + orderDetails["telephone"] + "\r"));
            parameters.Add(new ReportParameter("pIPAddress", "Ip Address: " + orderDetails["ip"] + "\r"));
            parameters.Add(new ReportParameter("pStatus", "Order Status: " + status + "\r"));
            StringBuilder paymentAddress = new StringBuilder();
            paymentAddress.AppendLine(orderDetails["payment_lastname"] + " " + orderDetails["payment_firstname"]);
            if (!string.IsNullOrEmpty(orderDetails["payment_company"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_company"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["payment_address_1"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_address_1"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["payment_address_2"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_address_2"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["payment_city"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_city"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["payment_postcode"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_postcode"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["payment_country"].ToString()))
            {
                paymentAddress.AppendLine(orderDetails["payment_country"].ToString());
            }

            StringBuilder shippingAddress = new StringBuilder();
            shippingAddress.AppendLine(orderDetails["shipping_lastname"] + " " + orderDetails["shipping_firstname"]);
            if (!string.IsNullOrEmpty(orderDetails["shipping_company"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_company"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["shipping_address_1"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_address_1"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["shipping_address_2"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_address_2"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["shipping_city"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_city"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["shipping_postcode"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_postcode"].ToString());
            }
            if (!string.IsNullOrEmpty(orderDetails["shipping_country"].ToString()))
            {
                shippingAddress.AppendLine(orderDetails["shipping_country"].ToString());
            }

            parameters.Add(new ReportParameter("pPaymentAddress", "Payment Address: " + paymentAddress.ToString()));
            parameters.Add(new ReportParameter("pDeliveryAddress", "Delivery Address: " + shippingAddress.ToString()));
            StringBuilder totals = new StringBuilder();

            foreach (XmlRpcStruct total in (Object[])orderDetails["order_totals"])
            {
                if (total["value"].ToString() != string.Empty)
                {
                    string value = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(total["value"]))).ToString();
                    totals.AppendLine(total["title"] + ": " + currencySymbol + value);
                }
                else
                {
                    totals.AppendLine(total["title"].ToString());
                }
            }
            parameters.Add(new ReportParameter("pSubTotal", totals.ToString()));            
            report.SetParameters(parameters);
            Microsoft.Reporting.WinForms.ReportDataSource datasource = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
            report.DataSources.Add(datasource);
            noOfRows = ((double)dt.Rows.Count) * 0.20839;
            
        }
    }
}
