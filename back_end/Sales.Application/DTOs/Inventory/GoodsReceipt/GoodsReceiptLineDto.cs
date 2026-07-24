using System;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO hiển thị dòng chi tiết trong Phiếu Nhập Kho.
    /// </summary>
    public class GoodsReceiptLineDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid UoMId { get; set; }
        public string UoMName { get; set; } = string.Empty;
        public decimal ConversionRate { get; set; }
        public decimal ExpectedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
