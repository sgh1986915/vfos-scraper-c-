using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFPO
{
    public struct OrderStatusConstants
    {
        public const string Pending = "1";
        public const string Processing = "2";
        public const string Shipped = "3";
        public const string Complete = "5";
        public const string Cancelled = "7";
         public const string Denied = "8";
         public const string CanceledReversal = "9";
         public const string Failed = "10";
         public const string Refunded = "11";
         public const string Reversed = "12";
         public const string Chargeback = "13";
         public const string Expired = "14";
         public const string Processed = "15";
         public const string Voided = "16";
         public const string NotProcessed = "";

    }
}
