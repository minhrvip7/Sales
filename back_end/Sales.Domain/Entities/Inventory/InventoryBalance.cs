using System;
using Sales.Domain.Interfaces;
using Sales.Domain.Entities.Product;

namespace Sales.Domain.Entities.Inventory
{
    /// <summary>
    /// Số dư tồn kho của sản phẩm (thực tế và khả dụng). Mỗi sản phẩm có 1 bản ghi duy nhất trong bảng này.
    /// </summary>
    public class InventoryBalance : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng Products.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Số lượng vật lý đang có trong kho tính đến thời điểm hiện tại.</summary>
        public int OnHandQty { get; set; } = 0;

        /// <summary>Số lượng hàng đã giữ chỗ (nằm trong các Đơn bán hàng đã Confirmed nhưng chưa xuất kho).</summary>
        public int AllocatedQty { get; set; } = 0;

        /// <summary>Số lượng khả dụng dùng để bán (Available = OnHandQty - AllocatedQty).</summary>
        public int AvailableQty { get; set; } = 0;

        // --- Các thuộc tính Audit ---
        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC).</summary>
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
        /// <summary>Sản phẩm liên kết với số dư kho (navigation property).</summary>
        public virtual Product.Product Product { get; set; } = null!;
    }
}
