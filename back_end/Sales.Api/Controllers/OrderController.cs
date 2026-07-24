using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Order;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>Lấy danh sách đơn hàng có phân trang.</summary>
        /// <remarks>Trả về danh sách đơn hàng có hỗ trợ phân trang và tìm kiếm theo từ khóa.</remarks>
        /// <response code="200">Lấy danh sách đơn hàng thành công.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await _orderService.GetPagedOrdersAsync(request);
            return OkResponse(result, "Lấy danh sách đơn hàng thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một đơn hàng theo ID.</summary>
        /// <remarks>Trả về thông tin đầy đủ của đơn hàng theo ID, bao gồm danh sách dòng chi tiết mặt hàng, trạng thái xử lý và trạng thái thanh toán.</remarks>
        /// <param name="id">ID của đơn hàng cần lấy.</param>
        /// <response code="200">Lấy thông tin đơn hàng thành công.</response>
        /// <response code="404">Không tìm thấy đơn hàng với ID đã cho.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn hàng thành công.");
        }

        /// <summary>Tạo mới một đơn hàng.</summary>
        /// <remarks>Tạo một đơn hàng mới với trạng thái Draft (Nháp). Hệ thống tự động tính toán SubTotal, DiscountAmount và TotalAmount từ các dòng chi tiết. Mã đơn hàng (OrderNumber) được tự sinh.</remarks>
        /// <response code="200">Tạo đơn hàng thành công. Trả về thông tin đơn hàng vừa tạo.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ (thiếu trường bắt buộc, sản phẩm/khách hàng không tồn tại).</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(dto);
            return OkResponse(result, "Tạo đơn hàng thành công.");
        }

        /// <summary>Hủy một đơn hàng.</summary>
        /// <remarks>Chuyển trạng thái đơn hàng sang Cancelled (Đã hủy). Chỉ cho phép hủy đơn ở trạng thái Draft hoặc Confirmed. Đơn đã Completed không thể hủy.</remarks>
        /// <param name="id">ID của đơn hàng cần hủy.</param>
        /// <response code="200">Hủy đơn hàng thành công.</response>
        /// <response code="400">Không thể hủy đơn hàng ở trạng thái hiện tại.</response>
        /// <response code="404">Không tìm thấy đơn hàng với ID đã cho.</response>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return SuccessResponse("Hủy đơn hàng thành công.");
        }
    }
}
