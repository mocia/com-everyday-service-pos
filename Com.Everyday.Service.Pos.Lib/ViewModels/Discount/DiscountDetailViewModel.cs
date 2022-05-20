using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Everyday.Service.Pos.Lib.ViewModels.Discount
{
    public class DiscountDetailViewModel : BasicViewModel
    {
        public ItemViewModelRead dataDestination { get; set; }
        public float DomesticCOGS { get; set; }

        public float DomesticWholesale { get; set; }
    
        public float DomesticRetail { get; set; }

        public float DomesticSale { get; set; }

        public float InternationalCOGS { get; set; }

        public float InternationalWholesale { get; set; }

        public float InternationalRetail { get; set; }

        public float InternationalSale { get; set; }
        //public float price { get; set; }
        //public string name { get; set; }
        //public string size { get; set; }
        //public string uom { get; set; }
        //public string uid { get; set; }
    }
    public class ItemViewModelRead
    {
        public int _id { get; set; }
        public string code { get; set; }

        public string name { get; set; }

        public string Description { get; set; }

        public string Uom { get; set; }

        public string Tags { get; set; }

        public string Remark { get; set; }

        public string ArticleRealizationOrder { get; set; }

        public string Size { get; set; }

        public string ImagePath { get; set; }

        public string ImgFile { get; set; }
    }
}
