namespace Sales.Domain.Enums
{
    /// <summary>
    /// Các loại giao dịch kho làm thay đổi số dư tồn kho.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>Nhập kho từ nhà cung cấp (Purchase Receipt).</summary>
        Inbound = 1,
        
        /// <summary>Xuất kho bán hàng (Sales Issue).</summary>
        Outbound = 2,
        
        /// <summary>Nhập kho điều chỉnh (Thừa).</summary>
        AdjustmentIn = 3,
        
        /// <summary>Xuất kho điều chỉnh (Thiếu).</summary>
        AdjustmentOut = 4,
        
        /// <summary>Các loại xuất kho khác (Hủy, tiêu hao).</summary>
        OtherIssue = 5
    }
}
