using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;
using Com.Bateeq.Service.Pos.Lib.Models.SalesReturn;
using Com.Bateeq.Service.Pos.Lib.Models.Discount;
using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;

using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Com.Danliris.Service.Inventory.Lib
{
    public class PosDbContext : BaseDbContext
    {
        public PosDbContext(DbContextOptions<PosDbContext> options) : base(options)
        {
        }

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountItem> DiscountItems { get; set; }
        public DbSet<DiscountDetail> DiscountDetails { get; set; }
        public DbSet<DiscountStore> DiscountStores { get; set; }
        public DbSet<SalesDoc> SalesDocs { get; set; }
        public DbSet<SalesDocDetail> SalesDocDetails { get; set; }
        public DbSet<SalesDocReturn> SalesDocReturns { get; set; }
        public DbSet<SalesDocReturnDetail> SalesDocReturnDetails { get; set; }
        public DbSet<SalesDocDetailReturnItem> SalesDocDetailReturnItems { get; set; }
        //public DbSet<FPReturnInvToPurchasing> FPReturnInvToPurchasings { get; set; }
        //public DbSet<FPReturnInvToPurchasingDetail> FPReturnInvToPurchasingDetails { get; set; }

        //public DbSet<InventoryDocument> InventoryDocuments { get; set; }
        //public DbSet<InventoryDocumentItem> InventoryDocumentItems { get; set; }
        //public DbSet<InventoryMovement> InventoryMovements { get; set; }
        //public DbSet<InventorySummary> InventorySummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new MaterialsRequestNoteConfig());
            //modelBuilder.ApplyConfiguration(new MaterialsRequestNote_ItemConfig());
            //modelBuilder.ApplyConfiguration(new FpRegradingResultDocsDetailsConfig());
            //modelBuilder.ApplyConfiguration(new FpRegradingResultDocsConfig());
            //modelBuilder.ApplyConfiguration(new MaterialDistributionNoteConfig());
            //modelBuilder.ApplyConfiguration(new MaterialDistributionNoteItemConfig());
            //modelBuilder.ApplyConfiguration(new MaterialDistributionNoteDetailConfig());
            //modelBuilder.ApplyConfiguration(new StockTransferNoteConfig());
            //modelBuilder.ApplyConfiguration(new StockTransferNoteItemConfig());
            //modelBuilder.ApplyConfiguration(new FPReturnInvToPurchasingConfig());
            //modelBuilder.ApplyConfiguration(new FPReturnInvToPurchasingDetailConfig());
            //modelBuilder.ApplyConfiguration(new InventoryDocumentConfig());
            //modelBuilder.ApplyConfiguration(new InventoryDocumentItemConfig());
            //modelBuilder.ApplyConfiguration(new InventoryMovementConfig());
            //modelBuilder.ApplyConfiguration(new InventorySummaryConfig());
        }
    }
}