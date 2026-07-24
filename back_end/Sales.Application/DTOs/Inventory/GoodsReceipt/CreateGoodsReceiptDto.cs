using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sales.Domain.Enums;

namespace Sales.Application.DTOs.Inventory.GoodsReceipt
{
    /// <summary>
    /// DTO tạo mới Phiếu Nhập Kho.
    /// </summary>
    public class CreateGoodsReceiptDto
    {
        [Required(ErrorMessage = "Loại phiếu là bắt buộc.")]
        public GoodsReceiptType Type { get; set; }

        public Guid? ReferenceId { get; set; }

        [MaxLength(50, ErrorMessage = "Mã tham chiếu tối đa 50 ký tự.")]
        public string ReferenceCode { get; set; } = string.Empty;

        public Guid? SupplierId { get; set; }

        [Required(ErrorMessage = "Kho nhập là bắt buộc.")]
        public Guid WarehouseId { get; set; }

        [Required(ErrorMessage = "Ngày nhập là bắt buộc.")]
        public DateTime ReceiptDate { get; set; }

        [MaxLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cần ít nhất 1 dòng sản phẩm.")]
        [MinLength(1, ErrorMessage = "Cần ít nhất 1 dòng sản phẩm.")]
        public List<CreateGoodsReceiptLineDto> Lines { get; set; } = new();
    }
}
