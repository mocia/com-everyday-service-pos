using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.Models.Discount
{
    public class DiscountDetail : StandardEntity
    {
        [MaxLength(255)]
        public string ArticleRealizationOrder { get; set; }
        [MaxLength(255)]
        public string Code { get; set; }
        public float DomesticCOGS { get; set; }
        public float DomesticRetail { get; set; }
        public float DomesticSale { get; set; }
        public float DomesticWholesale { get; set; }
        public float InternationalCOGS { get; set; }
        public float InternationalRetail { get; set; }
        public float InternationalSale { get; set; }
        public float InternationalWholesale { get; set; }
        public int ItemId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Size { get; set; }
        [MaxLength(255)]
        public string Uom { get; set; }
        [MaxLength(255)]
        public string Uid { get; set; }

        public virtual int DiscountItemId { get; set; }
        [ForeignKey("DiscountItemId")]
        public virtual DiscountItem DiscountItem { get; set; }
    }
}
