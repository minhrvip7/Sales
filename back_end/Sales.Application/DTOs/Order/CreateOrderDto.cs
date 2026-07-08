using System;
using System.Collections.Generic;

namespace Sales.Application.DTOs.Order
{
    /// <summary>Dữ liệu đầu vào để tạo mới một đơn hàng.</summary>
    public class CreateOrderDto
    {
        /// <summary>ID khách hàng đặt đơn. Bắt buộc, phải tồn tại trong hệ thống.</summary>
        public Guid CustomerId { get; set; }

        /// <summary>Ghi chú hoặc yêu cầu đặc biệt của khách hàng (tuỳ chọn).</summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Danh sách dòng chi tiết mặt hàng cần thêm vào đơn. Bắt buộc phải có ít nhất 1 dòng.
        /// </summary>
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
    }
}
