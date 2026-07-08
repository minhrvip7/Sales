using System;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Product
{
    /// <summary>
    /// Đơn vị tính dùng trong quản lý sản phẩm và giao dịch bán hàng.
    /// Ví dụ: Cái, Hộp, Thùng, Kg, Lít.
    /// </summary>
    public class Unit : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của đơn vị tính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên đơn vị tính, tối đa 150 ký tự, bắt buộc. Ví dụ: "Chai", "Thùng".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã viết tắt của đơn vị tính, duy nhất, tối đa 50 ký tự. Ví dụ: "CHI", "THG".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về đơn vị tính (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Trạng thái: <c>true</c> = đang sử dụng, <c>false</c> = đã ẩn/ngừng dùng.</summary>
        public bool Status { get; set; } = true;

        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
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
    }
}
