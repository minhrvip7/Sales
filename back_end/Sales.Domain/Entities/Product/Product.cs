using System;
using System.Collections.Generic;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Product
{
    /// <summary>
    /// Sản phẩm hàng hóa trong hệ thống bán hàng. Mỗi sản phẩm thuộc một nhóm hàng
    /// và có một đơn vị tính cơ bản; có thể cấu hình thêm các đơn vị tính phụ (quy đổi).
    /// </summary>
    public class Product : ISoftDelete
    {
        /// <summary>Khóa chính – định danh duy nhất của sản phẩm (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Tên sản phẩm, tối đa 150 ký tự, bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Mã sản phẩm nội bộ, duy nhất, tối đa 50 ký tự.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Mã vạch của đơn vị cơ bản. Tối đa 50 ký tự, tuỳ chọn.</summary>
        public string? Barcode { get; set; }

        /// <summary>Mô tả chi tiết hoặc ghi chú thêm về sản phẩm (tuỳ chọn).</summary>
        public string? Description { get; set; }

        /// <summary>Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ), độ chính xác 2 chữ số thập phân.</summary>
        public decimal Price { get; set; } = 0;

        /// <summary>Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ), độ chính xác 2 chữ số thập phân.</summary>
        public decimal Cost { get; set; } = 0;

        /// <summary>Tồn kho hiện tại tính theo đơn vị cơ bản. Cập nhật tự động khi có giao dịch.</summary>
        public int StockQuantity { get; set; } = 0;

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Categories</c> – nhóm hàng của sản phẩm.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Khóa ngoại tham chiếu tới bảng <c>Units</c> – đơn vị tính cơ bản.</summary>
        public Guid BaseUnitId { get; set; }

        /// <summary>Trạng thái sản phẩm: <c>true</c> = đang hoạt động, <c>false</c> = ngừng kinh doanh.</summary>
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
        /// <summary>
        /// Cờ xóa mềm: <c>true</c> = đã bị xóa mềm, không hiển thị trong danh sách sản phẩm thông thường.
        /// Dữ liệu vẫn được giữ lại để hiển thị trong lịch sử đơn hàng và báo cáo.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // Relationships
        /// <summary>Nhóm hàng mà sản phẩm thuộc về (navigation property).</summary>
        public virtual Category Category { get; set; } = null!;

        /// <summary>Đơn vị tính cơ bản của sản phẩm (navigation property).</summary>
        public virtual Unit BaseUnit { get; set; } = null!;

        /// <summary>Danh sách quy đổi đơn vị tính phụ của sản phẩm.</summary>
        public virtual ICollection<ProductUnitConversion> Conversions { get; set; } = new List<ProductUnitConversion>();
    }
}
