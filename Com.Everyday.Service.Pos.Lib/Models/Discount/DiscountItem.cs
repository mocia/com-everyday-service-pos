using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Everyday.Service.Pos.Lib.Models.Discount
{
    public class DiscountItem : StandardEntity
    {
        [MaxLength(255)]
        public string RealizationOrder { get; set; }
        public virtual ICollection<DiscountDetail> Details { get; set; }
        public virtual int DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }
    }
}
