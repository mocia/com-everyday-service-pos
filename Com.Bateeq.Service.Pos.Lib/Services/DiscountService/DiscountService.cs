using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Bateeq.Service.Pos.Lib.Models.Discount;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.Moonlay.Models;
using MongoDB.Bson;
using HashidsNet;
using Com.Bateeq.Service.Pos.Lib.ViewModels.Discount;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using static Com.Bateeq.Service.Pos.Lib.ViewModels.Discount.DiscountViewModel;
using Com.Bateeq.Service.Pos.Lib.Interfaces;
using Newtonsoft.Json;
using Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel;
using System.Linq;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Pos.Lib.ViewModels.Discount;

namespace Com.Bateeq.Service.Pos.Lib.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private const string UserAgent = "discount-service";
        protected DbSet<Discount> DbSet;
        protected DbSet<DiscountStore> DbSetDiscountStore;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public PosDbContext DbContext;
        public DiscountService(IServiceProvider serviceProvider, PosDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<Discount>();
            DbSetDiscountStore = dbContext.Set<DiscountStore>();
            IdentityService = serviceProvider.GetService<IIdentityService>();

        }

        public Tuple<List<Discount>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "")
        {
            IQueryable<Discount> Query = this.DbContext.Discounts.Where(x=>x._CreatedBy == Username);

            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<Discount>.Search(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Code", "DiscountOne", "DiscountTwo", "StartDate", "EndDate","Information"
            };

            Query = Query
                .Select(mdn => new Discount
                {
                    Id = mdn.Id,
                    Code = mdn.Code,
                    DiscountOne = mdn.DiscountOne,
                    DiscountTwo = mdn.DiscountTwo,
                    StartDate = mdn.StartDate,
                    EndDate = mdn.EndDate,
                    Information = mdn.Information
                });

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = QueryHelper<Discount>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<Discount>.Order(Query, OrderDictionary);

            Pageable<Discount> pageable = new Pageable<Discount>(Query, Page - 1, Size);
            List<Discount> Data = pageable.Data.ToList<Discount>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<int> Create(Discount model)
        {
            int Created = 0;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try { 
                    string code = GenerateCode("EFR-DC");
                    model.Code = code;
                    model.FlagForCreate(IdentityService.Username, UserAgent);
                    model.FlagForUpdate(IdentityService.Username, UserAgent);
                    foreach(var i in model.Items)
                    {
                        i.FlagForCreate(IdentityService.Username, UserAgent);
                        i.FlagForUpdate(IdentityService.Username, UserAgent);
                        foreach(var d in i.Details)
                        {
                            d.FlagForCreate(IdentityService.Username, UserAgent);
                            d.FlagForUpdate(IdentityService.Username, UserAgent);
                        }
                    }


                    DbSet.Add(model);
                    Created = await DbContext.SaveChangesAsync();
                    Created += await AddStores(model, model.Id);
                    transaction.Commit();
                }catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }


            }

            return Created;
        }
        public HashSet<int> GetDiscountId(long id)
        {
            return new HashSet<int>(DbContext.DiscountItems.Where(d => d.Discount.Id == id).Select(d => d.Id));
        }
        public int Delete(int id)
        {
            int deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try {
                    var discount = DbSet.Where(m => m.Id == id)
                    .Include(m => m.Items)
                    .ThenInclude(i => i.Details)
                    .FirstOrDefault();
                    discount.FlagForDelete(IdentityService.Username, UserAgent);
                    foreach (var i in discount.Items)
                    {
                        i.FlagForDelete(IdentityService.Username, UserAgent);
                        foreach (var d in i.Details)
                        {
                            d.FlagForDelete(IdentityService.Username, UserAgent);
                        }
                    }

                    var stores = DbContext.DiscountStores.Where(x => x.DiscountId == id);
                    foreach (var s in stores)
                    {
                        s.FlagForDelete(IdentityService.Username, UserAgent);
                    }
                    deleted = DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
                
            
            return deleted;
        }
        public async Task<int> Update(int id, Discount model, string user, int clientTimeZoneOffset = 7)
        {
            int Updated = 0;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var oldM = DbSet.AsNoTracking().Where(m => m.Id == id)
                    .Include(m => m.Items)
                    .ThenInclude(i => i.Details)
                    .FirstOrDefault();

                    if(oldM != null && oldM.Id == id)
                    {
                        model.FlagForUpdate(IdentityService.Username, UserAgent);

                        foreach (var item in model.Items)
                        {
                            if(item.Id == 0)
                            {
                                item.FlagForCreate(IdentityService.Username, UserAgent);
                                item.FlagForUpdate(IdentityService.Username, UserAgent);
                            }
                            else
                            {
                                item.FlagForUpdate(IdentityService.Username, UserAgent);
                            }

                            foreach(var detail in item.Details){

                                if(detail.Id == 0)
                                {
                                    detail.FlagForCreate(IdentityService.Username, UserAgent);
                                    detail.FlagForUpdate(IdentityService.Username, UserAgent);
                                }
                                else
                                {
                                    detail.FlagForUpdate(IdentityService.Username, UserAgent);
                                }
                            }
                        }
                    }

                    this.DbSet.Update(model);

                    foreach(var oldItem in oldM.Items)
                    {
                        var newItem = model.Items.FirstOrDefault(x => x.Id == oldItem.Id && x.RealizationOrder == oldItem.RealizationOrder);
                        foreach(var oldDetail in oldItem.Details)
                        {
                            //var newdetail = model.Items.Any(i=>i.Details.Where(x=>x.Code == oldDetail.Code))
                            if(newItem == null)
                            {
                                oldItem.FlagForDelete(IdentityService.Username, UserAgent);
                                DbContext.DiscountItems.Update(oldItem);
                                oldDetail.FlagForDelete(IdentityService.Username, UserAgent);
                                DbContext.DiscountDetails.Update(oldDetail);
                            }
                            else
                            {
                                var newDetail = newItem.Details.FirstOrDefault(x => x.Code == oldDetail.Code);
                                if(newDetail == null)
                                {
                                    oldDetail.FlagForDelete(IdentityService.Username, UserAgent);
                                    DbContext.DiscountDetails.Update(oldDetail);
                                }
                            }
                            
                        }
                    }
                    //if (oldM != null && oldM.Id == id) {
                    //    oldM.FlagForUpdate(IdentityService.Username, UserAgent);
                    //    oldM.Information = model.Information;
                    //    oldM.StartDate = model.StartDate;
                    //    oldM.StoreCategory = model.StoreCategory;
                    //    oldM.StoreName = model.StoreName;
                    //    oldM.DiscountOne = model.DiscountOne;
                    //    oldM.DiscountTwo = model.DiscountTwo;
                    //    oldM.EndDate = model.EndDate;

                    //    foreach (var oldItem in oldM.Items)
                    //    {
                    //        var newItem = model.Items.FirstOrDefault(i => i.Id.Equals(oldItem.Id));
                    //        if (newItem == null)
                    //        {
                    //            oldItem.FlagForDelete(IdentityService.Username, UserAgent);
                    //        }
                    //        else
                    //        {
                    //            oldItem.FlagForUpdate(IdentityService.Username, UserAgent);
                    //            oldItem.RealizationOrder = newItem.RealizationOrder;
                    //        }
                    //        foreach (var item in model.Items)
                    //        {
                    //            foreach (var olddetail in oldItem.Details)
                    //            {
                    //                var newdetail = item.Details.FirstOrDefault(d => d.Id.Equals(olddetail.Id));
                    //                if (newdetail == null)
                    //                {
                    //                    olddetail.FlagForDelete(IdentityService.Username, UserAgent);
                    //                }
                    //                else
                    //                {
                    //                    olddetail.FlagForUpdate(IdentityService.Username, UserAgent);
                    //                    olddetail.ArticleRealizationOrder = newdetail.ArticleRealizationOrder;
                    //                    olddetail.Code = newdetail.Code;
                    //                    olddetail.DomesticCOGS = newdetail.DomesticCOGS;
                    //                    olddetail.DomesticRetail = newdetail.DomesticRetail;
                    //                    olddetail.DomesticSale = newdetail.DomesticSale;
                    //                    olddetail.DomesticWholesale = newdetail.DomesticWholesale;
                    //                    olddetail.InternationalCOGS = newdetail.InternationalCOGS;
                    //                    olddetail.InternationalRetail = newdetail.InternationalRetail;
                    //                    olddetail.InternationalSale = newdetail.InternationalSale;
                    //                    olddetail.InternationalWholesale = newdetail.InternationalWholesale;
                    //                    olddetail.ItemId = newdetail.ItemId;
                    //                    olddetail.Name = newdetail.Name;
                    //                    olddetail.Size = newdetail.Size;
                    //                    olddetail.Uom = newdetail.Uom;

                    //                }
                    //            }
                    //        }
                    //    }

                    //        foreach (var item in model.Items.Where(i => i.Id == 0))
                    //        {
                    //            item.FlagForCreate(IdentityService.Username,UserAgent);
                    //            item.FlagForUpdate(IdentityService.Username,UserAgent);
                    //            //item.Status = "Belum diterima Pembelian";

                    //            oldM.Items.Add(item);

                    //            foreach (var olditem in oldM.Items)
                    //            {
                    //                foreach(var detail in item.Details.Where(i => i.Id == 0))
                    //                {
                    //                    detail.FlagForCreate(IdentityService.Username, UserAgent);
                    //                    detail.FlagForUpdate(IdentityService.Username, UserAgent);
                    //                    olditem.Details.Add(detail);
                    //                }
                    //            }
                    //        }

                    //        foreach (var item in model.Items) {
                    //            foreach (var olditem in oldM.Items)
                    //            {
                    //                foreach(var detail in item.Details.Where(i => i.Id == 0))
                    //                {
                    //                    detail.FlagForCreate(IdentityService.Username, UserAgent);
                    //                    detail.FlagForUpdate(IdentityService.Username, UserAgent);

                    //                    olditem.Details.Add(detail);
                    //                }
                    //            }
                    //        }
                    //    }
                    
                    Updated = await DbContext.SaveChangesAsync();

                    Updated += await UpdateStores(model.Id, model);
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return Updated;
        }
        public async Task<int> UpdateStores(int id, Discount model)
        {
            int Update = 0;
            List<StoreViewModels> storeviews = new List<StoreViewModels>();
            if (model.StoreCategory == "ALL" && model.StoreName == "ALL")
            {
                var storeview = GetStore("", "");
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }
            else if (model.StoreCategory != "ALL" && model.StoreName == "ALL")
            {
                var storeview = GetStore(model.StoreCategory, "");
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }
            else
            {
                var storeview = GetStore(model.StoreCategory, model.StoreName);
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }

            if(model.StoreCategory == "ALL")
            {
                  foreach (var store in storeviews)
                  {
                        var distore = DbContext.DiscountStores.Where(x => x.DiscountId == id && x.Name == store.name).FirstOrDefault();
                        if (distore == null)
                        {
                            DiscountStore discountStore = new DiscountStore
                            {
                                Address = store.address,
                                City = store.city,
                                Code = store.code,
                                Name = store.name,
                                OnlineOffline = store.onlineOffline,
                                SalesCategory = store.salesCategory,
                                StoreArea = store.storeArea,
                                StoreCategory = store.storeCategory,
                                StoreId = store.Id,
                                StoreWide = store.storeWide,
                                DiscountId = model.Id,
                                DiscountCode = model.Code
                            };
                            discountStore.FlagForCreate(IdentityService.Username, UserAgent);
                            discountStore.FlagForUpdate(IdentityService.Username, UserAgent);

                            DbSetDiscountStore.Add(discountStore);

                        }
                        else
                        {
                            distore._IsDeleted = false;
                        }
                  }
            } else if (DbContext.DiscountStores.Where(x => x.DiscountId == model.Id).Count() > 1)
                    {
                        foreach (var stores in DbContext.DiscountStores.Where(x => x.DiscountId == model.Id))
                        {
                            if (model.StoreName != stores.Name)
                            {
                                stores.FlagForDelete(IdentityService.Username, UserAgent);
                            }
                            else
                            {
                                var store = storeviews.Where(x => x.name == stores.Name).FirstOrDefault();
                                stores.Address = store.address;
                                stores.City = store.city;
                                stores.Code = store.code;
                                stores.Name = store.name;
                                stores.OnlineOffline = store.onlineOffline;
                                stores.SalesCategory = store.salesCategory;
                                stores.StoreArea = store.storeArea;
                                stores.StoreCategory = store.storeCategory;
                                stores.StoreId = store.Id;
                                stores.StoreWide = store.storeWide;
                                stores._IsDeleted = false;
                            }
                        }
                    }
            else
            {
                var stores = DbContext.DiscountStores.Where(x => x.DiscountId == model.Id ).FirstOrDefault();
                var store = storeviews.Where(x => x.name == model.StoreName ).FirstOrDefault();
                stores.Address = store.address;
                stores.City = store.city;
                stores.Code = store.code;
                stores.Name = store.name;
                stores.OnlineOffline = store.onlineOffline;
                stores.SalesCategory = store.salesCategory;
                stores.StoreArea = store.storeArea;
                stores.StoreCategory = store.storeCategory;
                stores.StoreId = store.Id;
                stores.StoreWide = store.storeWide;
            }

            Update += await DbContext.SaveChangesAsync();
            return Update;
        }
        public async Task<int> AddStores(Discount model, int Id)
        {
            List<StoreViewModels> storeviews = new List<StoreViewModels>();
            if (model.StoreCategory == "ALL" && model.StoreName == "ALL")
            {
                var storeview = GetStore("", "");
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }
            else if (model.StoreCategory != "ALL" && model.StoreName == "ALL")
            {
                var storeview = GetStore(model.StoreCategory, "");
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }
            else
            {
                var storeview = GetStore(model.StoreCategory, model.StoreName);
                foreach (var i in storeview)
                {
                    storeviews.Add(i);
                }
            }

            foreach (var store in storeviews)
            {
                DiscountStore discountStore = new DiscountStore
                {
                    Address = store.address,
                    City = store.city,
                    Code = store.code,
                    Name = store.name,
                    OnlineOffline = store.onlineOffline,
                    SalesCategory = store.salesCategory,
                    StoreArea = store.storeArea,
                    StoreCategory = store.storeCategory,
                    StoreId = store.Id,
                    StoreWide = store.storeWide,
                    DiscountId = Id,
                    DiscountCode = model.Code
                };
                discountStore.FlagForCreate(IdentityService.Username, UserAgent);
                discountStore.FlagForUpdate(IdentityService.Username, UserAgent);

                DbSetDiscountStore.Add(discountStore);
            }
            int result = 0;
            return result += await DbContext.SaveChangesAsync();
        }
        public Discount ReadModelById(int id)
        {
            var a = DbSet.Where(m => m.Id == id)
                    .Include(m => m.Items)
                    .ThenInclude(i => i.Details)
                    .FirstOrDefault();
            return a;
        }
        public string GenerateCode(string ModuleId)
        {
            var uid = ObjectId.GenerateNewId().ToString();
            var hashids = new Hashids(uid, 8, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
            var now = DateTime.Now;
            var begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var diff = (now - begin).Milliseconds;
            string code = String.Format("{0}/{1}/{2}", hashids.Encode(diff), ModuleId, DateTime.Now.ToString("MM/yyyy"));
            return code;
        }

        public Discount MapToModel(DiscountViewModel viewModel)
        {
            Discount model = new Discount();
            PropertyCopier<DiscountViewModel,Discount>.Copy(viewModel, model);
            model.DiscountOne = viewModel.discountOne;
            model.DiscountTwo = viewModel.discountTwo;
            model.EndDate = viewModel.endDate;
            model.StartDate = viewModel.startDate;
            model.StoreCategory = viewModel.storeCategory;
            model.Information = viewModel.storeCategory;
            model.StoreName = viewModel.store.name;
            model.Items = new List<DiscountItem>();
            //List<DiscountItem> discountItems = new List<DiscountItem>();
            //List<DiscountDetail> discountDetails = new List<DiscountDetail>();

            foreach (DiscountItemViewModel i in viewModel.items)
            {
                DiscountItem discountItem = new DiscountItem();
                PropertyCopier<DiscountItemViewModel, DiscountItem>.Copy(i, discountItem);
                discountItem.RealizationOrder = i.realizationOrder;

                discountItem.Details = new List<DiscountDetail>();
                foreach (DiscountDetailViewModel d in i.details)
                {
                    DiscountDetail discountDetail = new DiscountDetail();
                    PropertyCopier<DiscountDetailViewModel, DiscountDetail>.Copy(d, discountDetail);
                    discountDetail.ArticleRealizationOrder = d.dataDestination.ArticleRealizationOrder;
                    discountDetail.Code = d.dataDestination.code;
                    discountDetail.DomesticCOGS = d.DomesticCOGS;
                    discountDetail.DomesticRetail = d.DomesticRetail;
                    discountDetail.DomesticSale = d.DomesticSale;
                    discountDetail.DomesticWholesale = d.DomesticWholesale;
                    discountDetail.InternationalCOGS = d.InternationalCOGS;
                    discountDetail.InternationalRetail = d.InternationalRetail;
                    discountDetail.InternationalSale = d.InternationalSale;
                    discountDetail.InternationalWholesale = d.DomesticWholesale;
                    discountDetail.ItemId = d.dataDestination._id;
                    discountDetail.Name = d.dataDestination.name;
                    discountDetail.Size = d.dataDestination.Size;
                    discountDetail.Uom = d.dataDestination.Uom;
                    discountItem.Details.Add(discountDetail);
                }
                model.Items.Add(discountItem);
            }
            return model;
        }

        public DiscountViewModel MapToViewModel(Discount model)
        {
            DiscountViewModel viewModel = new DiscountViewModel();
            PropertyCopier<Discount, DiscountViewModel>.Copy(model, viewModel);

            viewModel.code = model.Code;
            viewModel.discountOne = model.DiscountOne;
            viewModel.discountTwo = model.DiscountTwo;
            viewModel.endDate = model.EndDate;
            viewModel.startDate = model.StartDate;
            viewModel.information = model.Information;
            viewModel.storeCategory = model.StoreCategory;
            viewModel.store = new StoreViewModel
            {
                name = model.StoreName
            };
            viewModel.Active = model.Active;
            viewModel.Id = model.Id;
            viewModel._CreatedAgent = model._CreatedAgent;
            viewModel._CreatedBy = model._CreatedBy;
            viewModel._IsDeleted = model._IsDeleted;
            viewModel._LastModifiedAgent = model._LastModifiedAgent;
            viewModel._LastModifiedBy = model._LastModifiedBy;
            viewModel._LastModifiedUtc = model._LastModifiedUtc;


            viewModel.items = new List<DiscountItemViewModel>();
            if (model.Items != null)
            {
                foreach (DiscountItem mdni in model.Items)
                {
                    DiscountItemViewModel discountItemView = new DiscountItemViewModel();
                    PropertyCopier<DiscountItem, DiscountItemViewModel>.Copy(mdni, discountItemView);

                    discountItemView.details = new List<DiscountDetailViewModel>();
                    foreach (DiscountDetail mdnd in mdni.Details)
                    {
                        DiscountDetailViewModel discountDetailViewModel = new DiscountDetailViewModel();
                        PropertyCopier<DiscountDetail, DiscountDetailViewModel>.Copy(mdnd, discountDetailViewModel);

                        discountDetailViewModel.dataDestination = new ItemViewModelRead {
                            ArticleRealizationOrder = mdnd.ArticleRealizationOrder,
                            code = mdnd.Code,
                            name = mdnd.Name,
                            Size = mdnd.Size,
                            Uom = mdnd.Uom,
                        };
                        //ItemViewModelRead itemViewModelRead = new ItemViewModelRead
                        //{
                        //    ArticleRealizationOrder = mdnd.ArticleRealizationOrder,
                        //    code = mdnd.Code,
                        //    name = mdnd.Name,
                        //    Size = mdnd.Size,
                        //    Uom = mdnd.Uom,
                        //};
                        //discountDetailViewModel.dataDestination.Add(itemViewModelRead);

                        discountDetailViewModel.DomesticCOGS = mdnd.DomesticCOGS;
                        discountDetailViewModel.DomesticRetail = mdnd.DomesticRetail;
                        discountDetailViewModel.DomesticSale = mdnd.DomesticSale;
                        discountDetailViewModel.DomesticWholesale = mdnd.DomesticWholesale;
                        discountDetailViewModel.InternationalCOGS = mdnd.InternationalCOGS;
                        discountDetailViewModel.InternationalRetail = mdnd.InternationalRetail;
                        discountDetailViewModel.InternationalSale = mdnd.InternationalSale;
                        discountDetailViewModel.InternationalWholesale = mdnd.InternationalWholesale;
                        discountDetailViewModel.Id = mdnd.Id;
                        discountDetailViewModel._CreatedAgent = mdnd._CreatedAgent;
                        discountDetailViewModel._CreatedBy = mdnd._CreatedBy;
                        discountDetailViewModel._IsDeleted = mdnd._IsDeleted;
                        discountDetailViewModel._LastModifiedAgent = mdnd._LastModifiedAgent;
                        discountDetailViewModel._LastModifiedBy = mdnd._LastModifiedBy;
                        discountDetailViewModel._LastModifiedUtc = mdnd._LastModifiedUtc;
                        discountItemView.details.Add(discountDetailViewModel);
                    }
                    discountItemView.realizationOrder = mdni.RealizationOrder;
                    discountItemView.Active = mdni.Active;
                    discountItemView.Id = mdni.Id;
                    discountItemView._CreatedAgent = mdni._CreatedAgent;
                    discountItemView._IsDeleted = mdni._IsDeleted;
                    discountItemView._LastModifiedAgent = mdni._LastModifiedAgent;
                    discountItemView._LastModifiedBy = mdni._LastModifiedBy;
                    discountItemView._LastModifiedUtc = mdni._LastModifiedUtc;
                    viewModel.items.Add(discountItemView);
                }
            }

            return viewModel;
        }

        public List<StoreViewModels> GetStore(string category, string storeCode)
        {
            string storeUri = "master/stores/category";
            IHttpClientService httpClient = (IHttpClientService)ServiceProvider.GetService(typeof(IHttpClientService));
            if (httpClient != null)
            {
                var response = httpClient.GetAsync($"{APIEndpoint.Core}{storeUri}?category={category}").Result.Content.ReadAsStringAsync();
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
                List<StoreViewModels> viewModel = JsonConvert.DeserializeObject<List<StoreViewModels>>(result.GetValueOrDefault("data").ToString());
                viewModel = viewModel.Where(x => x.name == (string.IsNullOrWhiteSpace(storeCode) ? x.name : storeCode)).ToList();
                return viewModel;
            }
            else
            {
                List<StoreViewModels> viewModel = null;
                return viewModel;
            }

        }
        public List<DiscountReadViewModel> GetDiscounts (string code, DateTime date)
        {
            var a = DbSet.Where(x=>x.Items.Any(i=>i.Details.Any(d=>d.Code == code)))
                    .Include(m => m.Items)
                    .ThenInclude(i => i.Details)
                    .ToList();
            a = a.Where(x => x.StartDate < date && x.EndDate > date).ToList();
            List<DiscountReadViewModel> discountReadViews = new List<DiscountReadViewModel>();
            foreach (var model in a)
            {
                List<StoreViewModel> stores = new List<StoreViewModel>();

                List<DiscountItemViewModel> itemViewModels = new List<DiscountItemViewModel>();
                List<DiscountDetailViewModel> detailViewModels = new List<DiscountDetailViewModel>();
                var store = DbContext.DiscountStores.Where(x => x.DiscountId == model.Id).ToList();
                foreach(var s in store)
                {
                    stores.Add(new StoreViewModel
                    {
                        code = s.Code,
                        id = s.Id,
                        name = s.Name
                    });
                }

                foreach(var item in model.Items)
                {
                    
                    foreach (var mdnd in item.Details)
                    {
                        detailViewModels.Add(new DiscountDetailViewModel
                        {
                            dataDestination = new ItemViewModelRead
                            {
                                ArticleRealizationOrder = mdnd.ArticleRealizationOrder,
                                code = mdnd.Code,
                                name = mdnd.Name,
                                Size = mdnd.Size,
                                Uom = mdnd.Uom,
                            },
                            DomesticCOGS = mdnd.DomesticCOGS,
                            DomesticRetail = mdnd.DomesticRetail,
                            DomesticSale = mdnd.DomesticSale,
                            DomesticWholesale = mdnd.DomesticWholesale,
                            InternationalCOGS = mdnd.InternationalCOGS,
                            InternationalRetail = mdnd.InternationalRetail,
                            InternationalSale = mdnd.InternationalSale,
                            InternationalWholesale = mdnd.InternationalWholesale,
                            Id = mdnd.Id,
                            _CreatedAgent = mdnd._CreatedAgent,
                            _CreatedBy = mdnd._CreatedBy,
                            _IsDeleted = mdnd._IsDeleted,
                            _LastModifiedAgent = mdnd._LastModifiedAgent,
                            _LastModifiedBy = mdnd._LastModifiedBy,
                            _LastModifiedUtc = mdnd._LastModifiedUtc,

                        });
                    }
                    itemViewModels.Add(new DiscountItemViewModel
                    {
                        realizationOrder = item.RealizationOrder,
                        details = detailViewModels
                    });
                    
                }
                discountReadViews.Add(new DiscountReadViewModel
                {
                    code = model.Code,
                    discountOne = model.DiscountOne,
                    discountTwo = model.DiscountTwo,
                    endDate = model.EndDate,
                    startDate = model.StartDate,
                    information = model.Information,
                    storeCategory = model.StoreCategory,
                    store = stores,

                    items = itemViewModels

                });
            }
            return discountReadViews;
            //var ab = (from a in DbContext.Discounts
            //          join b in DbContext.DiscountItems on a.Id equals b.DiscountId
            //          join c in DbContext.DiscountDetails on b.Id equals c.DiscountItemId
            //          where c.Code == code
            //          //&& a.StartDate <= date
            //          //&& a.EndDate >= date
            //          select new { a, b, c });
            //foreach(var i in ab)
            //{
            //    var store = DbContext.DiscountStores.Where(x => x.DiscountId == i.Id).ToList();
            //    foreach(var a in store)
            //    {
            //        i.store.Add(new StoreViewModel
            //        {
            //            code = a.Code,
            //            id = a.Id,
            //            name = a.Name
            //        });
            //    }
            //}
            //return ab;
        }
    }
}
