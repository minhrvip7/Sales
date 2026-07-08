using System;

namespace Sales.Application.DTOs.Unit
{
    /// <summary>Dữ liệu đầu vào để tạo mới hoặc cập nhật đơn vị tính.</summary>
    public class CreateUnitDto
    {
        /// <summary>Tên đơn vị tính, tối đa 150 ký tự. Bắt buộc. Ví dụ: "Chai", "Thùng".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã viết tắt, tối đa 50 ký tự, phải duy nhất. Bắt buộc. Ví dụ: "CHI", "THG".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về đơn vị tính (tuỳ chọn).</summary>
        public string? Description { get; set; }
    }
}
