using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel
{
    public class ItemViewModel
    {
        public string ArticleRealizationOrder { get; set; }

        public string code { get; set; }


        public double DomesticCOGS { get; set; }


        public double DomesticRetail { get; set; }

        public double DomesticSale { get; set; }

        public double DomesticWholeSale { get; set; }

        public long _id { get; set; }

        
        public string name { get; set; }

        
        public string Size { get; set; }

        
        public string Uom { get; set; }
    }
    
}
