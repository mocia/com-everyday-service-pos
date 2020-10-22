using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.Models.Discount
{
    public class Discount : StandardEntity
    {
        [MaxLength(255)]
        public string Code { get; set; }
        public int DiscountOne { get; set; }
        public int DiscountTwo { get; set; }
        public DateTimeOffset EndDate { get; set; }
        [MaxLength(255)]
        public string Information { get; set; }
        public DateTimeOffset StartDate { get; set; }
        [MaxLength(255)]
        public string StoreCategory { get; set; }
        [MaxLength(255)]
        public string StoreName { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }

        public virtual ICollection<DiscountItem> Items { get; set; }
    }
}
