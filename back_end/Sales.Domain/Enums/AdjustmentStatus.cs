namespace Sales.Domain.Enums
{
    /// <summary>
    /// Trạng thái của phiếu kiểm kê / điều chỉnh tồn kho.
    /// </summary>
    public enum AdjustmentStatus
    {
        /// <summary>Bản nháp, đang kiểm đếm, chưa ảnh hưởng tồn kho.</summary>
        Draft = 0,
        
        /// <summary>Hoàn tất, đã chốt số liệu và tự động sinh các giao dịch điều chỉnh (thừa/thiếu).</summary>
        Completed = 1,
        
        /// <summary>Đã bị hủy bỏ.</summary>
        Cancelled = 2
    }
}
