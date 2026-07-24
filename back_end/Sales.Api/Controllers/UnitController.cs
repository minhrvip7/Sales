using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Unit;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Quản lý đơn vị tính: thêm, sửa, xóa, tra cứu đơn vị tính dùng trong sản phẩm và giao dịch.
    /// </summary>
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        /// <summary>Lấy danh sách đơn vị tính có phân trang.</summary>
        /// <remarks>Lấy danh sách đơn vị tính có hỗ trợ phân trang và tìm kiếm theo từ khóa.</remarks>
        /// <response code="200">Lấy danh sách đơn vị tính thành công.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await _unitService.GetPagedUnitsAsync(request);
            return OkResponse(result, "Lấy danh sách đơn vị tính thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một đơn vị tính theo ID.</summary>
        /// <remarks>Trả về thông tin đầy đủ của đơn vị tính theo ID bao gồm tên, mã viết tắt, mô tả và trạng thái.</remarks>
        /// <param name="id">ID của đơn vị tính cần lấy.</param>
        /// <response code="200">Lấy thông tin đơn vị tính thành công.</response>
        /// <response code="404">Không tìm thấy đơn vị tính với ID đã cho.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _unitService.GetUnitByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn vị tính thành công.");
        }

        /// <summary>Tạo mới một đơn vị tính.</summary>
        /// <remarks>Thêm một đơn vị tính mới vào hệ thống. Mã đơn vị tính (Code) phải duy nhất trong toàn hệ thống.</remarks>
        /// <response code="200">Tạo đơn vị tính thành công. Trả về thông tin đơn vị tính vừa tạo.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc mã đơn vị tính đã tồn tại.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUnitDto dto)
        {
            var result = await _unitService.CreateUnitAsync(dto);
            return OkResponse(result, "Tạo đơn vị tính thành công.");
        }

        /// <summary>Cập nhật thông tin đơn vị tính.</summary>
        /// <remarks>Cập nhật tên, mã và mô tả của đơn vị tính theo ID.</remarks>
        /// <param name="id">ID của đơn vị tính cần cập nhật.</param>
        /// <param name="dto">Dữ liệu đơn vị tính cần cập nhật.</param>
        /// <response code="200">Cập nhật thông tin đơn vị tính thành công.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="404">Không tìm thấy đơn vị tính với ID đã cho.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUnitDto dto)
        {
            await _unitService.UpdateUnitAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin đơn vị tính thành công.");
        }

        /// <summary>Xóa đơn vị tính.</summary>
        /// <remarks>Xóa đơn vị tính khỏi hệ thống theo ID. Không thể xóa đơn vị tính đang được sử dụng bởi sản phẩm hoặc đơn hàng.</remarks>
        /// <param name="id">ID của đơn vị tính cần xóa.</param>
        /// <response code="200">Xóa đơn vị tính thành công.</response>
        /// <response code="400">Không thể xóa đơn vị tính đang được sử dụng.</response>
        /// <response code="404">Không tìm thấy đơn vị tính với ID đã cho.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitService.DeleteUnitAsync(id);
            return SuccessResponse("Xóa đơn vị tính thành công.");
        }
    }
}
