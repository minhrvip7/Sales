using System;

namespace Sales.Application.DTOs.Unit
{
    /// <summary>Thông tin đơn vị tính trả về cho client.</summary>
    public class UnitDto
    {
        /// <summary>Định danh duy nhất của đơn vị tính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên đơn vị tính. Ví dụ: "Chai", "Thùng", "Kg".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã viết tắt, duy nhất trong hệ thống. Ví dụ: "CHI", "THG".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mô tả thêm về đơn vị tính (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Trạng thái: <c>true</c> = đang sử dụng, <c>false</c> = đã ẩn.</summary>
        public bool Status { get; set; }

        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; }
    }
}
