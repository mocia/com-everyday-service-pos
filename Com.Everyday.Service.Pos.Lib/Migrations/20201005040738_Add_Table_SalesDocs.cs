using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Everyday.Service.Pos.Lib.Migrations
{
    public partial class Add_Table_SalesDocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesDocs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    _CreatedUtc = table.Column<DateTime>(nullable: false),
                    _CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    _LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    _LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    _IsDeleted = table.Column<bool>(nullable: false),
                    _DeletedUtc = table.Column<DateTime>(nullable: false),
                    _DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    BankCode = table.Column<string>(maxLength: 255, nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(maxLength: 255, nullable: true),
                    BankCardCode = table.Column<string>(maxLength: 255, nullable: true),
                    BankCardId = table.Column<int>(nullable: false),
                    BankCardName = table.Column<string>(maxLength: 255, nullable: true),
                    Card = table.Column<string>(maxLength: 255, nullable: true),
                    CardAmount = table.Column<double>(nullable: false),
                    CardName = table.Column<string>(maxLength: 255, nullable: true),
                    CardNumber = table.Column<string>(maxLength: 255, nullable: true),
                    CardTypeCode = table.Column<string>(maxLength: 255, nullable: true),
                    CardTypeId = table.Column<int>(nullable: false),
                    CardTypeName = table.Column<string>(maxLength: 255, nullable: true),
                    CashAmount = table.Column<double>(nullable: false),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    isReturn = table.Column<bool>(nullable: false),
                    isVoid = table.Column<bool>(nullable: false),
                    GrandTotal = table.Column<double>(nullable: false),
                    PaymentType = table.Column<string>(maxLength: 255, nullable: true),
                    Reference = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(maxLength: 255, nullable: true),
                    Shift = table.Column<int>(nullable: false),
                    StoreCode = table.Column<string>(maxLength: 255, nullable: true),
                    StoreName = table.Column<string>(maxLength: 255, nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    StoreStorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    StoreStorageName = table.Column<string>(maxLength: 255, nullable: true),
                    StoreStorageId = table.Column<int>(nullable: false),
                    SubTotal = table.Column<double>(nullable: false),
                    TotalProduct = table.Column<double>(nullable: false),
                    VoucherValue = table.Column<double>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesDocDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    _CreatedUtc = table.Column<DateTime>(nullable: false),
                    _CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    _LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    _LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    _IsDeleted = table.Column<bool>(nullable: false),
                    _DeletedUtc = table.Column<DateTime>(nullable: false),
                    _DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    _DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    Discount1 = table.Column<double>(nullable: false),
                    Discount2 = table.Column<double>(nullable: false),
                    DiscountNominal = table.Column<double>(nullable: false),
                    isReturn = table.Column<bool>(nullable: false),
                    ItemArticleRealizationOrder = table.Column<string>(maxLength: 255, nullable: true),
                    ItemCode = table.Column<string>(maxLength: 255, nullable: true),
                    ItemDomesticCOGS = table.Column<double>(nullable: false),
                    ItemDomesticRetail = table.Column<double>(nullable: false),
                    ItemDomesticSale = table.Column<double>(nullable: false),
                    ItemDomesticWholeSale = table.Column<double>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 255, nullable: true),
                    ItemSize = table.Column<string>(maxLength: 255, nullable: true),
                    ItemUom = table.Column<string>(maxLength: 255, nullable: true),
                    Margin = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    PromoCode = table.Column<string>(maxLength: 255, nullable: true),
                    PromoName = table.Column<string>(maxLength: 255, nullable: true),
                    PromoId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Size = table.Column<string>(maxLength: 255, nullable: true),
                    SpesialDiscount = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    SalesDocId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesDocDetails_SalesDocs_SalesDocId",
                        column: x => x.SalesDocId,
                        principalTable: "SalesDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesDocDetails_SalesDocId",
                table: "SalesDocDetails",
                column: "SalesDocId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesDocDetails");

            migrationBuilder.DropTable(
                name: "SalesDocs");
        }
    }
}
