using System;
using Sales.Domain.Interfaces;
using Sales.Domain.Enums;
using Sales.Domain.Entities.Product;

namespace Sales.Domain.Entities.Inventory
{
    /// <summary>
    /// Lịch sử giao dịch kho (Thẻ kho). Ghi nhận mọi thay đổi tăng/giảm tồn kho của sản phẩm.
    /// </summary>
    public class InventoryTransaction : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng Products.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Loại giao dịch (Inbound, Outbound, Adjustment...).</summary>
        public TransactionType Type { get; set; }

        /// <summary>Mã tham chiếu chứng từ liên quan (vd: mã Purchase Order, mã Sales Order, mã Phiếu Điều chỉnh). Tối đa 50 ký tự.</summary>
        public string ReferenceNumber { get; set; } = string.Empty;

        /// <summary>Số lượng thao tác theo đơn vị lúc thao tác.</summary>
        public int TransactedQty { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng Units. Đơn vị tính lúc thao tác.</summary>
        public Guid TransactedUomId { get; set; }

        /// <summary>Số lượng quy đổi tuyệt đối cộng/trừ vào kho theo đơn vị cơ bản (dùng để tính tồn kho On-hand).</summary>
        public int BaseQty { get; set; }

        /// <summary>Ghi chú hoặc lý do của giao dịch (vd: Lý do điều chỉnh, Reversal...). Tối đa 500 ký tự.</summary>
        public string? Reason { get; set; }

        // --- Các thuộc tính Audit ---
        /// <summary>Ngày giờ tạo bản ghi (UTC). Đây cũng là ngày phát sinh giao dịch.</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo giao dịch.</summary>
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
        /// <summary>Sản phẩm phát sinh giao dịch (navigation property).</summary>
        public virtual Product.Product Product { get; set; } = null!;

        /// <summary>Đơn vị tính lúc giao dịch (navigation property).</summary>
        public virtual Unit TransactedUom { get; set; } = null!;
    }
}
