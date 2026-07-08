using System;
using System.Collections.Generic;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Customer
{
    /// <summary>
    /// Thông tin khách hàng mua hàng trong hệ thống bán lẻ.
    /// </summary>
    public class Customer : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của khách hàng (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên khách hàng, tối đa 150 ký tự, bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã khách hàng nội bộ, duy nhất, tối đa 50 ký tự.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Số điện thoại liên hệ, tối đa 20 ký tự (tuỳ chọn).</summary>
        public string? Phone { get; set; }

        /// <summary>Địa chỉ email liên hệ, tối đa 100 ký tự (tuỳ chọn).</summary>
        public string? Email { get; set; }

        /// <summary>Địa chỉ giao hàng / liên hệ, tối đa 250 ký tự (tuỳ chọn).</summary>
        public string? Address { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>CustomerGroups</c>.</summary>
        public Guid CustomerGroupId { get; set; }

        /// <summary>Trạng thái: <c>true</c> = đang hoạt động, <c>false</c> = đã ẩn/ngừng giao dịch.</summary>
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

        // Relationships
        /// <summary>Nhóm khách hàng mà khách hàng này thuộc về (navigation property).</summary>
        public virtual CustomerGroup CustomerGroup { get; set; } = null!;
    }
}
