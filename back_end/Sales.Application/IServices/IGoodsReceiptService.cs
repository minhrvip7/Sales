using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Common;
using Sales.Application.DTOs.Inventory.GoodsReceipt;

namespace Sales.Application.IServices
{
    public interface IGoodsReceiptService
    {
        Task<PagedResponse<GoodsReceiptSummaryDto>> GetPagedAsync(PagedRequest request);
        Task<GoodsReceiptDto> GetByIdAsync(Guid id);
        Task<GoodsReceiptDto> CreateAsync(CreateGoodsReceiptDto dto, Guid currentUserId);
        Task<GoodsReceiptDto> UpdateAsync(Guid id, UpdateGoodsReceiptDto dto, Guid currentUserId);
        Task<GoodsReceiptDto> CompleteAsync(Guid id, Guid currentUserId);
        Task<ResolveBarcodeDto> ResolveBarcodeAsync(string barcode);
    }
}
