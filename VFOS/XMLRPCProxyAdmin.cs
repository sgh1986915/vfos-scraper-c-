using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace VFPO
{
      
    [XmlRpcUrl("http://www.beautyeducationdirectory.com/api/index.php")]
    public interface IXMLRPCProxyAdmin : IXmlRpcProxy
    {
        [XmlRpcMethod("SAgetOrders")]
        XmlRpcStruct[] GetOrders(string username, string password, string order_status_id, string limit, string storeid);

        [XmlRpcMethod("SAgetOrder")]
        XmlRpcStruct GetOrder(string username, string password, string orderId);

        [XmlRpcMethod("getStores")]
        XmlRpcStruct[] GetStores(string username, string password);

        [XmlRpcMethod("SAupdateOrder")]
        string UpdateOrder(string username, string password, string orderId, string order_status_id, string notify,string comment);
         [XmlRpcMethod("getOrderStatuses")]
        XmlRpcStruct[] GetOrderStatuses(string storeId, string securityId);
         [XmlRpcMethod("setStoreStatus")]
         string SetStoreStatus(string storeapiid, string securitykey, string storestatus);

    }
   
}
