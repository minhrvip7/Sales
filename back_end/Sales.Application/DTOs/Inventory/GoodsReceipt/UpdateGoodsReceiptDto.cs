using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO cập nhật Phiếu Nhập Kho (Chỉ áp dụng khi phiếu ở trạng thái Draft).
    /// </summary>
    public class UpdateGoodsReceiptDto
    {
        public Guid? ReferenceId { get; set; }

        [MaxLength(50, ErrorMessage = "Mã tham chiếu tối đa 50 ký tự.")]
        public string ReferenceCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kho nhập là bắt buộc.")]
        public Guid WarehouseId { get; set; }

        [Required(ErrorMessage = "Ngày nhập là bắt buộc.")]
        public DateTime ReceiptDate { get; set; }

        [MaxLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cần ít nhất 1 dòng sản phẩm.")]
        [MinLength(1, ErrorMessage = "Cần ít nhất 1 dòng sản phẩm.")]
        public List<UpdateGoodsReceiptLineDto> Lines { get; set; } = new();
    }

    /// <summary>
    /// DTO cập nhật Dòng Phiếu Nhập Kho.
    /// </summary>
    public class UpdateGoodsReceiptLineDto
    {
        public Guid? Id { get; set; } // Null nếu là dòng mới thêm

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
