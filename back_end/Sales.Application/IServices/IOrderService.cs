using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Order;

namespace Sales.Application.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<bool> CancelOrderAsync(Guid id);
    }
}

