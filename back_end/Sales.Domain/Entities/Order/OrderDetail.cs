using System;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Order
{
    /// <summary>
    /// Chi tiết một dòng mặt hàng trong đơn hàng. Mỗi dòng tương ứng với một sản phẩm
    /// ở một đơn vị tính cụ thể (có thể khác đơn vị cơ bản).
    /// </summary>
    public class OrderDetail : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của dòng chi tiết (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Orders</c>.</summary>
        public Guid OrderId { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Products</c>.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Đơn vị tính được chọn cho dòng này (có thể là đơn vị phụ).</summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Tỷ lệ quy đổi từ đơn vị bán sang đơn vị cơ bản. Mặc định = 1.
        /// </summary>
        public decimal ConversionRate { get; set; } = 1;

        /// <summary>Số lượng bán tính theo đơn vị tính đã chọn.</summary>
        public int Quantity { get; set; }

        /// <summary>Đơn giá bán tại thời điểm lập đơn (VNĐ). Chốt cứng, không thay đổi theo giá sản phẩm sau.</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>Phần trăm chiết khấu dòng (0–100).</summary>
        public double DiscountPercentage { get; set; } = 0;

        /// <summary>Số tiền chiết khấu dòng = UnitPrice × Quantity × DiscountPercentage / 100.</summary>
        public decimal DiscountAmount { get; set; } = 0;

        /// <summary>Thành tiền dòng = UnitPrice × Quantity − DiscountAmount.</summary>
        public decimal TotalAmount { get; set; }

        // ISoftDelete
        /// <summary>Cờ xóa mềm: <c>true</c> = dòng đã bị xóa mềm.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // Relationships
        /// <summary>Đơn hàng cha chứa dòng chi tiết này (navigation property).</summary>
        public virtual Order Order { get; set; } = null!;

        /// <summary>Sản phẩm được bán trong dòng này (navigation property).</summary>
        public virtual Product.Product Product { get; set; } = null!;

        /// <summary>Đơn vị tính được áp dụng cho dòng này (navigation property).</summary>
        public virtual Product.Unit Unit { get; set; } = null!;
    }
}
