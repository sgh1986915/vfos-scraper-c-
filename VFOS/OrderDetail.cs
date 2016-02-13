using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFPO
{
    public struct OrderDetail
    {
        public string order_id { set; get; }
        public string invoice_no { set; get; }
        public string invoice_prefix { set; get; }
        public string store_id { set; get; }
        public string store_name { set; get; }
        public string store_url { set; get; }
        public string customer_id { set; get; }
        public string customer { set; get; }
        public string customer_group_id { set; get; }
        public string firstname { set; get; }
        public string lastname { set; get; }
        public string telephone { set; get; }
        public string fax { set; get; }
        public string email { set; get; }
        public string payment_firstname { set; get; }
        public string payment_lastname { set; get; }
        public string payment_company { set; get; }
        public string payment_company_id { set; get; }
        public string payment_tax_id { set; get; }
        public string payment_address_1 { set; get; }
        public string payment_address_2 { set; get; }
        public string payment_postcode { set; get; }
        public string payment_city { set; get; }
        public string payment_zone_id { set; get; }
        public string payment_zone { set; get; }
        public string payment_zone_code { set; get; }
        public string payment_country_id { set; get; }
        public string payment_country { set; get; }
        public string payment_iso_code_2 { set; get; }
        public string payment_iso_code_3 { set; get; }
        public string payment_address_format { set; get; }
        public string payment_method { set; get; }
        public string payment_code { set; get; }
        public string shipping_firstname { set; get; }
        public string shipping_lastname { set; get; }
        public string shipping_company { set; get; }
        public string shipping_address_1 { set; get; }
        public string shipping_address_2 { set; get; }
        public string shipping_postcode { set; get; }
        public string shipping_city { set; get; }
        public string shipping_zone_id { set; get; }
        public string shipping_zone { set; get; }
        public string shipping_zone_code { set; get; }
        public string shipping_country_id { set; get; }
        public string shipping_country { set; get; }
        public string shipping_iso_code_2 { set; get; }
        public string shipping_iso_code_3 { set; get; }
        public string shipping_address_format { set; get; }
        public string shipping_method { set; get; }
        public string shipping_code { set; get; }
        public string comment { set; get; }
        public string total { set; get; }
        public int reward { set; get; }
        public string order_status_id { set; get; }
        public string affiliate_id { set; get; }
        public string affiliate_firstname { set; get; }
        public string affiliate_lastname { set; get; }
        public string commission { set; get; }
        public string language_id { set; get; }
        public string language_code { set; get; }
        public string language_filename { set; get; }
        public string language_directory { set; get; }
        public string currency_id { set; get; }
        public string currency_code { set; get; }
        public string currency_value { set; get; }
        public string ip { set; get; }
        public string forwarded_ip { set; get; }
        public string user_agent { set; get; }
        public string accept_language { set; get; }
        public string date_added { set; get; }
        public string date_modified { set; get; }
        public OrderProduct[] order_products { set; get; }
        public OrderTotal[] order_totals { set; get; }

    }
    public struct OrderProduct
    {
        public string order_product_id { set; get; }
        public string order_id { set; get; }
        public string product_id { set; get; }
        public string name { set; get; }
        public string model { get; set; }
        public string quantity { set; get; }
        public string price { set; get; }
        public string total { set; get; }
        public string tax { set; get; }
        public string reward { set; get; }
        public ProductOption[] option { set; get; }

    }
    public struct ProductOption
    {
        public string order_option_id { set; get; }
        public string order_id { set; get; }
        public string order_product_id { set; get; }
        public string product_option_id { set; get; }
        public string product_option_value_id { set; get; }
        public string name { set; get; }
        public string value { set; get; }
        public string type { set; get; }
    }
    public struct OrderTotal
    {
        public string order_total_id { set; get; }
        public string order_id { set; get; }
        public string code { set; get; }

        public string title { set; get; }
        public string text { set; get; }
        public string value { set; get; }
        public string sort_order { set; get; }
    }
}
