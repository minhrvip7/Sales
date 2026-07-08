using System;

namespace Sales.Application.DTOs.Product
{
    /// <summary>Thông tin quy đổi đơn vị tính phụ cho sản phẩm.</summary>
    public class ProductUnitConversionDto
    {
        /// <summary>ID đơn vị tính phụ. Ví dụ: Thùng, Lốc. Phải tồn tại trong hệ thống.</summary>
        public Guid AlternativeUnitId { get; set; }

        /// <summary>Tên đơn vị tính phụ (đã được resolve từ AlternativeUnitId).</summary>
        public string AlternativeUnitName { get; set; } = string.Empty;

        /// <summary>
        /// Tỷ lệ quy đổi: số lượng đơn vị cơ bản tương đương 1 đơn vị phụ.
        /// Ví dụ: 1 Thùng = 12 Chai → ConversionRate = 12. Phải > 0.
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>Mã vạch riêng của đơn vị tính phụ (tuỳ chọn). Nếu cung cấp, phải duy nhất trong hệ thống.</summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// Giá bán riêng khi bán theo đơn vị phụ này (tuỳ chọn, VNĐ).
        /// Nếu không cung cấp, hệ thống tự tính: giá cơ bản × ConversionRate.
        /// </summary>
        public decimal? Price { get; set; }
    }
}
