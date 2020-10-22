using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.Discount
{
    public class DiscountViewModel : BasicViewModel, IValidatableObject
    {
        public string code { get; set; }
        public int discountOne { get; set; }
        public int discountTwo { get; set; }
        public DateTimeOffset endDate { get; set; }
        public string information { get; set; }
        public DateTimeOffset startDate { get; set; }
        public string storeCategory { get; set; }
        public bool isEdit { get; set; }
        public StoreViewModel store { get; set; }
        public List<DiscountItemViewModel> items { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (endDate.Equals(DateTimeOffset.MinValue) || endDate == null)
            {
                yield return new ValidationResult("EndDate is required", new List<string> { "EndDate" });
            }
            if (startDate.Equals(DateTimeOffset.MinValue) || startDate == null)
            {
                yield return new ValidationResult("StartDate is required", new List<string> { "StartDate" });
            }
            if (discountOne == 0 && discountTwo == 0)
            {
                yield return new ValidationResult("Discount must added", new List<string> { "Discount" });
            }
            if (store == null || store.name.Equals("- stores -"))
            {
                yield return new ValidationResult("store is required", new List<string> { "store" });
            }
            if (storeCategory == null || storeCategory == "" || storeCategory == "- categories -")
            {
                yield return new ValidationResult("StoreCategory is required", new List<string> { "StoreCategory" });
            }


            int itemErrorCount = 0;
            int detailErrorCount = 0;

            if (this.items == null || items.Count <= 0)
            {
                yield return new ValidationResult("item is required", new List<string> { "itemscount" });
            }
            else
            {
                string itemError = "[";

                foreach (var item in items)
                {
                    itemError += "{";

                    if (item.realizationOrder == null || item.realizationOrder == "")
                    {
                        itemErrorCount++;
                        itemError += "RO: 'No RO selected', ";
                    }

                    if (item.details == null || item.details.Count.Equals(0))
                    {
                        itemErrorCount++;
                        itemError += "details: 'No detail selected', ";
                    }
                    else
                    {
                        string detailError = "[";
                        foreach (var d in item.details)
                        {
                            PosDbContext posDbContext = (PosDbContext)validationContext.GetService(typeof(PosDbContext));
                            detailError += "{";
                            var db = from a in posDbContext.Discounts
                                     join b in posDbContext.DiscountItems on a.Id equals b.DiscountId
                                     join c in posDbContext.DiscountDetails on b.Id equals c.DiscountItemId
                                     where b.RealizationOrder == item.realizationOrder
                                     select new
                                     {
                                         a.StartDate,
                                         a.EndDate,
                                         a.DiscountOne,
                                         a.DiscountTwo,
                                         c.Code
                                     };
                            if (db.Where(x => x.Code == d.dataDestination.code).Count() > 0 && !isEdit)
                            {
                                if (discountOne == db.Where(x => x.Code == d.dataDestination.code).Select(x => x.DiscountOne).FirstOrDefault() &&
                                   discountTwo == db.Where(x => x.Code == d.dataDestination.code).Select(x => x.DiscountTwo).FirstOrDefault())
                                {
                                    if ((startDate.Date >= db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date
                                        && startDate.Date >= db.Where(x => x.Code == d.dataDestination.code).Select(x => x.EndDate).FirstOrDefault().Date.AddDays(1).AddTicks(-1))
                                        || (db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date >= startDate.Date &&
                                            db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date <= startDate.Date.AddDays(1).AddTicks(-1)))
                                    {
                                        detailErrorCount++;
                                        detailError += "item: 'item already use', ";
                                    }

                                }
                                else
                                {
                                    if ((startDate.Date >= db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date
                                        && startDate.Date >= db.Where(x => x.Code == d.dataDestination.code).Select(x => x.EndDate).FirstOrDefault().Date.AddDays(1).AddTicks(-1))
                                        || (db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date >= startDate.Date &&
                                            db.Where(x => x.Code == d.dataDestination.code).Select(x => x.StartDate).FirstOrDefault().Date <= startDate.Date.AddDays(1).AddTicks(-1)))
                                    {
                                        detailErrorCount++;
                                        detailError += "item: 'item already use', ";
                                    }
                                }
                            }


                            //join c in posDbContext.DiscountDetails
                            //if(posDbContext)
                            detailError += "}, ";
                        }

                        detailError += "]";

                        if (detailErrorCount > 0)
                        {
                            itemErrorCount++;
                            itemError += $"details: {detailError}, ";
                        }
                    }
                    
                    itemError += "}, ";
                }

                itemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(itemError, new List<string> { "items" });
            }
        }
        public class StoreViewModel
        {
            public int id { get; set; }
            public string code { get; set; }
            public string name { get; set; }
        }
    }
}
