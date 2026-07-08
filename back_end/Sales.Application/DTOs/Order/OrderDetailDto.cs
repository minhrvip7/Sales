using System;

namespace Sales.Application.DTOs.Order
{
    /// <summary>Thông tin chi tiết một dòng mặt hàng trong đơn hàng trả về cho client.</summary>
    public class OrderDetailDto
    {
        /// <summary>Định danh duy nhất của dòng chi tiết (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>ID sản phẩm trong dòng này.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Tên sản phẩm (đã được resolve từ ProductId).</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>Mã sản phẩm nội bộ.</summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>ID đơn vị tính được sử dụng cho dòng này.</summary>
        public Guid UnitId { get; set; }

        /// <summary>Tên đơn vị tính (đã được resolve từ UnitId). Ví dụ: "Thùng", "Chai".</summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>Tỷ lệ quy đổi từ đơn vị bán sang đơn vị cơ bản. Mặc định = 1.</summary>
        public decimal ConversionRate { get; set; }

        /// <summary>Số lượng bán tính theo đơn vị tính đã chọn.</summary>
        public int Quantity { get; set; }

        /// <summary>Đơn giá bán tại thời điểm lập đơn (VNĐ).</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>Phần trăm chiết khấu áp dụng cho dòng này (0–100).</summary>
        public double DiscountPercentage { get; set; }

        /// <summary>Số tiền chiết khấu tính ra (VNĐ).</summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>Thành tiền dòng = UnitPrice × Quantity − DiscountAmount.</summary>
        public decimal TotalAmount { get; set; }
    }
}
