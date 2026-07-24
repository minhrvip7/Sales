using System;
using System.ComponentModel.DataAnnotations;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO tạo mới Dòng Phiếu Nhập Kho.
    /// </summary>
    public class CreateGoodsReceiptLineDto
    {
        [Required(ErrorMessage = "Sản phẩm là bắt buộc.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Đơn vị tính là bắt buộc.")]
        public Guid UoMId { get; set; }

        [Required(ErrorMessage = "Tỷ lệ quy đổi là bắt buộc.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Tỷ lệ quy đổi phải lớn hơn 0.")]
        public decimal ConversionRate { get; set; }

        public decimal ExpectedQuantity { get; set; }

        [Required(ErrorMessage = "Số lượng thực tế là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng thực tế không được âm.")]
        public decimal ActualQuantity { get; set; }

        [MaxLength(250, ErrorMessage = "Ghi chú tối đa 250 ký tự.")]
        public string Notes { get; set; } = string.Empty;
    }
}
