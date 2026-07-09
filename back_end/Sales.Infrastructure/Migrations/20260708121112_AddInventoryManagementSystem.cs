using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Khóa chính (UUID)."),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Products. Sản phẩm của số dư này."),
                    OnHandQty = table.Column<int>(type: "integer", nullable: false, comment: "Số lượng vật lý đang có trong kho."),
                    AllocatedQty = table.Column<int>(type: "integer", nullable: false, comment: "Số lượng hàng đã giữ chỗ (Sales Order Confirmed)."),
                    AvailableQty = table.Column<int>(type: "integer", nullable: false, comment: "Số lượng khả dụng dùng để bán (Available = OnHandQty - AllocatedQty)."),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Cờ xóa mềm: true = bản ghi đã bị xóa mềm."),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBalances_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Bảng số dư tồn kho của sản phẩm, dùng để kiểm soát tồn kho tức thời.");

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Khóa chính (UUID)."),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Products."),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Loại giao dịch: 1=Inbound, 2=Outbound, 3=AdjustmentIn, 4=AdjustmentOut, 5=OtherIssue."),
                    ReferenceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Mã chứng từ tham chiếu (VD: PO-123, SO-456)."),
                    TransactedQty = table.Column<int>(type: "integer", nullable: false, comment: "Số lượng giao dịch theo đơn vị lúc thao tác."),
                    TransactedUomId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Units. Đơn vị thao tác."),
                    BaseQty = table.Column<int>(type: "integer", nullable: false, comment: "Số lượng quy đổi theo đơn vị cơ bản (cộng/trừ)."),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Ghi chú/Lý do."),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Cờ xóa mềm."),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Units_TransactedUomId",
                        column: x => x.TransactedUomId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Bảng thẻ kho ghi nhận lịch sử giao dịch (Nhập, Xuất, Điều chỉnh).");

            migrationBuilder.CreateTable(
                name: "ProductCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Khóa chính (UUID)."),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Products."),
                    MovingAverageCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Giá vốn bình quân theo đơn vị cơ bản."),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Ngày giờ hiệu lực của mức giá vốn."),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Cờ xóa mềm."),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCosts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Lịch sử giá vốn bình quân gia quyền (Moving Average Cost) của sản phẩm.");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalances_ProductId",
                table: "InventoryBalances",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductId",
                table: "InventoryTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_TransactedUomId",
                table: "InventoryTransactions",
                column: "TransactedUomId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCosts_ProductId",
                table: "ProductCosts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryBalances");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "ProductCosts");
        }
    }
}
