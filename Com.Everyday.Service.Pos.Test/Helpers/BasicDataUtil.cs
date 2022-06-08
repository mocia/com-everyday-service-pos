using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Test.Helpers
{
    public abstract class BasicDataUtil<TDbContext, TService, TModel>
        where TDbContext : DbContext
        where TService : BasicService<TDbContext, TModel>
        where TModel : StandardEntity, IValidatableObject
    {
        public TDbContext DbContext { get; set; }
        public TService Service { get; set; }

        public BasicDataUtil(TDbContext dbContext, TService service)
        {
            DbContext = dbContext;
            Service = service;
            Service.Username = "Unit Test";
        }

        public abstract TModel GetNewData();
        public abstract Task<TModel> GetTestData();
    }
}
