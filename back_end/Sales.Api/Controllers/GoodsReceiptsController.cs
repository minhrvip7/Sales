using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs.Common;
using Sales.Application.DTOs.Inventory.GoodsReceipt;
using Sales.Application.IServices;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Quản lý luồng Nhập kho (Goods Receipt): tạo phiếu, sửa nháp, phân giải mã vạch và chốt phiếu.
    /// </summary>
    [Route("api/goods-receipts")]
    [Tags("Goods-Receipts")]
    public class GoodsReceiptsController : BaseController
    {
        private readonly IGoodsReceiptService _service;

        public GoodsReceiptsController(IGoodsReceiptService service)
        {
            _service = service;
        }

        /// <summary>Lấy danh sách phiếu nhập kho (có phân trang).</summary>
        /// <remarks>Lấy danh sách các phiếu nhập kho đã tạo, có hỗ trợ phân trang.</remarks>
        /// <response code="200">Lấy danh sách thành công.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return OkResponse(result, "Lấy danh sách thành công.");
        }

        /// <summary>Lấy chi tiết một phiếu nhập kho.</summary>
        /// <remarks>Trả về thông tin chi tiết của một phiếu nhập kho theo ID bao gồm các dòng sản phẩm bên trong.</remarks>
        /// <param name="id">ID của phiếu nhập kho cần lấy.</param>
        /// <response code="200">Lấy thông tin thành công.</response>
        /// <response code="404">Không tìm thấy phiếu nhập kho.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return OkResponse(result, "Lấy thông tin thành công.");
        }

        /// <summary>Tạo mới phiếu nhập kho (Lưu nháp).</summary>
        /// <remarks>Tạo một phiếu nhập kho mới ở trạng thái Draft. Chưa cộng tồn kho hay tính giá vốn.</remarks>
        /// <response code="200">Tạo phiếu nhập kho thành công.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateGoodsReceiptDto dto)
        {
            var currentUserId = Guid.Empty; // Mock user
            var result = await _service.CreateAsync(dto, currentUserId);
            return OkResponse(result, "Tạo phiếu nhập kho thành công.");
        }

        /// <summary>Cập nhật phiếu nhập kho đang ở trạng thái nháp.</summary>
        /// <remarks>Chỉ cho phép cập nhật thông tin phiếu khi phiếu đang ở trạng thái Draft.</remarks>
        /// <param name="id">ID của phiếu nhập kho cần cập nhật.</param>
        /// <param name="dto">Dữ liệu phiếu nhập kho cần cập nhật.</param>
        /// <response code="200">Cập nhật phiếu nhập kho thành công.</response>
        /// <response code="400">Trạng thái phiếu không hợp lệ để sửa.</response>
        /// <response code="404">Không tìm thấy phiếu nhập kho.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoodsReceiptDto dto)
        {
            var currentUserId = Guid.Empty; // Mock user
            var result = await _service.UpdateAsync(id, dto, currentUserId);
            return OkResponse(result, "Cập nhật phiếu nhập kho thành công.");
        }

        /// <summary>Chốt phiếu nhập kho (Complete).</summary>
        /// <remarks>Hoàn tất phiếu nhập kho, thực hiện cộng tồn kho vào thẻ kho và chuyển trạng thái phiếu sang Completed.</remarks>
        /// <param name="id">ID của phiếu nhập kho cần chốt.</param>
        /// <response code="200">Chốt phiếu nhập kho thành công.</response>
        /// <response code="400">Trạng thái phiếu không hợp lệ để chốt.</response>
        /// <response code="404">Không tìm thấy phiếu nhập kho.</response>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Complete(Guid id)
        {
            var currentUserId = Guid.Empty; // Mock user
            var result = await _service.CompleteAsync(id, currentUserId);
            return OkResponse(result, "Chốt phiếu nhập kho thành công.");
        }

        /// <summary>Phân giải mã vạch sản phẩm.</summary>
        /// <remarks>Kiểm tra mã vạch quét được thuộc về Sản phẩm nào, Đơn vị tính nào (Cơ bản hay Quy đổi) và Tỷ lệ quy đổi là bao nhiêu.</remarks>
        /// <param name="barcode">Chuỗi mã vạch cần phân giải.</param>
        /// <response code="200">Phân giải mã vạch thành công.</response>
        /// <response code="404">Không tìm thấy sản phẩm tương ứng với mã vạch.</response>
        [HttpGet("resolve-barcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResolveBarcode([FromQuery] string barcode)
        {
            var result = await _service.ResolveBarcodeAsync(barcode);
            return OkResponse(result, "Phân giải mã vạch thành công.");
        }
    }
}
