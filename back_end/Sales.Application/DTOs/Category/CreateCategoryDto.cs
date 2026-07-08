using System;

namespace Sales.Application.DTOs.Category
{
    /// <summary>Dữ liệu đầu vào để tạo mới hoặc cập nhật nhóm hàng.</summary>
    public class CreateCategoryDto
    {
        /// <summary>Tên nhóm hàng, tối đa 150 ký tự. Bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã nhóm hàng nội bộ, tối đa 50 ký tự, phải duy nhất. Bắt buộc.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về nhóm hàng (tuỳ chọn).</summary>
        public string? Description { get; set; }
    }
}
