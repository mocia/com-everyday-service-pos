using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Everyday.Service.Pos.Lib.Models.Discount
{
    public class DiscountStore : StandardEntity
    {
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]
        public string Code { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string OnlineOffline { get; set; }
        [MaxLength(255)]
        public string SalesCategory { get; set; }
        [MaxLength(255)]
        public string StorageCode { get; set; }
        [MaxLength(255)]
        public string StorageId { get; set; }
        [MaxLength(255)]
        public string StorageIsCentral { get; set; }
        [MaxLength(255)]
        public string StorageName { get; set; }
        [MaxLength(255)]
        public string StoreArea { get; set; }
        public int StoreId { get; set; }
        [MaxLength(255)]
        public string StoreCategory { get; set; }
        [MaxLength(255)]
        public string StoreWide { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }
        public int DiscountId { get; set; }
        [MaxLength(255)]
        public string DiscountCode { get; set; }

    }
}
