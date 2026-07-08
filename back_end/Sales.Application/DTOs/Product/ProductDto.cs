using System;
using System.Collections.Generic;

namespace Sales.Application.DTOs.Product
{
    /// <summary>Thông tin chi tiết sản phẩm trả về cho client.</summary>
    public class ProductDto
    {
        /// <summary>Định danh duy nhất của sản phẩm (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên sản phẩm.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã sản phẩm nội bộ, duy nhất trong hệ thống.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mã vạch của đơn vị cơ bản (tuỳ chọn).</summary>
        public string? Barcode { get; set; }

        /// <summary>Mô tả chi tiết hoặc ghi chú về sản phẩm (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ).</summary>
        public decimal Price { get; set; }

        /// <summary>Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ).</summary>
        public decimal Cost { get; set; }

        /// <summary>Tồn kho hiện tại tính theo đơn vị cơ bản.</summary>
        public int StockQuantity { get; set; }

        /// <summary>ID nhóm hàng của sản phẩm.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Tên nhóm hàng (đã được resolve từ CategoryId).</summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>ID đơn vị tính cơ bản.</summary>
        public Guid BaseUnitId { get; set; }

        /// <summary>Tên đơn vị tính cơ bản (đã được resolve từ BaseUnitId). Ví dụ: "Chai".</summary>
        public string BaseUnitName { get; set; } = string.Empty;

        /// <summary>Trạng thái sản phẩm: <c>true</c> = đang hoạt động, <c>false</c> = ngừng kinh doanh.</summary>
        public bool Status { get; set; }

        /// <summary>
        /// Cho biết sản phẩm đã có giao dịch hay chưa.
        /// <c>true</c> = đã xuất hiện trong đơn hàng, không được phép xóa.
        /// </summary>
        public bool HasTransactions { get; set; }

        /// <summary>Danh sách quy đổi đơn vị tính phụ của sản phẩm.</summary>
        public List<ProductUnitConversionDto> Conversions { get; set; } = new List<ProductUnitConversionDto>();
    }
}
