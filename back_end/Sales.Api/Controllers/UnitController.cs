using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Unit;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitService.GetAllUnitsAsync();
            return OkResponse(result, "Lấy danh sách đơn vị tính thành công.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _unitService.GetUnitByIdAsync(id);
            return OkResponse(result, "Lấy thông tin đơn vị tính thành công.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUnitDto dto)
        {
            var result = await _unitService.CreateUnitAsync(dto);
            return OkResponse(result, "Tạo đơn vị tính thành công.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUnitDto dto)
        {
            await _unitService.UpdateUnitAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin đơn vị tính thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitService.DeleteUnitAsync(id);
            return SuccessResponse("Xóa đơn vị tính thành công.");
        }
    }
}
