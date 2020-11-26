using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels
{
    public class SalesVoidReportViewModel : BasicViewModel
    {
        public DateTimeOffset date { get; set; }
        public string code { get; set; }
        public double grandTotal { get; set; }
        public int storeId { get; set; }
        public string storeCode { get; set; }
        public string storeName { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemSize { get; set; }
        public double Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int shift { get; set; }        
        public string reference { get; set; }
        public string remark { get; set; }
        public bool isReturn { get; set; }
        public bool isVoid { get; set; }
    }
}
