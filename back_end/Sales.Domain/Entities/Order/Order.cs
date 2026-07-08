using System;
using System.Collections.Generic;
using Sales.Domain.Enums;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Order
{
    /// <summary>
    /// Đơn hàng bán lẻ. Mỗi đơn hàng gắn với một khách hàng và chứa danh sách
    /// chi tiết mặt hàng đã mua.
    /// </summary>
    public class Order : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của đơn hàng (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Mã số đơn hàng tự sinh, ví dụ: <c>ORD-20260708-001</c>. Duy nhất trong hệ thống.</summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Customers</c>.</summary>
        public Guid CustomerId { get; set; }

        /// <summary>Ngày giờ đặt hàng (UTC).</summary>
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>Tổng tiền hàng trước khi chiết khấu và thuế (= Σ TotalAmount của các dòng chi tiết).</summary>
        public decimal SubTotal { get; set; } = 0;

        /// <summary>Số tiền chiết khấu toàn đơn.</summary>
        public decimal DiscountAmount { get; set; } = 0;

        /// <summary>Thuế VAT hoặc các loại thuế khác tính trên đơn hàng.</summary>
        public decimal TaxAmount { get; set; } = 0;

        /// <summary>Thành tiền cuối cùng = SubTotal - DiscountAmount + TaxAmount.</summary>
        public decimal TotalAmount { get; set; } = 0;

        /// <summary>Ghi chú nội bộ hoặc yêu cầu đặc biệt của khách hàng (tuỳ chọn).</summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Trạng thái xử lý đơn hàng.
        /// 0=Draft (Nháp), 1=Confirmed (Đã xác nhận), 2=Completed (Hoàn thành), 3=Cancelled (Đã hủy).
        /// </summary>
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Draft;

        /// <summary>
        /// Trạng thái thanh toán.
        /// 0=Unpaid (Chưa thanh toán), 1=PartiallyPaid (Thanh toán một phần), 2=FullyPaid (Đã thanh toán đủ).
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo đơn hàng.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC). Null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>ID người dùng cập nhật lần cuối.</summary>
        public Guid? UpdatedBy { get; set; }

        // ISoftDelete
        /// <summary>Cờ xóa mềm: <c>true</c> = đã bị xóa, không hiển thị trong các query thông thường.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // Relationships
        /// <summary>Thông tin khách hàng đặt đơn (navigation property).</summary>
        public virtual Customer.Customer Customer { get; set; } = null!;

        /// <summary>Danh sách chi tiết mặt hàng trong đơn hàng.</summary>
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
