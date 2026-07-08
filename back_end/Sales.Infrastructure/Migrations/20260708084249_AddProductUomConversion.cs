using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUomConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Products",
                newName: "BaseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                newName: "IX_Products_BaseUnitId");

            migrationBuilder.AddColumn<decimal>(
                name: "ConversionRate",
                table: "OrderDetails",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductUnitConversions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlternativeUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversionRate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnitConversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUnitConversions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUnitConversions_Units_AlternativeUnitId",
                        column: x => x.AlternativeUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_UnitId",
                table: "OrderDetails",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnitConversions_AlternativeUnitId",
                table: "ProductUnitConversions",
                column: "AlternativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnitConversions_Barcode",
                table: "ProductUnitConversions",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnitConversions_ProductId",
                table: "ProductUnitConversions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Units_UnitId",
                table: "OrderDetails",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_BaseUnitId",
                table: "Products",
                column: "BaseUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Units_UnitId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_BaseUnitId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductUnitConversions");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_UnitId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ConversionRate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "BaseUnitId",
                table: "Products",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BaseUnitId",
                table: "Products",
                newName: "IX_Products_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
