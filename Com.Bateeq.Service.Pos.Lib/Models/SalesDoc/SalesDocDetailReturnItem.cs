using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.Models.SalesDoc
{
    public class SalesDocDetailReturnItem : StandardEntity
    {
        public double Discount1 { get; set; }
        public double Discount2 { get; set; }
        public double DiscountNominal { get; set; }
        public bool isReturn { get; set; }
        [MaxLength(255)]
        public string ItemArticleRealizationOrder { get; set; }

        [MaxLength(255)]
        public string ItemCode { get; set; }


        public double ItemDomesticCOGS { get; set; }


        public double ItemDomesticRetail { get; set; }

        public double ItemDomesticSale { get; set; }

        public double ItemDomesticWholeSale { get; set; }

        public long ItemId { get; set; }

        [MaxLength(255)]
        public string ItemName { get; set; }

        [MaxLength(255)]
        public string ItemSize { get; set; }

        [MaxLength(255)]
        public string ItemUom { get; set; }
        public double Margin { get; set; }
        public double Price { get; set; }
        [MaxLength(255)]
        public string PromoCode { get; set; }
        [MaxLength(255)]
        public string PromoName { get; set; }
        public int PromoId { get; set; }
        public double Quantity { get; set; }
        [MaxLength(255)]
        public string Size { get; set; }
        public double SpesialDiscount { get; set; }
        public double Total { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }

        public int SalesDocDetailId { get; set; }
    }
}
