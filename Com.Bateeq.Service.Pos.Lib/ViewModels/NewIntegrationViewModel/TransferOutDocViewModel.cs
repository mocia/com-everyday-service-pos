using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel
{
    public class TransferOutDocViewModel
    {
        public string code { get; set; }

        public DateTimeOffset date { get; set; }

        public DestinationViewModel destination { get; set; }

        public string reference { get; set; }

        public ExpeditionServiceViewModel expeditionService { get; set; }

        public string remark { get; set; }

        public SourceViewModel source { get; set; }

        public List<TransferOutDocItemViewModel> items { get; set; }
    }

    public class ExpeditionServiceViewModel
    {
        public long _id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
