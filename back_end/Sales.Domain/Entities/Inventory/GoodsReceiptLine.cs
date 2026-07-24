using System;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Inventory
{
    /// <summary>
    /// Thực thể đại diện cho một Dòng chi tiết trong Phiếu Nhập Kho.
    /// </summary>
    public class GoodsReceiptLine : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Khóa ngoại trỏ đến Phiếu Nhập Kho tương ứng. FK -> GoodsReceipts.</summary>
        public Guid GoodsReceiptId { get; set; }

        /// <summary>Phiếu Nhập Kho liên quan.</summary>
        public virtual GoodsReceipt GoodsReceipt { get; set; } = null!;

        /// <summary>Khóa ngoại trỏ đến Sản phẩm. FK -> Products.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Khóa ngoại trỏ đến Đơn vị tính (Unit of Measure). FK -> Units.</summary>
        public Guid UoMId { get; set; }

        /// <summary>Tỷ lệ quy đổi so với đơn vị cơ bản lúc nhập.</summary>
        public decimal ConversionRate { get; set; } = 1;

        /// <summary>Số lượng dự kiến từ chứng từ gốc (tính theo UoMId).</summary>
        public decimal ExpectedQuantity { get; set; }

        /// <summary>Số lượng thực tế nhập kho (tính theo UoMId).</summary>
        public decimal ActualQuantity { get; set; }

        /// <summary>Ghi chú tại từng dòng (VD: Hàng móp méo, ướt...).</summary>
        public string Notes { get; set; } = string.Empty;

        // --- ISoftDelete ---
        /// <summary>Cờ xóa mềm.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC).</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // --- Audit ---
        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC).</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>ID người dùng cập nhật lần cuối.</summary>
        public Guid? UpdatedBy { get; set; }
    }
}
