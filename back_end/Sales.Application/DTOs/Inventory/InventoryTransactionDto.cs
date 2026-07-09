using System;
using Sales.Domain.Enums;

namespace Sales.Application.DTOs.Inventory
{
    /// <summary>
    /// Thông tin chi tiết lịch sử giao dịch tồn kho.
    /// </summary>
    public class InventoryTransactionDto
    {
        /// <summary>ID của giao dịch.</summary>
        public Guid Id { get; set; }

        /// <summary>ID sản phẩm.</summary>
        public Guid ProductId { get; set; }

        /// <summary>Tên sản phẩm.</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>Loại giao dịch.</summary>
        public TransactionType Type { get; set; }

        /// <summary>Mã tham chiếu (PO, SO, v.v.).</summary>
        public string ReferenceNumber { get; set; } = string.Empty;

        /// <summary>Số lượng giao dịch theo đơn vị thao tác.</summary>
        public int TransactedQty { get; set; }

        /// <summary>Tên đơn vị tính lúc thao tác.</summary>
        public string TransactedUomName { get; set; } = string.Empty;

        /// <summary>Số lượng quy đổi cộng/trừ vào tồn kho (theo đơn vị cơ bản).</summary>
        public int BaseQty { get; set; }

        /// <summary>Lý do giao dịch.</summary>
        public string? Reason { get; set; }

        /// <summary>Ngày giờ giao dịch.</summary>
        public DateTime CreatedDate { get; set; }
    }
}
