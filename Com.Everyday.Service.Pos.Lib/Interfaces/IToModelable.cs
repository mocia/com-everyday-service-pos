using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Interfaces
{
    public interface IToModelable<TModel>
    {
        TModel ToModel();
    }
}
