using System;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Product
{
    /// <summary>
    /// Cấu hình quy đổi đơn vị tính phụ cho sản phẩm. Cho phép bán sản phẩm theo
    /// nhiều đơn vị khác nhau (ví dụ: bán lẻ theo Chai, bán sỉ theo Thùng = 12 Chai).
    /// </summary>
    public class ProductUnitConversion : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của bản ghi quy đổi (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Products</c>.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Units</c> – đơn vị tính phụ (ví dụ: Thùng).</summary>
        public Guid AlternativeUnitId { get; set; }

        /// <summary>
        /// Tỷ lệ quy đổi: số lượng đơn vị cơ bản tương đương 1 đơn vị phụ.
        /// Ví dụ: 1 Thùng = 12 Chai → ConversionRate = 12.
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>Mã vạch riêng của đơn vị tính phụ (tuỳ chọn). Tối đa 50 ký tự, duy nhất.</summary>
        public string? Barcode { get; set; }

        /// <summary>Giá bán riêng khi bán theo đơn vị phụ (tuỳ chọn, VNĐ). Null = tự tính từ giá cơ bản × ConversionRate.</summary>
        public decimal? Price { get; set; }

        // ISoftDelete
        /// <summary>Cờ xóa mềm: <c>true</c> = đã bị xóa, không hiển thị trong các query thông thường.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // Relationships
        /// <summary>Sản phẩm sở hữu bản ghi quy đổi này (navigation property).</summary>
        public virtual Product Product { get; set; } = null!;

        /// <summary>Đơn vị tính phụ (navigation property).</summary>
        public virtual Unit AlternativeUnit { get; set; } = null!;
    }
}
