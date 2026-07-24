using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_GoodsReceipt_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsReceipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Khóa chính (UUID)."),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Mã phiếu nhập kho (Ví dụ: GR-YYMM-00001)."),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Loại phiếu: 1=Purchase, 2=Return, 3=Transfer."),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Mã chứng từ gốc để hiển thị nhanh."),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Ghi chú cho phiếu nhập kho."),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Trạng thái: 0=Draft, 1=Completed, 2=Cancelled."),
                    TotalQuantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Tổng số lượng thực tế của tất cả các dòng sản phẩm."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Cờ xóa mềm."),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceipts", x => x.Id);
                },
                comment: "Bảng lưu trữ thông tin phiếu nhập kho (Header).");

            migrationBuilder.CreateTable(
                name: "GoodsReceiptLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Khóa chính (UUID)."),
                    GoodsReceiptId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → GoodsReceipts."),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Products."),
                    UoMId = table.Column<Guid>(type: "uuid", nullable: false, comment: "FK → Units."),
                    ConversionRate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false, comment: "Tỷ lệ quy đổi so với đơn vị cơ bản lúc nhập."),
                    ExpectedQuantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Số lượng dự kiến từ chứng từ gốc."),
                    ActualQuantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Số lượng thực tế nhập kho."),
                    Notes = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, comment: "Ghi chú tại dòng."),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Cờ xóa mềm."),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptLines_GoodsReceipts_GoodsReceiptId",
                        column: x => x.GoodsReceiptId,
                        principalTable: "GoodsReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Dòng chi tiết trong phiếu nhập kho.");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptLines_GoodsReceiptId",
                table: "GoodsReceiptLines",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_Code",
                table: "GoodsReceipts",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsReceiptLines");

            migrationBuilder.DropTable(
                name: "GoodsReceipts");
        }
    }
}
