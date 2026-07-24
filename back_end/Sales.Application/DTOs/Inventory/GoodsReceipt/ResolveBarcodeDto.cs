using System;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO chứa thông tin trả về khi phân giải mã vạch.
    /// </summary>
    public class ResolveBarcodeDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid UoMId { get; set; }
        public string UoMName { get; set; } = string.Empty;
        public decimal ConversionRate { get; set; }
        
        /// <summary>
        /// True nếu mã vạch thuộc đơn vị cơ bản (BaseUnit), False nếu thuộc đơn vị phụ (AlternativeUnit).
        /// </summary>
        public bool IsBaseUnit { get; set; }
    }
}
