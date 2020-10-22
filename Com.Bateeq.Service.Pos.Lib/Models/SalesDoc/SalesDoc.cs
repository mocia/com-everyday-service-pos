using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.Models.SalesDoc
{
    public class SalesDoc : StandardEntity
    {
        [MaxLength(255)]
        public string BankCode { get; set; }
        public int BankId { get; set; }
        [MaxLength(255)]
        public string BankName { get; set; }
        [MaxLength(255)]
        public string BankCardCode { get; set; }
        public int BankCardId { get; set; }
        [MaxLength(255)]
        public string BankCardName { get; set; }
        [MaxLength(255)]
        public string Card { get; set; }
        public double CardAmount { get; set; }
        [MaxLength(255)]
        public string CardName { get; set; }
        [MaxLength(255)]
        public string CardNumber { get; set; }
        [MaxLength(255)]
        public string CardTypeCode { get; set; }
        public int CardTypeId { get; set; }
        [MaxLength(255)]
        public string CardTypeName { get; set; }
        public double CashAmount { get; set; }
        [MaxLength(255)]
        public string Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Discount { get; set; }
        public bool isReturn { get; set; }
        public bool isVoid { get; set; }
        public double GrandTotal { get; set; }
        [MaxLength(255)]
        public string PaymentType { get; set; }
        [MaxLength(255)]
        public string Reference { get; set; }
        [MaxLength(255)]
        public string Remark { get; set; }
        public int Shift { get; set; }
        [MaxLength(255)]
        public string StoreCode { get; set; }
        [MaxLength(255)]
        public string StoreCategory { get; set; }
        [MaxLength(255)]
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        [MaxLength(255)]
        public string StoreStorageCode { get; set; }
        [MaxLength(255)]
        public string StoreStorageName { get; set; }
        public int StoreStorageId { get; set; }
        public double SubTotal { get; set; }
        public double TotalProduct { get; set; }
        public double VoucherValue { get; set; }
        [MaxLength(255)]
        public string UId { get; set; }

        public virtual ICollection<SalesDocDetail> Details { get; set; }
    }
}
