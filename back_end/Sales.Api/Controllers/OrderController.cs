using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Order;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Quản lý đơn hàng bán lẻ: tạo, tra cứu, hủy đơn hàng.
    /// </summary>
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>Lấy danh sách tất cả đơn hàng.</summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lấy danh sách đơn hàng",
            Description = "Trả về toàn bộ đơn hàng trong hệ thống bao gồm thông tin khách hàng, trạng thái và tổng tiền. Sắp xếp theo ngày tạo mới nhất trước."
        )]
        [SwaggerResponse(200, "Lấy danh sách đơn hàng thành công.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return OkResponse(result, "Lấy danh sách đơn hàng thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một đơn hàng theo ID.</summary>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Lấy chi tiết đơn hàng",
            Description = "Trả về thông tin đầy đủ của đơn hàng theo ID, bao gồm danh sách dòng chi tiết mặt hàng, trạng thái xử lý và trạng thái thanh toán."
        )]
        [SwaggerResponse(200, "Lấy thông tin đơn hàng thành công.")]
        [SwaggerResponse(404, "Không tìm thấy đơn hàng với ID đã cho.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn hàng thành công.");
        }

        /// <summary>Tạo mới một đơn hàng.</summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo đơn hàng mới",
            Description = "Tạo một đơn hàng mới với trạng thái Draft (Nháp). Hệ thống tự động tính toán SubTotal, DiscountAmount và TotalAmount từ các dòng chi tiết. Mã đơn hàng (OrderNumber) được tự sinh."
        )]
        [SwaggerResponse(200, "Tạo đơn hàng thành công. Trả về thông tin đơn hàng vừa tạo.")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ (thiếu trường bắt buộc, sản phẩm/khách hàng không tồn tại).")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(dto);
            return OkResponse(result, "Tạo đơn hàng thành công.");
        }

        /// <summary>Hủy một đơn hàng.</summary>
        [HttpPost("{id}/cancel")]
        [SwaggerOperation(
            Summary = "Hủy đơn hàng",
            Description = "Chuyển trạng thái đơn hàng sang Cancelled (Đã hủy). Chỉ cho phép hủy đơn ở trạng thái Draft hoặc Confirmed. Đơn đã Completed không thể hủy."
        )]
        [SwaggerResponse(200, "Hủy đơn hàng thành công.")]
        [SwaggerResponse(400, "Không thể hủy đơn hàng ở trạng thái hiện tại.")]
        [SwaggerResponse(404, "Không tìm thấy đơn hàng với ID đã cho.")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return SuccessResponse("Hủy đơn hàng thành công.");
        }
    }
}
