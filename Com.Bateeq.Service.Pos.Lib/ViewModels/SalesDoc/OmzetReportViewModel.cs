using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc
{
    public class OmzetReportViewModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemSize { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double Discount1 { get; set; }
        public double Discount2 { get; set; }
        public double DiscountNominal { get; set; }
        public double SpecialDiscount { get; set; }
        public double Margin { get; set; }
        public string Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsReturn { get; set; }
        public bool IsVoid { get; set; }
        public double Discount { get; set; }
        public string PaymentType { get; set; }
        public string Card { get; set; }
        public string CardTypeName { get; set; }
        public double CashAmount { get; set; }
        public double CardAmount { get; set; }
        public string BankName { get; set; }
        public string BankCardName { get; set; }
        public double SubTotal { get; set; }
        public string StoreName { get; set; }
        public int Shift { get; set; }
        public double Voucher { get; set; }
        public double TotalOmzetBruto { get; set; }
        public double TotalOmzetNetto { get; set; }
    }
}
