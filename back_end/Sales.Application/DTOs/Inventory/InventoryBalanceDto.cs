using System;

namespace Sales.Application.DTOs.Inventory
{
    /// <summary>
    /// Thông tin số dư tồn kho của sản phẩm.
    /// </summary>
    public class InventoryBalanceDto
    {
        /// <summary>ID của bản ghi số dư.</summary>
        public Guid Id { get; set; }

        /// <summary>ID của sản phẩm.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Mã sản phẩm.</summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>Tên sản phẩm.</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>Số lượng vật lý đang có trong kho.</summary>
        public int OnHandQty { get; set; }

        /// <summary>Số lượng hàng đã giữ chỗ (Sales Order Confirmed).</summary>
        public int AllocatedQty { get; set; }

        /// <summary>Số lượng khả dụng dùng để bán (Available = OnHandQty - AllocatedQty).</summary>
        public int AvailableQty { get; set; }
    }
}
