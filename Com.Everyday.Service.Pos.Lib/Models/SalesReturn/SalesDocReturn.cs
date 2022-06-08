using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Everyday.Service.Pos.Lib.Models.SalesReturn
{
    public class SalesDocReturn : StandardEntity
    {
        [MaxLength(255)]
        public string Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsVoid { get; set; }
        public int SalesDocId { get; set; }
        [MaxLength(255)]
        public string SalesDocCode { get; set; }
        public bool SalesDocIsReturn { get; set; }
        public int SalesDocReturnId { get; set; }
        [MaxLength(255)]
        public string SalesDocReturnCode { get; set; }
        public bool SalesDocReturnIsReturn { get; set; }
        [MaxLength(255)]
        public string StoreCode { get; set; }
        public int StoreId { get; set; }
        [MaxLength(255)]
        public string StoreName { get; set; }
        [MaxLength(255)]
        public string StoreStorageName { get; set; }
        [MaxLength(255)]
        public string StoreStorageCode { get; set; }
        public int StoreStorageId { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }

        public virtual ICollection<SalesDocReturnDetail> Details { get; set; }

    }
}
