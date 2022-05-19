using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Everyday.Service.Pos.Lib.Migrations
{
    public partial class Add_Table_SalesReturn_and_SalesReturnItemsDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesDocDetailReturnItems",
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
                    SalesDocDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocDetailReturnItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesDocReturns",
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
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    IsVoid = table.Column<bool>(nullable: false),
                    SalesDocId = table.Column<int>(nullable: false),
                    SalesDocCode = table.Column<string>(maxLength: 255, nullable: true),
                    SalesDocIsReturn = table.Column<bool>(nullable: false),
                    SalesDocReturnId = table.Column<int>(nullable: false),
                    SalesDocReturnCode = table.Column<string>(maxLength: 255, nullable: true),
                    SalesDocReturnIsReturn = table.Column<bool>(nullable: false),
                    StoreCode = table.Column<string>(maxLength: 255, nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    StoreName = table.Column<string>(maxLength: 255, nullable: true),
                    StoreStorageName = table.Column<string>(maxLength: 255, nullable: true),
                    StoreStorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    StoreStorageId = table.Column<int>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocReturns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesDocReturnDetails",
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
                    SalesReturnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesDocReturnDetails_SalesDocReturns_SalesReturnId",
                        column: x => x.SalesReturnId,
                        principalTable: "SalesDocReturns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesDocReturnDetails_SalesReturnId",
                table: "SalesDocReturnDetails",
                column: "SalesReturnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesDocDetailReturnItems");

            migrationBuilder.DropTable(
                name: "SalesDocReturnDetails");

            migrationBuilder.DropTable(
                name: "SalesDocReturns");
        }
    }
}
