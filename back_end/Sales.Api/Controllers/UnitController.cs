using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Unit;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>Lấy danh sách tất cả đơn vị tính.</summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lấy danh sách đơn vị tính",
            Description = "Trả về toàn bộ đơn vị tính trong hệ thống. Ví dụ: Chai, Hộp, Thùng, Kg, Lít."
        )]
        [SwaggerResponse(200, "Lấy danh sách đơn vị tính thành công.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitService.GetAllUnitsAsync();
            return OkResponse(result, "Lấy danh sách đơn vị tính thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một đơn vị tính theo ID.</summary>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Lấy chi tiết đơn vị tính",
            Description = "Trả về thông tin đầy đủ của đơn vị tính theo ID bao gồm tên, mã viết tắt, mô tả và trạng thái."
        )]
        [SwaggerResponse(200, "Lấy thông tin đơn vị tính thành công.")]
        [SwaggerResponse(404, "Không tìm thấy đơn vị tính với ID đã cho.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _unitService.GetUnitByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn vị tính thành công.");
        }

        /// <summary>Tạo mới một đơn vị tính.</summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo đơn vị tính mới",
            Description = "Thêm một đơn vị tính mới vào hệ thống. Mã đơn vị tính (Code) phải duy nhất trong toàn hệ thống."
        )]
        [SwaggerResponse(200, "Tạo đơn vị tính thành công. Trả về thông tin đơn vị tính vừa tạo.")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ hoặc mã đơn vị tính đã tồn tại.")]
        public async Task<IActionResult> Create([FromBody] CreateUnitDto dto)
        {
            var result = await _unitService.CreateUnitAsync(dto);
            return OkResponse(result, "Tạo đơn vị tính thành công.");
        }

        /// <summary>Cập nhật thông tin đơn vị tính.</summary>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Cập nhật đơn vị tính",
            Description = "Cập nhật tên, mã và mô tả của đơn vị tính theo ID."
        )]
        [SwaggerResponse(200, "Cập nhật thông tin đơn vị tính thành công.")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ.")]
        [SwaggerResponse(404, "Không tìm thấy đơn vị tính với ID đã cho.")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUnitDto dto)
        {
            await _unitService.UpdateUnitAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin đơn vị tính thành công.");
        }

        /// <summary>Xóa đơn vị tính.</summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Xóa đơn vị tính",
            Description = "Xóa đơn vị tính khỏi hệ thống theo ID. Không thể xóa đơn vị tính đang được sử dụng bởi sản phẩm hoặc đơn hàng."
        )]
        [SwaggerResponse(200, "Xóa đơn vị tính thành công.")]
        [SwaggerResponse(400, "Không thể xóa đơn vị tính đang được sử dụng.")]
        [SwaggerResponse(404, "Không tìm thấy đơn vị tính với ID đã cho.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitService.DeleteUnitAsync(id);
            return SuccessResponse("Xóa đơn vị tính thành công.");
        }
    }
}
