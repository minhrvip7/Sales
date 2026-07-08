using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Units",
                comment: "Đơn vị tính dùng trong quản lý sản phẩm và giao dịch. Ví dụ: Cái, Hộp, Thùng, Kg, Lít.");

            migrationBuilder.AlterTable(
                name: "ProductUnitConversions",
                comment: "Cấu hình quy đổi đơn vị tính phụ cho sản phẩm. Ví dụ: 1 Thùng = 12 Chai.");

            migrationBuilder.AlterTable(
                name: "Products",
                comment: "Sản phẩm hàng hóa trong hệ thống bán hàng. Mỗi sản phẩm thuộc một nhóm hàng và có một đơn vị tính cơ bản.");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "Đơn hàng bán lẻ. Mỗi đơn gắn với một khách hàng và chứa danh sách mặt hàng đã mua.");

            migrationBuilder.AlterTable(
                name: "OrderDetails",
                comment: "Chi tiết dòng mặt hàng trong đơn hàng. Mỗi dòng là một sản phẩm ở một đơn vị tính cụ thể.");

            migrationBuilder.AlterTable(
                name: "Customers",
                comment: "Thông tin khách hàng mua hàng trong hệ thống bán lẻ.");

            migrationBuilder.AlterTable(
                name: "CustomerGroups",
                comment: "Nhóm khách hàng dùng để phân loại khách hàng theo chính sách giá, chiết khấu.");

            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "Nhóm hàng (danh mục sản phẩm). Dùng để phân loại sản phẩm theo ngành hàng.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Units",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ cập nhật gần nhất (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Units",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng cập nhật lần cuối.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Units",
                type: "boolean",
                nullable: false,
                comment: "Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Tên đơn vị tính. Ví dụ: Chai, Thùng, Kg.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Units",
                type: "text",
                nullable: true,
                comment: "Mô tả thêm về đơn vị tính (tuỳ chọn).",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Units",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Ngày giờ tạo bản ghi (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Units",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng tạo bản ghi.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Units",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã viết tắt đơn vị tính, duy nhất, tối đa 50 ký tự. Ví dụ: CHI, THG.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Units",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                comment: "FK → Products. Sản phẩm sở hữu bản ghi quy đổi này.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductUnitConversions",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "Giá bán riêng khi bán theo đơn vị phụ (tuỳ chọn, VNĐ). Null = tự tính từ giá cơ bản × ConversionRate.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ConversionRate",
                table: "ProductUnitConversions",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                comment: "Tỷ lệ quy đổi: số lượng đơn vị cơ bản = 1 đơn vị phụ. Ví dụ: 12 (1 Thùng = 12 Chai).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "ProductUnitConversions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Mã vạch riêng của đơn vị phụ (tuỳ chọn), duy nhất, tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AlternativeUnitId",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                comment: "FK → Units. Đơn vị tính phụ (ví dụ: Thùng).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ cập nhật gần nhất (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng cập nhật lần cuối.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "integer",
                nullable: false,
                comment: "Tồn kho hiện tại tính theo đơn vị cơ bản. Cập nhật tự động khi có giao dịch.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Products",
                type: "boolean",
                nullable: false,
                comment: "Trạng thái: 1 = đang hoạt động, 0 = ngừng kinh doanh.",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Tên sản phẩm, tối đa 150 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: true,
                comment: "Mô tả chi tiết hoặc ghi chú về sản phẩm (tuỳ chọn).",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Ngày giờ tạo bản ghi (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng tạo bản ghi.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã sản phẩm nội bộ, duy nhất, tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "FK → Categories. Nhóm hàng của sản phẩm.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BaseUnitId",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "FK → Units. Đơn vị tính cơ bản (ví dụ: Chai, Hộp).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Mã vạch của đơn vị cơ bản (tuỳ chọn), tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ cập nhật gần nhất (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng cập nhật lần cuối.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Thành tiền cuối cùng = SubTotal - DiscountAmount + TaxAmount.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Thuế VAT hoặc các loại thuế khác tính trên đơn hàng.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Tổng tiền hàng trước chiết khấu và thuế (= Σ TotalAmount của các dòng chi tiết).",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Orders",
                type: "integer",
                nullable: false,
                comment: "Trạng thái thanh toán: 0=Unpaid (Chưa thanh toán), 1=PartiallyPaid (Thanh toán một phần), 2=FullyPaid (Đã thanh toán đủ).",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "integer",
                nullable: false,
                comment: "Trạng thái xử lý đơn hàng: 0=Draft (Nháp), 1=Confirmed (Đã xác nhận), 2=Completed (Hoàn thành), 3=Cancelled (Đã hủy).",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã số đơn hàng tự sinh, duy nhất. Ví dụ: ORD-20260708-001.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Ngày giờ đặt hàng (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "text",
                nullable: true,
                comment: "Ghi chú nội bộ hoặc yêu cầu đặc biệt của khách hàng.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
                oldScale: 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                comment: "FK → Customers. Khách hàng đặt đơn.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Ngày giờ tạo bản ghi (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng tạo đơn hàng.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Đơn giá bán tại thời điểm lập đơn (VNĐ). Chốt cứng, không thay đổi theo giá sản phẩm sau.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "FK → Units. Đơn vị tính được chọn cho dòng này (có thể là đơn vị phụ).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Thành tiền dòng = UnitPrice × Quantity − DiscountAmount.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderDetails",
                type: "integer",
                nullable: false,
                comment: "Số lượng bán tính theo đơn vị tính đã chọn.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "FK → Products. Sản phẩm được bán trong dòng này.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "FK → Orders. Đơn hàng cha chứa dòng này.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercentage",
                table: "OrderDetails",
                type: "double precision",
                nullable: false,
                comment: "Phần trăm chiết khấu dòng (0–100). Ví dụ: 10.5 = giảm 10.5%.",
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Số tiền chiết khấu dòng = UnitPrice × Quantity × DiscountPercentage / 100.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

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
                oldScale: 4);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Số điện thoại liên hệ, tối đa 20 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Tên khách hàng, tối đa 150 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Địa chỉ email liên hệ, tối đa 100 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Customers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã khách hàng nội bộ, duy nhất, tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                comment: "Địa chỉ giao hàng / liên hệ, tối đa 250 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Customers",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerGroups",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Tên nhóm khách hàng, tối đa 150 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CustomerGroups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã nhóm khách hàng nội bộ, duy nhất, tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CustomerGroups",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Ngày giờ cập nhật gần nhất (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng cập nhật lần cuối.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Categories",
                type: "boolean",
                nullable: false,
                comment: "Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Tên nhóm hàng, tối đa 150 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "Mô tả thêm về nhóm hàng (tuỳ chọn).",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Ngày giờ tạo bản ghi (UTC).",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "ID người dùng tạo bản ghi.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Categories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Mã nhóm hàng nội bộ, duy nhất, tối đa 50 ký tự.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                comment: "Khóa chính (UUID).",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Units",
                oldComment: "Đơn vị tính dùng trong quản lý sản phẩm và giao dịch. Ví dụ: Cái, Hộp, Thùng, Kg, Lít.");

            migrationBuilder.AlterTable(
                name: "ProductUnitConversions",
                oldComment: "Cấu hình quy đổi đơn vị tính phụ cho sản phẩm. Ví dụ: 1 Thùng = 12 Chai.");

            migrationBuilder.AlterTable(
                name: "Products",
                oldComment: "Sản phẩm hàng hóa trong hệ thống bán hàng. Mỗi sản phẩm thuộc một nhóm hàng và có một đơn vị tính cơ bản.");

            migrationBuilder.AlterTable(
                name: "Orders",
                oldComment: "Đơn hàng bán lẻ. Mỗi đơn gắn với một khách hàng và chứa danh sách mặt hàng đã mua.");

            migrationBuilder.AlterTable(
                name: "OrderDetails",
                oldComment: "Chi tiết dòng mặt hàng trong đơn hàng. Mỗi dòng là một sản phẩm ở một đơn vị tính cụ thể.");

            migrationBuilder.AlterTable(
                name: "Customers",
                oldComment: "Thông tin khách hàng mua hàng trong hệ thống bán lẻ.");

            migrationBuilder.AlterTable(
                name: "CustomerGroups",
                oldComment: "Nhóm khách hàng dùng để phân loại khách hàng theo chính sách giá, chiết khấu.");

            migrationBuilder.AlterTable(
                name: "Categories",
                oldComment: "Nhóm hàng (danh mục sản phẩm). Dùng để phân loại sản phẩm theo ngành hàng.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Units",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Ngày giờ cập nhật gần nhất (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Units",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng cập nhật lần cuối.");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Units",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Tên đơn vị tính. Ví dụ: Chai, Thùng, Kg.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Units",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Mô tả thêm về đơn vị tính (tuỳ chọn).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Units",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Ngày giờ tạo bản ghi (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Units",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng tạo bản ghi.");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Units",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã viết tắt đơn vị tính, duy nhất, tối đa 50 ký tự. Ví dụ: CHI, THG.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Units",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Products. Sản phẩm sở hữu bản ghi quy đổi này.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductUnitConversions",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Giá bán riêng khi bán theo đơn vị phụ (tuỳ chọn, VNĐ). Null = tự tính từ giá cơ bản × ConversionRate.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConversionRate",
                table: "ProductUnitConversions",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComment: "Tỷ lệ quy đổi: số lượng đơn vị cơ bản = 1 đơn vị phụ. Ví dụ: 12 (1 Thùng = 12 Chai).");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "ProductUnitConversions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Mã vạch riêng của đơn vị phụ (tuỳ chọn), duy nhất, tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "AlternativeUnitId",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Units. Đơn vị tính phụ (ví dụ: Thùng).");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductUnitConversions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Ngày giờ cập nhật gần nhất (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng cập nhật lần cuối.");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Tồn kho hiện tại tính theo đơn vị cơ bản. Cập nhật tự động khi có giao dịch.");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Trạng thái: 1 = đang hoạt động, 0 = ngừng kinh doanh.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ).");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Tên sản phẩm, tối đa 150 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Mô tả chi tiết hoặc ghi chú về sản phẩm (tuỳ chọn).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Ngày giờ tạo bản ghi (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng tạo bản ghi.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ).");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã sản phẩm nội bộ, duy nhất, tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Categories. Nhóm hàng của sản phẩm.");

            migrationBuilder.AlterColumn<Guid>(
                name: "BaseUnitId",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Units. Đơn vị tính cơ bản (ví dụ: Chai, Hộp).");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Mã vạch của đơn vị cơ bản (tuỳ chọn), tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Ngày giờ cập nhật gần nhất (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng cập nhật lần cuối.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Thành tiền cuối cùng = SubTotal - DiscountAmount + TaxAmount.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Thuế VAT hoặc các loại thuế khác tính trên đơn hàng.");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Tổng tiền hàng trước chiết khấu và thuế (= Σ TotalAmount của các dòng chi tiết).");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Trạng thái thanh toán: 0=Unpaid (Chưa thanh toán), 1=PartiallyPaid (Thanh toán một phần), 2=FullyPaid (Đã thanh toán đủ).");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Trạng thái xử lý đơn hàng: 0=Draft (Nháp), 1=Confirmed (Đã xác nhận), 2=Completed (Hoàn thành), 3=Cancelled (Đã hủy).");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã số đơn hàng tự sinh, duy nhất. Ví dụ: ORD-20260708-001.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Ngày giờ đặt hàng (UTC).");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Ghi chú nội bộ hoặc yêu cầu đặc biệt của khách hàng.");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Số tiền chiết khấu toàn đơn (áp dụng thêm trên mức chiết khấu dòng).");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Customers. Khách hàng đặt đơn.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Ngày giờ tạo bản ghi (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng tạo đơn hàng.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Đơn giá bán tại thời điểm lập đơn (VNĐ). Chốt cứng, không thay đổi theo giá sản phẩm sau.");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Units. Đơn vị tính được chọn cho dòng này (có thể là đơn vị phụ).");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Thành tiền dòng = UnitPrice × Quantity − DiscountAmount.");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Số lượng bán tính theo đơn vị tính đã chọn.");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Products. Sản phẩm được bán trong dòng này.");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "FK → Orders. Đơn hàng cha chứa dòng này.");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercentage",
                table: "OrderDetails",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComment: "Phần trăm chiết khấu dòng (0–100). Ví dụ: 10.5 = giảm 10.5%.");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "OrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Số tiền chiết khấu dòng = UnitPrice × Quantity × DiscountPercentage / 100.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConversionRate",
                table: "OrderDetails",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComment: "Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1 (bán theo đơn vị cơ bản).");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderDetails",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "Số điện thoại liên hệ, tối đa 20 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Tên khách hàng, tối đa 150 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Địa chỉ email liên hệ, tối đa 100 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Customers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã khách hàng nội bộ, duy nhất, tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true,
                oldComment: "Địa chỉ giao hàng / liên hệ, tối đa 250 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Customers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerGroups",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Tên nhóm khách hàng, tối đa 150 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CustomerGroups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã nhóm khách hàng nội bộ, duy nhất, tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CustomerGroups",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Ngày giờ cập nhật gần nhất (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng cập nhật lần cuối.");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Categories",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Tên nhóm hàng, tối đa 150 ký tự.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Mô tả thêm về nhóm hàng (tuỳ chọn).");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Ngày giờ tạo bản ghi (UTC).");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "ID người dùng tạo bản ghi.");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Categories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Mã nhóm hàng nội bộ, duy nhất, tối đa 50 ký tự.");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Khóa chính (UUID).");
        }
    }
}
