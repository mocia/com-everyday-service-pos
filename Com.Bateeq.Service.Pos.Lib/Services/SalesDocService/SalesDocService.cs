using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Services;
using Com.Bateeq.Service.Pos.Lib.Services.SalesDocService;
using Com.Bateeq.Service.Pos.Lib.ViewModels.NewIntegrationViewModel;
using Com.Bateeq.Service.Pos.Lib.Interfaces;
using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;
using Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Pos.Lib.Services.SalesDocService
{
    public class SalesDocService : ISalesDocService
    {
        private const string UserAgent = "sales-service";
        protected DbSet<SalesDoc> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public PosDbContext DbContext;
        public SalesDocService(IServiceProvider serviceProvider, PosDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<SalesDoc>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }
        public Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModel(string storecode,int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "")
        {
            IQueryable<SalesDoc> Query = this.DbContext.SalesDocs.Where(x => x._CreatedBy == Username && x.StoreCode == storecode);

            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<SalesDoc>.Search(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Date", "Code", "StoreName"
            };

           

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = QueryHelper<SalesDoc>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<SalesDoc>.Order(Query, OrderDictionary);

            Pageable<SalesDoc> pageable = new Pageable<SalesDoc>(Query, Page - 1, Size);
            List<SalesDoc> Data = pageable.Data.ToList<SalesDoc>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        public Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModelVoid(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "")
        {
            //IQueryable<SalesDoc> Query = this.DbContext.SalesDocs.Where(x => x._CreatedBy == Username && x.isVoid == false && x.StoreCode == storecode);
            IQueryable<SalesDoc> Query = DbSet.Where(x => x._CreatedBy == Username && x.isVoid == false && x.StoreCode == storecode)
                .Include(x => x.Details);
            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<SalesDoc>.Search(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Date", "Code", "StoreName", "GrandTotal", "_CreatedBy"
            };

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = QueryHelper<SalesDoc>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<SalesDoc>.Order(Query, OrderDictionary);

            Pageable<SalesDoc> pageable = new Pageable<SalesDoc>(Query, Page - 1, Size);
            List<SalesDoc> Data = pageable.Data.ToList<SalesDoc>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        public Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModelReturn(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "")
        {
            //IQueryable<SalesDoc> Query = this.DbContext.SalesDocs.Where(x => x._CreatedBy == Username && x.isVoid == false && x.StoreCode == storecode);
            IQueryable<SalesDoc> Query = DbSet.Where(x => x._CreatedBy == Username && x.isReturn == false && x.StoreCode == storecode)
                .Include(x => x.Details);
            List<string> SearchAttributes = new List<string>()
            {
                "Code"
            };

            Query = QueryHelper<SalesDoc>.Search(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "Date", "Code", "StoreName", "GrandTotal", "_CreatedBy"
            };

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = QueryHelper<SalesDoc>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<SalesDoc>.Order(Query, OrderDictionary);

            Pageable<SalesDoc> pageable = new Pageable<SalesDoc>(Query, Page - 1, Size);
            List<SalesDoc> Data = pageable.Data.ToList<SalesDoc>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        public SalesDoc ReadModelById(int id)
        {
            var a = DbSet.Where(m => m.Id == id)
                    .Include(m => m.Details)
                    .FirstOrDefault();
            return a;
        }

        public SalesDoc ReadModelByCode(string code, string storecode)
        {
            var a = DbSet.Where(m => m.Code == code && m.StoreStorageCode == storecode)
                    .Include(m => m.Details)
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
        public SalesDoc MapToModel(SalesDocViewModel viewModel)
        {
            SalesDoc model = new SalesDoc();
            PropertyCopier<SalesDocViewModel, SalesDoc>.Copy(viewModel, model);
            if(viewModel.salesDetail.bankCard != null)
            {
                model.BankCardCode = viewModel.salesDetail.bankCard.code;
                model.BankCardId = viewModel.salesDetail.bankCard._id;
                model.BankCardName = viewModel.salesDetail.bankCard.name;
            }
            if(viewModel.salesDetail.bank != null)
            {
                model.BankCode = viewModel.salesDetail.bank.code;
                model.BankId = viewModel.salesDetail.bank._id;
                model.BankName = viewModel.salesDetail.bank.name;
            }
            
            model.CardAmount = viewModel.salesDetail.cardAmount;
            model.CardName = viewModel.salesDetail.cardName;
            model.CardNumber = viewModel.salesDetail.cardNumber;
            if(viewModel.salesDetail.cardType != null)
            {
                model.CardTypeCode = viewModel.salesDetail.cardType.code;
                model.CardTypeId = viewModel.salesDetail.cardType._id;
                model.CardTypeName = viewModel.salesDetail.cardType.name;
            }
            model.CashAmount = viewModel.salesDetail.cashAmount;
            model.Card = viewModel.salesDetail.card;
            model.Date = viewModel.date;
            model.Discount = viewModel.discount;
            model.GrandTotal = viewModel.grandTotal;
            model.isReturn = false;
            model.isVoid = false;
            model.PaymentType = viewModel.salesDetail.paymentType;
            model.Reference = viewModel.reference;
            model.Remark = viewModel.remark;
            model.Shift = viewModel.shift;
            model.StoreCode = viewModel.store.Code;
            model.StoreId = viewModel.store.Id;
            model.StoreName = viewModel.store.Name;
            model.StoreCategory = viewModel.store.StoreCategory;
            model.StoreStorageCode = viewModel.store.Storage.code;
            model.StoreStorageId = viewModel.store.Storage._id;
            model.StoreStorageName = viewModel.store.Storage.name;
            model.SubTotal = viewModel.subTotal;
            model.TotalProduct = viewModel.totalProduct;
            model.VoucherValue = viewModel.salesDetail.voucher.value;

            model.Details = new List<SalesDocDetail>();

            foreach (SalesDocDetailViewModel i in viewModel.items)
            {
                SalesDocDetail salesDocDetail = new SalesDocDetail();
                PropertyCopier<SalesDocDetailViewModel, SalesDocDetail>.Copy(i, salesDocDetail);
                salesDocDetail.Discount1 = i.discount1;
                salesDocDetail.Discount2 = i.discount2;
                salesDocDetail.DiscountNominal = i.discountNominal;
                salesDocDetail.isReturn = false;
                salesDocDetail.ItemArticleRealizationOrder = i.item.ArticleRealizationOrder;
                salesDocDetail.ItemCode = i.item.code;
                salesDocDetail.ItemDomesticCOGS = i.item.DomesticCOGS;
                salesDocDetail.ItemDomesticRetail = i.item.DomesticRetail;
                salesDocDetail.ItemDomesticSale = i.item.DomesticSale;
                salesDocDetail.ItemDomesticWholeSale = i.item.DomesticWholeSale;
                salesDocDetail.ItemId = i.item._id;
                salesDocDetail.ItemName = i.item.name;
                salesDocDetail.ItemSize = i.item.Size;
                salesDocDetail.ItemUom = i.item.Uom;
                salesDocDetail.Margin = i.margin;
                salesDocDetail.Price = i.price;
                salesDocDetail.Quantity = i.quantity;
                salesDocDetail.Size = i.item.Size;
                salesDocDetail.SpesialDiscount = i.specialDiscount;
                salesDocDetail.Total = i.total;

                
                model.Details.Add(salesDocDetail);
            }
            return model;
        }

        public SalesDocViewModel MaptoViewModel(SalesDoc model)
        {
            SalesDocViewModel viewModel = new SalesDocViewModel();
            PropertyCopier<SalesDoc, SalesDocViewModel>.Copy(model, viewModel);
            var salesdoc = DbContext.Set<SalesDoc>().Where(x => x.Id == model.Id).FirstOrDefault();
            viewModel.Active = model.Active;
            viewModel.date = model.Date;
            viewModel.discount = model.Discount;
            viewModel.grandTotal = model.GrandTotal;
            viewModel.Id = model.Id;
            viewModel.reference = model.Reference;
            viewModel.remark = model.Remark;
            viewModel.code = model.Code;
            viewModel.isReturn = model.isReturn;
            viewModel.isVoid = model.isVoid;
            viewModel.salesDetail = new SalesDetail
            {
                bank = new Bank
                {
                    code = model.BankCode,
                    description = "",
                    name = model.BankName,
                    _id = model.BankId
                },
                bankCard = new Bank
                {
                    code = model.BankCardCode,
                    description = "",
                    name = model.BankCardName,
                    _id = model.BankCardId
                },
                cardAmount = model.CardAmount,
                cardName = model.CardName,
                cardNumber = model.CardNumber,
                cardType = new Card
                {
                    code = model.CardTypeCode,
                    description = "",
                    name = model.CardTypeName,
                    _id = model.CardTypeId
                },
                cashAmount = model.CashAmount,
                paymentType = model.PaymentType,
                voucher = new Voucher
                {
                    value = model.VoucherValue
                },
                card = model.Card
            };
            var Store = GetStore(salesdoc.StoreCode);
            viewModel.store = new Store
            {
                Code = model.StoreCode,
                Name = model.StoreName,
                Id = model.StoreId,
                Address = Store.address,
                Phone = Store.phone,
                StoreCategory = model.StoreCategory,

                Storage = new StorageViewModel
                {
                    code = model.StoreStorageCode,
                    name = model.StoreStorageName,
                    _id = model.StoreStorageId,

                }
            };
            viewModel.shift = model.Shift;
            viewModel.subTotal = model.SubTotal;
            viewModel.totalProduct = model.TotalProduct;
            viewModel.grandTotal = model.GrandTotal;
            viewModel.totalDiscount = model.Discount;
            
            viewModel.Id = model.Id;
            viewModel.items = new List<SalesDocDetailViewModel>();
            foreach (SalesDocDetail i in model.Details)
            {
                SalesDocDetailViewModel salesDocDetailViewModel = new SalesDocDetailViewModel();
                PropertyCopier<SalesDocDetail, SalesDocDetailViewModel>.Copy(i, salesDocDetailViewModel);
                salesDocDetailViewModel.Id = i.Id;
                salesDocDetailViewModel.item = new ViewModels.NewIntegrationViewModel.ItemViewModel
                {
                    ArticleRealizationOrder = i.ItemArticleRealizationOrder,
                    code = i.ItemCode,
                    DomesticCOGS = i.ItemDomesticCOGS,
                    DomesticRetail = i.ItemDomesticRetail,
                    DomesticSale = i.ItemDomesticSale,
                    DomesticWholeSale = i.ItemDomesticWholeSale,
                    name = i.ItemName,
                    Size = i.ItemSize,
                    Uom = i.ItemUom,
                    _id = i.ItemId
                };
                salesDocDetailViewModel.discount1 = i.Discount1;
                salesDocDetailViewModel.discount2 = i.Discount2;
                salesDocDetailViewModel.discountNominal = i.DiscountNominal;
                salesDocDetailViewModel.itemCode = i.ItemCode;
                salesDocDetailViewModel.itemId = (int)i.ItemId;
                salesDocDetailViewModel.margin = i.Margin;
                salesDocDetailViewModel.price = i.Price;
                salesDocDetailViewModel.quantity = i.Quantity;
                salesDocDetailViewModel.specialDiscount = i.SpesialDiscount;
                salesDocDetailViewModel.total = i.Total;


                viewModel.items.Add(salesDocDetailViewModel);
            }
            return viewModel;

        }

        public async Task<SalesDoc> Create(SalesDoc model)
        {
            int Created = 0;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    
                    string code = GenerateCode("sales");
                    model.Code = code;
                    model.FlagForCreate(IdentityService.Username, UserAgent);
                    model.FlagForUpdate(IdentityService.Username, UserAgent);
                    if(model.isReturn != true)
                    {
                        model.isReturn = false;
                    }
                    
                    model.isVoid = false;
                    
                    
                    TransferOutDocViewModel transferOutDocViewModel = new TransferOutDocViewModel();
                    List<TransferOutDocItemViewModel> transferOutDocItemViewModels = new List<TransferOutDocItemViewModel>();
                    TransferInDocViewModel transferInDocView = new TransferInDocViewModel();
                    List<TransferInDocItemViewModel> transferInDocItemViews = new List<TransferInDocItemViewModel>(); 
                    transferOutDocViewModel.code = code;
                    transferOutDocViewModel.reference = code;
                    transferOutDocViewModel.source = new SourceViewModel
                    {
                        _id = model.StoreStorageId,
                        code = model.StoreStorageCode,
                        name = model.StoreStorageName
                    };
                    transferOutDocViewModel.destination = new DestinationViewModel
                    {
                        _id = model.StoreStorageId,
                        code = model.StoreStorageCode,
                        name = model.StoreStorageName
                    };
                    transferOutDocViewModel.remark = model.Remark;
                    bool isAnyTransferIn = false;
                    transferInDocView.code = code;
                    transferInDocView.reference = code;
                    transferInDocView.source = new SourceViewModel
                    {
                        _id = model.StoreStorageId,
                        code = model.StoreStorageCode,
                        name = model.StoreStorageName
                    };
                    transferInDocView.destination = new DestinationViewModel
                    {
                        _id = model.StoreStorageId,
                        code = model.StoreStorageCode,
                        name = model.StoreStorageName
                    };
                    transferInDocView.remark = model.Remark;
                    foreach (var item in model.Details)
                    {
                        if (!item.isReturn)
                        {
                            transferOutDocItemViewModels.Add(new TransferOutDocItemViewModel
                            {
                                articleRealizationOrder = item.ItemArticleRealizationOrder,
                                remark = model.Remark,
                                quantity = item.Quantity,
                                item = new ItemViewModels
                                {
                                    articleRealizationOrder = item.ItemArticleRealizationOrder,
                                    code = item.ItemCode,
                                    domesticCOGS = item.ItemDomesticCOGS,
                                    domesticRetail = item.ItemDomesticRetail,
                                    domesticSale = item.ItemDomesticSale,
                                    domesticWholesale = item.ItemDomesticWholeSale,
                                    name = item.ItemName,
                                    size = item.ItemSize,
                                    uom = item.ItemUom,
                                    _id = item.ItemId,
                                    

                                }
                            });

                        }
                        else if(item.isReturn)
                        {
                            transferInDocItemViews.Add(new TransferInDocItemViewModel
                            {
                                remark = model.Remark,
                                sendquantity = item.Quantity,
                                item = new ItemViewModels
                                {
                                    articleRealizationOrder = item.ItemArticleRealizationOrder,
                                    code = item.ItemCode,
                                    domesticCOGS = item.ItemDomesticCOGS,
                                    domesticRetail = item.ItemDomesticRetail,
                                    domesticSale = item.ItemDomesticSale,
                                    domesticWholesale = item.ItemDomesticWholeSale,
                                    name = item.ItemName,
                                    size = item.ItemSize,
                                    uom = item.ItemUom,
                                    _id = item.ItemId,


                                }
                            });
                            isAnyTransferIn = true;
                        }
                        item.FlagForCreate(IdentityService.Username, UserAgent);
                        item.FlagForUpdate(IdentityService.Username, UserAgent);
                    }
                    transferOutDocViewModel.items = transferOutDocItemViewModels;
                    transferInDocView.items = transferInDocItemViews;

                    if (isAnyTransferIn)
                    {
                        string warehouseUritransferin = "transfer-in/for-pos";
                        var httpClienttarnsfer = (IHttpClientService)ServiceProvider.GetService(typeof(IHttpClientService));
                        var responsetransfer = await httpClienttarnsfer.PostAsync($"{APIEndpoint.Warehouse}{warehouseUritransferin}", new StringContent(JsonConvert.SerializeObject(transferInDocView).ToString(), Encoding.UTF8, General.JsonMediaType));

                        responsetransfer.EnsureSuccessStatusCode();
                    }

                    string warehouseUri = "transfer-out/pos";
                    var httpClient = (IHttpClientService)ServiceProvider.GetService(typeof(IHttpClientService));
                    var response = await httpClient.PostAsync($"{APIEndpoint.Warehouse}{warehouseUri}", new StringContent(JsonConvert.SerializeObject(transferOutDocViewModel).ToString(), Encoding.UTF8, General.JsonMediaType));

                    response.EnsureSuccessStatusCode();

                    model.FlagForCreate(IdentityService.Username, UserAgent);
                    model.FlagForUpdate(IdentityService.Username, UserAgent);

                    DbSet.Add(model);
                    Created = await DbContext.SaveChangesAsync();
                    Created = model.Id;
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }


            }

            return model;
        }

        public async Task<int> Void(int id, string user, int clientTimeZoneOffset = 7)
        {
            int Updated = 0;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var oldM = DbSet.Where(m => m.Id == id)
                    .Include(m => m.Details)
                    .FirstOrDefault();

                    if (oldM != null && oldM.Id == id)
                    {
                        oldM.isVoid = true;
                        oldM.FlagForUpdate(IdentityService.Username, UserAgent);

                        TransferInDocViewModel transferInDocView = new TransferInDocViewModel();
                        List<TransferInDocItemViewModel> transferInDocItems = new List<TransferInDocItemViewModel>();

                        foreach(var i in oldM.Details)
                        {
                            transferInDocItems.Add(new TransferInDocItemViewModel
                            {
                                item = new ItemViewModels
                                {
                                    articleRealizationOrder = i.ItemArticleRealizationOrder,
                                    code = i.ItemCode,
                                    domesticCOGS = i.ItemDomesticCOGS,
                                    domesticRetail = i.ItemDomesticRetail,
                                    domesticSale = i.ItemDomesticSale,
                                    domesticWholesale = i.ItemDomesticWholeSale,
                                    name = i.ItemName,
                                    size = i.ItemSize,
                                    uom = i.ItemUom,
                                    _id = i.ItemId
                                },
                                remark = oldM.Remark,
                                sendquantity = i.Quantity
                            });
                        }
                        transferInDocView.code = GenerateCode("voidsales");
                        transferInDocView.destination = new DestinationViewModel
                        {
                            code = oldM.StoreStorageCode,
                            name = oldM.StoreStorageName,
                            _id = oldM.StoreStorageId
                        };
                        transferInDocView.reference = oldM.Code;
                        transferInDocView.source = new SourceViewModel
                        {
                            code = oldM.StoreStorageCode,
                            name = oldM.StoreStorageName,
                            _id = oldM.StoreStorageId
                        };
                        transferInDocView.items = transferInDocItems;
                        string warehouseUri = "transfer-in/for-pos";
                        var httpClient = (IHttpClientService)ServiceProvider.GetService(typeof(IHttpClientService));
                        var response = await httpClient.PostAsync($"{APIEndpoint.Warehouse}{warehouseUri}", new StringContent(JsonConvert.SerializeObject(transferInDocView).ToString(), Encoding.UTF8, General.JsonMediaType));

                        response.EnsureSuccessStatusCode();

                        //if(oldM.isReturn == true)
                        //{

                        //}
                    }

                    Updated = await DbContext.SaveChangesAsync();
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

        public StoreViewModels GetStore(string storeCode)
        {
            string storeUri = "master/stores/code";
            IHttpClientService httpClient = (IHttpClientService)ServiceProvider.GetService(typeof(IHttpClientService));
            if (httpClient != null)
            {
                var response = httpClient.GetAsync($"{APIEndpoint.Core}{storeUri}?code={storeCode}").Result.Content.ReadAsStringAsync();
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
                StoreViewModels viewModel = JsonConvert.DeserializeObject<StoreViewModels>(result.GetValueOrDefault("data").ToString());
                //viewModel = viewModel.Where(x => x.name == (string.IsNullOrWhiteSpace(storeCode) ? x.name : storeCode)).ToList();
                return viewModel;
            }
            else
            {
                StoreViewModels viewModel = null;
                return viewModel;
            }

        }
    }
}
