using System;
using System.Collections.Generic;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Customer
{
    /// <summary>
    /// Nhóm khách hàng dùng để phân loại khách hàng theo chính sách giá, chiết khấu.
    /// </summary>
    public class CustomerGroup : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của nhóm khách hàng (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên nhóm khách hàng, tối đa 150 ký tự, bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã nhóm khách hàng nội bộ, duy nhất, tối đa 50 ký tự.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về nhóm khách hàng (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Phần trăm chiết khấu mặc định áp dụng cho nhóm khách hàng này (0–100).</summary>
        public double DiscountPercentage { get; set; } = 0;

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

        // Relationships
        /// <summary>Danh sách khách hàng thuộc nhóm này (navigation property).</summary>
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
