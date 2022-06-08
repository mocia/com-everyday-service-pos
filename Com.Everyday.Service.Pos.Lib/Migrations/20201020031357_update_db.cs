using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Everyday.Service.Pos.Lib.Migrations
{
    public partial class update_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "StoreCode",
                table: "Discounts",
                newName: "UId");

            migrationBuilder.AddColumn<string>(
                name: "StoreCategory",
                table: "SalesDocs",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "DiscountItems",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreCategory",
                table: "SalesDocs");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "DiscountItems");

            migrationBuilder.RenameColumn(
                name: "UId",
                table: "Discounts",
                newName: "StoreCode");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Discounts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
