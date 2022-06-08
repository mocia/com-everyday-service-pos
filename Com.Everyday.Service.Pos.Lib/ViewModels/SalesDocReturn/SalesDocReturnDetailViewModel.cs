
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Everyday.Service.Pos.Lib.ViewModels.NewIntegrationViewModel;
using Com.Everyday.Service.Pos.Lib.ViewModels.SalesDoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Everyday.Service.Pos.Lib.ViewModels.SalesDocReturn
{
    public class SalesDocReturnDetailViewModel : BasicViewModel
    {
        public SalesDocDetailViewModel item { get; set; }
        public string itemCode { get; set; }
        public int itemId { get; set; }
        public double margin { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public double quantityPurchase { get; set; }
        public double specialDiscount { get; set; }
        public double total { get; set; }
        public double discount1 { get; set; }
        public double discount2 { get; set; }
        public double discountNominal { get; set; }
        public bool isReturn { get; set; }
        public List<SalesDocDetailViewModel> returnItems { get; set; }

    }
}
