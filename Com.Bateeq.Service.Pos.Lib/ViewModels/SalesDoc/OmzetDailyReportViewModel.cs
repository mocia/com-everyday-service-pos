using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc
{
    public class OmzetDailyReportViewModel
    {
    }

    public class StoreViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string OnlineOffline { get; set; }
        public string SalesCategory { get; set; }
        public string StoreCategory { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }


    }


    public class CategoryViewModel
    {
        public string CategoryName { get; set; }
        public string OfflineOnline { get; set; }
        public double GrandTotal { get; set; }
        public double Count { get; set; }
    }

    public class CategoryCollectViewModel
    {
        public CategoryViewModel StandAlone { get; set; }
        public CategoryViewModel Konsinyasi { get; set; }
        public CategoryViewModel Online { get; set; }
        public CategoryViewModel WholeSale { get; set; }
        public CategoryViewModel vvip { get; set; }
    }


    public class DataViewModel
    {
        public List<TotalViewModel> StandAlone { get; set; }
        public List<TotalViewModel> Konsinyasi { get; set; }
        public List<TotalViewModel> Online { get; set; }
        public List<TotalViewModel> WholeSale { get; set; }
        public List<TotalViewModel> vvip { get; set; }
    }

    public class TotalViewModel
    {
        public double GrandTotal { get; set; }
        public double Count { get; set; }
        public StoreViewModel Store { get; set; }
    }

    public class TotalCategoryViewModel
    {
        public CategoryCollectViewModel CategoryList { get; set; }
        public DataViewModel DataList { get; set; }
    }
}
