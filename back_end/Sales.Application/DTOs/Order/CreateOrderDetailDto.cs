using System;

namespace Sales.Application.DTOs.Order
{
    /// <summary>Dữ liệu đầu vào cho một dòng mặt hàng khi tạo đơn hàng.</summary>
    public class CreateOrderDetailDto
    {
        /// <summary>ID sản phẩm cần thêm vào đơn. Bắt buộc, phải tồn tại trong hệ thống.</summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// ID đơn vị tính cho dòng này. Có thể là đơn vị cơ bản hoặc đơn vị phụ đã được cấu hình quy đổi.
        /// Bắt buộc.
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>Số lượng bán tính theo đơn vị tính đã chọn. Phải lớn hơn 0.</summary>
        public int Quantity { get; set; }

        /// <summary>Phần trăm chiết khấu cho dòng này (0–100). Mặc định = 0 (không giảm giá).</summary>
        public double DiscountPercentage { get; set; }
    }
}
