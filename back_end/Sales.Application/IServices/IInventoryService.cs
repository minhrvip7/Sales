using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Inventory;

namespace Sales.Application.IServices
{
    /// <summary>
    /// Service xử lý nghiệp vụ tồn kho.
    /// </summary>
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryBalanceDto>> GetBalancesAsync();
        Task<IEnumerable<InventoryTransactionDto>> GetTransactionsAsync(Guid productId);
        Task ProcessInboundAsync(ProcessInboundDto request, Guid userId);
        Task ProcessOutboundAsync(ProcessOutboundDto request, Guid userId);
        Task AllocateInventoryAsync(Guid productId, int qty, Guid userId);
    }
}
