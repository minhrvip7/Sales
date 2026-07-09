using System;
using Sales.Domain.Interfaces;
using Sales.Domain.Entities.Product;

namespace Sales.Domain.Entities.Inventory
{
    /// <summary>
    /// Lưu trữ lịch sử giá vốn bình quân gia quyền (Moving Average Cost) của sản phẩm.
    /// Giá vốn này được cập nhật sau mỗi lần nhập kho.
    /// </summary>
    public class ProductCost : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng Products.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Giá vốn bình quân tính theo đơn vị cơ bản tại thời điểm tính toán (VNĐ).</summary>
        public decimal MovingAverageCost { get; set; } = 0;

        /// <summary>Ngày giờ hiệu lực của mức giá vốn này (thường trùng với thời điểm giao dịch nhập kho).</summary>
        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;

        // --- Các thuộc tính Audit ---
        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC). Null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>ID người dùng cập nhật lần cuối.</summary>
        public Guid? UpdatedBy { get; set; }

        // --- ISoftDelete ---
        /// <summary>Cờ xóa mềm: true = đã bị xóa mềm.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC).</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // --- Relationships ---
        /// <summary>Sản phẩm áp dụng giá vốn (navigation property).</summary>
        public virtual Product.Product Product { get; set; } = null!;
    }
}
