using System;
using Sales.Domain.Enums;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO hiển thị tóm tắt Phiếu Nhập Kho (dùng cho list/phân trang).
    /// </summary>
    public class GoodsReceiptSummaryDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public GoodsReceiptType Type { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }
        public GoodsReceiptStatus Status { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
