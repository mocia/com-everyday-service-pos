using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel
{
    public class StoreViewModels : BasicViewModel
    {
        public string address { get; set; }
        public string city { get; set; }
        public DateTimeOffset closedDate { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public float monthlyTotalCost { get; set; }
        public string name { get; set; }
        public string onlineOffline { get; set; }
        public DateTimeOffset openedDate { get; set; }
        public string pic { get; set; }
        public string phone { get; set; }
        public float salesCapital { get; set; }
        public string salesCategory { get; set; }
        public float salesTarget { get; set; }
        public string status { get; set; }
        public string storeArea { get; set; }
        public string storeCategory { get; set; }
        public string storeWide { get; set; }
    }
}
