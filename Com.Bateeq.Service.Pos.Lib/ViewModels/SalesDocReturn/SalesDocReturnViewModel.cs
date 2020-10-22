using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Bateeq.Service.Pos.Lib.Interfaces;
using Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDocReturn
{
    public class SalesDocReturnViewModel : BasicViewModel, IValidatableObject
    {
        public DateTimeOffset date { get; set; }
        public string code { get; set; }
        public double grandTotal { get; set; }
        public SalesDetail salesDetail { get; set; }
        public double sisaBayar { get; set; }
        public Store store { get; set; }
        public int shift { get; set; }
        public double subTotal { get; set; }
        public SalesDocViewModel sales { get; set; }
        public SalesDocViewModel salesDocReturn { get; set; }
        public double subTotalRetur { get; set; }
        public double total { get; set; }
        public double totalBayar { get; set; }
        public double totalDiscount { get; set; }
        public double totalProduct { get; set; }
        public string reference { get; set; }
        public string remark { get; set; }
        public bool isReturn { get; set; }
        public bool isVoid { get; set; }
        public List<SalesDocReturnDetailViewModel> items { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (code == null || code == "")
            //{
            //    yield return new ValidationResult("code is required", new List<string> { "code" });
            //}
            if (store == null || store.Id == 0)
            {
                yield return new ValidationResult("store is required", new List<string> { "storeId" });
            }
            PosDbContext posDbContext = (PosDbContext)validationContext.GetService(typeof(PosDbContext));
            if (posDbContext.SalesDocs.Where(x=>x.Id == Convert.ToInt32(reference)).Count() == 0)
            {
                yield return new ValidationResult("salesId not Found", new List<string> { "reference" });
            }
            if (salesDetail == null)
            {
                yield return new ValidationResult("salesDetail is required", new List<string> { "salesDetail" });
            }
            if (salesDetail.paymentType == null || salesDetail.paymentType == "")
            {
                yield return new ValidationResult("paymentType is required", new List<string> { "paymentType" });
            }
            if (salesDetail.paymentType == "Card" || salesDetail.paymentType == "Partial")
            {
                if(salesDetail.card == null || salesDetail.card == "")
                {
                    yield return new ValidationResult("card is required", new List<string> { "card" });
                }
                if (salesDetail.cardType == null || salesDetail.cardType._id == 0)
                {
                    yield return new ValidationResult("cardType is required", new List<string> { "cardTypeId" });
                }
                if (salesDetail.bank == null || salesDetail.bank._id == 0)
                {
                    yield return new ValidationResult("bank is required", new List<string> { "bankId" });
                }
                if (salesDetail.bankCard == null || salesDetail.bankCard._id == 0)
                {
                    yield return new ValidationResult("bankCard is required", new List<string> { "bankCardId" });
                }
                if (string.IsNullOrWhiteSpace(salesDetail.cardNumber))
                {
                    yield return new ValidationResult("cardNumber is required", new List<string> { "cardNumber" });
                }
                if (string.IsNullOrWhiteSpace(salesDetail.cardName))
                {
                    yield return new ValidationResult("bankCard is required", new List<string> { "cardName" });
                }
            }
            if (date == null)
            {
                yield return new ValidationResult("date is not valid", new List<string> { "date" });
            }

            int itemErrorCount = 0;
            int returnitemsErrorCount = 0;
            if (this.items == null || items.Count <= 0)
            {
                yield return new ValidationResult("item is required", new List<string> { "itemcount" });
            }
            else
            {
                string itemError = "[";
                var index = 0;
                foreach (var item in items)
                {
                    index++;
                    itemError += "{";

                    if (item.item.item._id  == 0 )
                    {
                        itemErrorCount++;
                        itemError += "itemId: 'itemId is required', ";
                    }
                    else
                    {
                        for(var i = index; i < items.Count(); i++)
                        {
                            if(item.item.item._id == items[i].item.item._id)
                            {
                                itemErrorCount++;
                                itemError += "itemId : 'itemId already exists on another detail',";
                            }
                        }
                        var sales = from a in posDbContext.SalesDocs
                                    join b in posDbContext.SalesDocDetails on a.Id equals b.SalesDocId
                                    where a.Id == Convert.ToInt16(reference)
                                    select new { b.ItemId };
                        if (sales.Where(x=>x.ItemId == item.item.item._id).Count() == 0)
                        {
                            itemErrorCount++;
                            itemError += "itemId : 'itemId not exists in sales',";
                        }
                    }

                    if(item.quantity == 0)
                    {
                        itemErrorCount++;
                        itemError += "quantity : 'quantity is required',";
                    }else if(item.quantity <= 0)
                    {
                        itemErrorCount++;
                        itemError += "quantity : 'quantity must be greater than 0',";
                    }
                    else
                    {
                        var sales = from a in posDbContext.SalesDocs
                                    join b in posDbContext.SalesDocDetails on a.Id equals b.SalesDocId
                                    where a.Code == reference
                                    select new { b.Quantity, b.ItemId, a.isReturn };
                        foreach(var itemsales in sales)
                        {
                            if (!itemsales.isReturn && itemsales.ItemId == item.item.item._id)
                            {
                                if(item.quantity > itemsales.Quantity)
                                {
                                    itemErrorCount++;
                                    itemError += "quantity : 'quantity must not be greater than sales.Quantity',";
                                }
                                    
                            }
                        }
                    }

                    if(item.price == 0)
                    {
                        itemErrorCount++;
                        itemError += "price : 'price is required',";
                    }else if(item.price < 0)
                    {
                        itemErrorCount++;
                        itemError += "price : 'price must be greater than 0',";
                    }

                    if (item.discount1 < 0 || item.discount1 > 100)
                    {
                        itemErrorCount++;
                        itemError += "discount1 : 'discount1 must be greater than 0 or less than 100',";
                    }

                    if (item.discount2 < 0 || item.discount2 > 100)
                    {
                        itemErrorCount++;
                        itemError += "discount2 : 'discount2 must be greater than 0 or less than 100',";
                    }

                    if (item.discountNominal < 0 )
                    {
                        itemErrorCount++;
                        itemError += "discountNominal : 'discountNominal must be greater than 0 ',";
                    }
                    if (item.margin < 0)
                    {
                        itemErrorCount++;
                        itemError += "margin : 'margin must be greater than 0 ',";
                    }
                    if (item.specialDiscount < 0)
                    {
                        itemErrorCount++;
                        itemError += "specialDiscount : 'specialDiscount must be greater than 0 ',";
                    }

                    if (item.returnItems == null)
                    {
                        itemErrorCount++;
                        itemError += "returnitems: 'returnitems is required', ";
                    }
                    else
                    {
                        string returnItemErrors = "[";
                        foreach (var i in item.returnItems)
                        {
                            if(i.item._id == 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "itemId: itemId is required";
                            }
                            foreach (var a in items)
                            {
                                foreach (var t in a.returnItems)
                                {
                                    if (i.item._id == a.item.item._id)
                                    {
                                        returnitemsErrorCount++;
                                        returnItemErrors += "itemId : 'itemId already exists on another detail',";
                                    }
                                }
                                
                            }
                            if (i.quantity == 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "quantity : 'quantity is required',";
                            }
                            else if (i.quantity <= 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "quantity : 'quantity must be greater than 0',";
                            }
                            if (i.price == 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "price : 'price is required',";
                            }
                            else if (i.price < 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "price : 'price must be greater than 0',";
                            }
                            if (i.discount1 < 0 || i.discount1 > 100)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "discount1 : 'discount1 must be greater than 0 or less than 100',";
                            }

                            if (i.discount2 < 0 || i.discount2 > 100)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "discount2 : 'discount2 must be greater than 0 or less than 100',";
                            }

                            if (i.discountNominal < 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "discountNominal : 'discountNominal must be greater than 0 ',";
                            }
                            if (i.margin < 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "margin : 'margin must be greater than 0 ',";
                            }
                            if (i.specialDiscount < 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "specialDiscount : 'specialDiscount must be greater than 0 ',";
                            }

                            if(i.quantity > i.stock || i.stock == 0)
                            {
                                returnitemsErrorCount++;
                                returnItemErrors += "quantity : 'Stok Tidak Tersedia ',";
                            }

                            returnItemErrors += "}, ";
                        }
                        returnItemErrors += "]";

                        if (returnitemsErrorCount > 0)
                        {
                            itemErrorCount++;
                            itemError += $"returnItems: {returnItemErrors }, ";
                        }
                    }

                    itemError += "}, ";
                }

                itemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(itemError, new List<string> { "items" });
            }
        }
    }
}
