using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Order;
using Sales.Application.DTOs.Common;

namespace Sales.Application.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<PagedResponse<OrderDto>> GetPagedOrdersAsync(PagedRequest request);
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<bool> CancelOrderAsync(Guid id);
    }
}

