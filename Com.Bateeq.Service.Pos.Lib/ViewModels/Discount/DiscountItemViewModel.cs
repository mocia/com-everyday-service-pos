using Com.Danliris.Service.Inventory.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Pos.Lib.ViewModels.Discount
{
    public class DiscountItemViewModel : BasicViewModel
    {
        public string realizationOrder { get; set; }
        public List<DiscountDetailViewModel> details { get; set; }
    }
}
