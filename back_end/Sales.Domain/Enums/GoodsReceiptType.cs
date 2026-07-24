namespace Sales.Domain.Enums
{
    /// <summary>
    /// Loại phiếu nhập kho.
    /// </summary>
    public enum GoodsReceiptType
    {
        /// <summary>Nhập mua (Purchase) - Nhập từ đơn mua hàng.</summary>
        Purchase = 1,

        /// <summary>Nhập trả hàng (Return) - Nhập hàng bị khách trả lại.</summary>
        Return = 2,

        /// <summary>Điều chuyển (Transfer) - Nhập hàng điều chuyển từ kho khác.</summary>
        Transfer = 3
    }
}
