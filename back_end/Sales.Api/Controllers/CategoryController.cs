using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Category;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Quản lý nhóm hàng (danh mục sản phẩm): thêm, sửa, xóa, tra cứu nhóm hàng.
    /// </summary>
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>Lấy danh sách nhóm hàng có phân trang.</summary>
        /// <remarks>Lấy danh sách nhóm hàng có hỗ trợ phân trang và tìm kiếm theo từ khóa.</remarks>
        /// <response code="200">Lấy danh sách nhóm hàng thành công.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await _categoryService.GetPagedCategoriesAsync(request);
            return OkResponse(result, "Lấy danh sách nhóm sản phẩm thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một nhóm hàng theo ID.</summary>
        /// <remarks>Trả về thông tin đầy đủ của nhóm hàng theo ID bao gồm tên, mã, mô tả và trạng thái.</remarks>
        /// <param name="id">ID của nhóm hàng cần lấy.</param>
        /// <response code="200">Lấy thông tin nhóm hàng thành công.</response>
        /// <response code="404">Không tìm thấy nhóm hàng với ID đã cho.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return OkResponse(result, "Lấy thông tin nhóm sản phẩm thành công.");
        }

        /// <summary>Tạo mới một nhóm hàng.</summary>
        /// <remarks>Thêm một nhóm hàng mới vào hệ thống. Mã nhóm hàng (Code) phải duy nhất trong toàn hệ thống.</remarks>
        /// <response code="200">Tạo nhóm hàng thành công. Trả về thông tin nhóm hàng vừa tạo.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc mã nhóm hàng đã tồn tại.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            return OkResponse(result, "Tạo nhóm sản phẩm thành công.");
        }

        /// <summary>Cập nhật thông tin nhóm hàng.</summary>
        /// <remarks>Cập nhật tên, mã và mô tả của nhóm hàng theo ID.</remarks>
        /// <param name="id">ID của nhóm hàng cần cập nhật.</param>
        /// <param name="dto">Dữ liệu nhóm hàng cần cập nhật.</param>
        /// <response code="200">Cập nhật thông tin nhóm hàng thành công.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="404">Không tìm thấy nhóm hàng với ID đã cho.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto dto)
        {
            await _categoryService.UpdateCategoryAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin nhóm sản phẩm thành công.");
        }

        /// <summary>Xóa nhóm hàng.</summary>
        /// <remarks>Xóa nhóm hàng khỏi hệ thống theo ID. Không thể xóa nhóm hàng đang có sản phẩm. Hãy chuyển toàn bộ sản phẩm sang nhóm khác trước khi xóa.</remarks>
        /// <param name="id">ID của nhóm hàng cần xóa.</param>
        /// <response code="200">Xóa nhóm hàng thành công.</response>
        /// <response code="400">Không thể xóa nhóm hàng đang có sản phẩm.</response>
        /// <response code="404">Không tìm thấy nhóm hàng với ID đã cho.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return SuccessResponse("Xóa nhóm sản phẩm thành công.");
        }
    }
}
