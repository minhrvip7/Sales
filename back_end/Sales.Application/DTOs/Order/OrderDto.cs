using System;
using System.Collections.Generic;
using Sales.Domain.Enums;

namespace Sales.Application.DTOs.Order
{
    /// <summary>Thông tin chi tiết đơn hàng trả về cho client.</summary>
    public class OrderDto
    {
        /// <summary>Định danh duy nhất của đơn hàng (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Mã số đơn hàng tự sinh, ví dụ: <c>ORD-20260708-001</c>.</summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>ID khách hàng đặt đơn.</summary>
        public Guid CustomerId { get; set; }

        /// <summary>Tên khách hàng (đã được resolve từ CustomerId).</summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>Ngày giờ đặt hàng (UTC).</summary>
        public DateTime OrderDate { get; set; }

        /// <summary>Tổng tiền hàng trước chiết khấu và thuế.</summary>
        public decimal SubTotal { get; set; }

        /// <summary>Số tiền chiết khấu toàn đơn.</summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>Thuế áp dụng trên đơn hàng.</summary>
        public decimal TaxAmount { get; set; }

        /// <summary>Thành tiền cuối cùng khách phải thanh toán.</summary>
        public decimal TotalAmount { get; set; }

        /// <summary>Ghi chú hoặc yêu cầu đặc biệt (tuỳ chọn).</summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Trạng thái xử lý đơn hàng.
        /// 0 = Draft (Nháp), 1 = Confirmed (Đã xác nhận), 2 = Completed (Hoàn thành), 3 = Cancelled (Đã hủy).
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// Trạng thái thanh toán đơn hàng.
        /// 0 = Unpaid (Chưa thanh toán), 1 = PartiallyPaid (Thanh toán một phần), 2 = FullyPaid (Đã thanh toán đủ).
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>Danh sách dòng chi tiết mặt hàng trong đơn hàng.</summary>
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}
