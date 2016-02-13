using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace VFPO
{


    [XmlRpcUrl("http://www.beautyeducationdirectory.com/api/index.php")]
    public interface IXMLRPCProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("getOrders")]
        XmlRpcStruct[] GetOrders(string storeid, string securitykey, string order_status_id, string limit);

        [XmlRpcMethod("getOrder")]
        XmlRpcStruct GetOrder(string storeId, string securityId, string orderId);

        [XmlRpcMethod("updateOrder")]
        string UpdateOrder(string storeId, string securityId, string orderId, string order_status_id, string comment, string notify);
        [XmlRpcMethod("getOrderStatuses")]
        XmlRpcStruct[] GetOrderStatuses(string storeId, string securityId);
        [XmlRpcMethod("setStoreStatus")]
        string SetStoreStatus(string storeapiid, string securitykey, string storestatus);
    }
    public struct OrderStatus
    {
        public string order_status_id { set; get; }
        public string name { set; get; }
    }
   
    public struct Order
    {
        public string order_id { set; get; }

        public string customer{ set; get; }

        public string status { set; get; }
        public string total { set; get; }
        public string currency_code { set; get; }

        public string date_added { set; get; }

        public string date_modified { set; get; }
    }
}
