namespace Sales.Domain.Enums
{
    /// <summary>
    /// Trạng thái của phiếu nhập kho.
    /// </summary>
    public enum GoodsReceiptStatus
    {
        /// <summary>Nháp - Phiếu đang trong quá trình kiểm đếm, có thể chỉnh sửa.</summary>
        Draft = 0,

        /// <summary>Hoàn tất - Phiếu đã chốt số liệu, tính giá vốn và cộng tồn kho. Không thể sửa xóa.</summary>
        Completed = 1,

        /// <summary>Đã hủy - Phiếu bị hủy, không còn giá trị.</summary>
        Cancelled = 2
    }
}
