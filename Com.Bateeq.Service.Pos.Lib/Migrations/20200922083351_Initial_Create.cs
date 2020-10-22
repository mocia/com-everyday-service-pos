using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Bateeq.Service.Pos.Lib.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
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
                    DiscountOne = table.Column<int>(nullable: false),
                    DiscountTwo = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    Information = table.Column<string>(maxLength: 255, nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    StoreCategory = table.Column<string>(maxLength: 255, nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    StoreCode = table.Column<string>(maxLength: 255, nullable: true),
                    StoreName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountItems",
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
                    RealizationOrder = table.Column<string>(maxLength: 255, nullable: true),
                    DiscountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountItems_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountDetails",
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
                    ArticleRealizationOrder = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    DomesticCOGS = table.Column<float>(nullable: false),
                    DomesticRetail = table.Column<float>(nullable: false),
                    DomesticSale = table.Column<float>(nullable: false),
                    DomesticWholesale = table.Column<float>(nullable: false),
                    InternationalCOGS = table.Column<float>(nullable: false),
                    InternationalRetail = table.Column<float>(nullable: false),
                    InternationalSale = table.Column<float>(nullable: false),
                    InternationalWholesale = table.Column<float>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Size = table.Column<string>(maxLength: 255, nullable: true),
                    Uom = table.Column<string>(maxLength: 255, nullable: true),
                    Uid = table.Column<string>(maxLength: 255, nullable: true),
                    DiscountItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountDetails_DiscountItems_DiscountItemId",
                        column: x => x.DiscountItemId,
                        principalTable: "DiscountItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDetails_DiscountItemId",
                table: "DiscountDetails",
                column: "DiscountItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountItems_DiscountId",
                table: "DiscountItems",
                column: "DiscountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountDetails");

            migrationBuilder.DropTable(
                name: "DiscountItems");

            migrationBuilder.DropTable(
                name: "Discounts");
        }
    }
}
