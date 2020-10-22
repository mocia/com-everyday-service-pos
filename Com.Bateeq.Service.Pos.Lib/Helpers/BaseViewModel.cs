using Com.Danliris.Service.Inventory.Lib.Interfaces;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Helpers
{
    public abstract class BaseViewModel<TModel> : IToModelable<TModel>
    {
        public int Id { get; set; }
        public bool _IsDeleted { get; set; }
        public bool Active { get; set; }
        public DateTime _CreatedUtc { get; set; }
        public string _CreatedBy { get; set; }
        public string _CreatedAgent { get; set; }
        public DateTime _LastModifiedUtc { get; set; }
        public string _LastModifiedBy { get; set; }
        public string _LastModifiedAgent { get; set; }

        public abstract TModel ToModel();
    }
}
