using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Units",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Units",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Units",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ProductUnitConversions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductUnitConversions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm.");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm. Dữ liệu vẫn giữ lại để hiển thị trong lịch sử đơn hàng và báo cáo.");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Số tiền chiết khấu toàn đơn.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Số tiền chiết khấu toàn đơn (áp dụng thêm trên mức chiết khấu dòng).");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "FK → Units. Đơn vị tính được chọn cho dòng này.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Units. Đơn vị tính được chọn cho dòng này (có thể là đơn vị phụ).");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercentage",
                table: "OrderDetails",
                type: "double precision",
                nullable: false,
                comment: "Phần trăm chiết khấu dòng (0–100).",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComment: "Phần trăm chiết khấu dòng (0–100). Ví dụ: 10.5 = giảm 10.5%.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConversionRate",
                table: "OrderDetails",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                comment: "Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComment: "Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1 (bán theo đơn vị cơ bản).");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "OrderDetails",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "OrderDetails",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = dòng đã bị xóa mềm.");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Customers",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "CustomerGroups",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "CustomerGroups",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng thực hiện xóa mềm.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ProductUnitConversions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ProductUnitConversions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductUnitConversions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CustomerGroups");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "CustomerGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerGroups");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Số tiền chiết khấu toàn đơn (áp dụng thêm trên mức chiết khấu dòng).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Số tiền chiết khấu toàn đơn.");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "FK → Units. Đơn vị tính được chọn cho dòng này (có thể là đơn vị phụ).",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Units. Đơn vị tính được chọn cho dòng này.");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercentage",
                table: "OrderDetails",
                type: "double precision",
                nullable: false,
                comment: "Phần trăm chiết khấu dòng (0–100). Ví dụ: 10.5 = giảm 10.5%.",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComment: "Phần trăm chiết khấu dòng (0–100).");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConversionRate",
                table: "OrderDetails",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                comment: "Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1 (bán theo đơn vị cơ bản).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComment: "Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1.");
        }
    }
}
