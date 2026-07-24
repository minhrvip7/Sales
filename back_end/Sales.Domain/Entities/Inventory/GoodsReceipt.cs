using System;
using System.Collections.Generic;
using Sales.Domain.Enums;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Inventory
{
    /// <summary>
    /// Thực thể đại diện cho một Phiếu Nhập Kho.
    /// </summary>
    public class GoodsReceipt : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Mã phiếu nhập kho (Ví dụ: GR-YYMM-00001).</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Loại phiếu nhập kho.</summary>
        public GoodsReceiptType Type { get; set; }

        /// <summary>Khóa ngoại trỏ đến chứng từ gốc (Purchase Order, Return Order, Transfer Order).</summary>
        public Guid? ReferenceId { get; set; }

        /// <summary>Mã chứng từ gốc để hiển thị nhanh.</summary>
        public string ReferenceCode { get; set; } = string.Empty;

        /// <summary>Khóa ngoại trỏ đến Nhà cung cấp (Lấy từ PO). FK -> Customers (hoặc Suppliers).</summary>
        public Guid? SupplierId { get; set; }

        /// <summary>Kho nhập. FK -> Warehouses (nếu có, ở đây có thể lưu dưới dạng tham chiếu hoặc enum nếu không có bảng Warehouse).</summary>
        public Guid WarehouseId { get; set; }

        /// <summary>Ngày nhập kho thực tế.</summary>
        public DateTime ReceiptDate { get; set; }

        /// <summary>Ghi chú cho phiếu nhập kho.</summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>Trạng thái của phiếu nhập kho.</summary>
        public GoodsReceiptStatus Status { get; set; } = GoodsReceiptStatus.Draft;

        /// <summary>Tổng số lượng thực tế của tất cả các dòng sản phẩm.</summary>
        public decimal TotalQuantity { get; set; }

        // --- ISoftDelete ---
        /// <summary>Cờ xóa mềm: true = đã bị xóa mềm, không hiển thị trong các query thông thường.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // --- Audit ---
        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC). Null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>ID người dùng cập nhật lần cuối.</summary>
        public Guid? UpdatedBy { get; set; }

        // --- Navigation Properties ---
        /// <summary>Danh sách các dòng sản phẩm chi tiết trong phiếu nhập.</summary>
        public virtual ICollection<GoodsReceiptLine> Lines { get; set; } = new List<GoodsReceiptLine>();
    }
}
