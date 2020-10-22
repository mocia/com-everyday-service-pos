using Com.Bateeq.Service.Pos.Lib.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Text;
using static Com.Bateeq.Service.Pos.Lib.ViewModels.Discount.DiscountViewModel;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.Discount
{
    public class DiscountReadViewModel
    {
        public string code { get; set; }
        public int discountOne { get; set; }
        public int discountTwo { get; set; }
        public DateTimeOffset endDate { get; set; }
        public string information { get; set; }
        public DateTimeOffset startDate { get; set; }
        public string storeCategory { get; set; }
        public bool isEdit { get; set; }
        public List<StoreViewModel> store { get; set; }
        public List<DiscountItemViewModel> items { get; set; }
        
    }
}
