using System;
using System.Collections.Generic;
using Sales.Domain.Enums;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO hiển thị chi tiết Phiếu Nhập Kho.
    /// </summary>
    public class GoodsReceiptDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public GoodsReceiptType Type { get; set; }
        public Guid? ReferenceId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; }
        public Guid WarehouseId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public GoodsReceiptStatus Status { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<GoodsReceiptLineDto> Lines { get; set; } = new();
    }
}
