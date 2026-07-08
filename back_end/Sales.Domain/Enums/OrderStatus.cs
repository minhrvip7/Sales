namespace Sales.Domain.Enums
{
    /// <summary>
    /// Trạng thái của đơn hàng trong quy trình xử lý.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>Nháp – đơn hàng mới tạo, chưa xác nhận.</summary>
        Draft = 0,

        /// <summary>Đã xác nhận – đơn hàng đã được duyệt, chờ giao.</summary>
        Confirmed = 1,

        /// <summary>Hoàn thành – đơn hàng đã giao thành công.</summary>
        Completed = 2,

        /// <summary>Đã hủy – đơn hàng bị hủy, không xử lý nữa.</summary>
        Cancelled = 3
    }
}
