using System;
using System.Threading.Tasks;
using Evo.Sales.Application.DTOs.Order;
using Evo.Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Evo.Sales.Api.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return OkResponse(result, "Lấy danh sách đơn hàng thành công.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn hàng thành công.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(dto);
            return OkResponse(result, "Tạo đơn hàng thành công.");
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return SuccessResponse("Hủy đơn hàng thành công.");
        }
    }
}
