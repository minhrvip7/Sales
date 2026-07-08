using System;

namespace Sales.Application.DTOs.Category
{
    /// <summary>Thông tin nhóm hàng trả về cho client.</summary>
    public class CategoryDto
    {
        /// <summary>Định danh duy nhất của nhóm hàng (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên nhóm hàng.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã nhóm hàng nội bộ, duy nhất trong hệ thống.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về nhóm hàng (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Trạng thái: <c>true</c> = đang sử dụng, <c>false</c> = đã ẩn.</summary>
        public bool Status { get; set; }

        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; }
    }
}
