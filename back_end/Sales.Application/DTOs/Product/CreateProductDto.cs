using System;
using System.Collections.Generic;

namespace Sales.Application.DTOs.Product
{
    /// <summary>Dữ liệu đầu vào để tạo mới hoặc cập nhật sản phẩm.</summary>
    public class CreateProductDto
    {
        /// <summary>Tên sản phẩm, tối đa 150 ký tự. Bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã sản phẩm nội bộ, tối đa 50 ký tự, phải duy nhất. Bắt buộc.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mã vạch của đơn vị cơ bản (tuỳ chọn). Nếu cung cấp, phải duy nhất.</summary>
        public string? Barcode { get; set; }

        /// <summary>Mô tả chi tiết hoặc ghi chú về sản phẩm (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ). Phải ≥ 0.</summary>
        public decimal Price { get; set; }

        /// <summary>Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ). Phải ≥ 0.</summary>
        public decimal Cost { get; set; }

        /// <summary>ID nhóm hàng của sản phẩm. Bắt buộc, phải tồn tại trong hệ thống.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>ID đơn vị tính cơ bản. Bắt buộc, phải tồn tại trong hệ thống.</summary>
        public Guid BaseUnitId { get; set; }

        /// <summary>Danh sách quy đổi đơn vị tính phụ (tuỳ chọn). Ví dụ: 1 Thùng = 12 Chai.</summary>
        public List<ProductUnitConversionDto> Conversions { get; set; } = new List<ProductUnitConversionDto>();
    }
}
