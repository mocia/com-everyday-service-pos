using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel
{
    public class TransferInDocViewModel
    {
        public string code { get; set; }

        public DateTimeOffset date { get; set; }

        public DestinationViewModel destination { get; set; }


        public string reference { get; set; }

        public string password { get; set; }

        public string remark { get; set; }

        public string uid { get; set; }

        public SourceViewModel source { get; set; }

        public List<TransferInDocItemViewModel> items { get; set; }
    }

    public class DestinationViewModel
    {
        public long _id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class SourceViewModel
    {
        public long _id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
