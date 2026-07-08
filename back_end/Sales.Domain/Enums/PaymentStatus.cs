namespace Sales.Domain.Enums
{
    /// <summary>
    /// Trạng thái thanh toán của đơn hàng.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>Chưa thanh toán – đơn hàng chưa nhận được khoản tiền nào.</summary>
        Unpaid = 0,

        /// <summary>Thanh toán một phần – đã nhận một phần tiền, còn lại chưa thanh toán.</summary>
        PartiallyPaid = 1,

        /// <summary>Đã thanh toán đủ – đơn hàng đã được thanh toán toàn bộ.</summary>
        FullyPaid = 2
    }
}
