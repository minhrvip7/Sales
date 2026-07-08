using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Category;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>Lấy danh sách tất cả nhóm hàng.</summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lấy danh sách nhóm hàng",
            Description = "Trả về toàn bộ nhóm hàng trong hệ thống, bao gồm cả nhóm đang hoạt động và đã ẩn."
        )]
        [SwaggerResponse(200, "Lấy danh sách nhóm hàng thành công.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return OkResponse(result, "Lấy danh sách nhóm sản phẩm thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một nhóm hàng theo ID.</summary>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Lấy chi tiết nhóm hàng",
            Description = "Trả về thông tin đầy đủ của nhóm hàng theo ID bao gồm tên, mã, mô tả và trạng thái."
        )]
        [SwaggerResponse(200, "Lấy thông tin nhóm hàng thành công.")]
        [SwaggerResponse(404, "Không tìm thấy nhóm hàng với ID đã cho.")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return OkResponse(result, "Lấy thông tin nhóm sản phẩm thành công.");
        }

        /// <summary>Tạo mới một nhóm hàng.</summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo nhóm hàng mới",
            Description = "Thêm một nhóm hàng mới vào hệ thống. Mã nhóm hàng (Code) phải duy nhất trong toàn hệ thống."
        )]
        [SwaggerResponse(200, "Tạo nhóm hàng thành công. Trả về thông tin nhóm hàng vừa tạo.")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ hoặc mã nhóm hàng đã tồn tại.")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            return OkResponse(result, "Tạo nhóm sản phẩm thành công.");
        }

        /// <summary>Cập nhật thông tin nhóm hàng.</summary>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Cập nhật nhóm hàng",
            Description = "Cập nhật tên, mã và mô tả của nhóm hàng theo ID."
        )]
        [SwaggerResponse(200, "Cập nhật thông tin nhóm hàng thành công.")]
        [SwaggerResponse(400, "Dữ liệu đầu vào không hợp lệ.")]
        [SwaggerResponse(404, "Không tìm thấy nhóm hàng với ID đã cho.")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto dto)
        {
            await _categoryService.UpdateCategoryAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin nhóm sản phẩm thành công.");
        }

        /// <summary>Xóa nhóm hàng.</summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Xóa nhóm hàng",
            Description = "Xóa nhóm hàng khỏi hệ thống theo ID. Không thể xóa nhóm hàng đang có sản phẩm. Hãy chuyển toàn bộ sản phẩm sang nhóm khác trước khi xóa."
        )]
        [SwaggerResponse(200, "Xóa nhóm hàng thành công.")]
        [SwaggerResponse(400, "Không thể xóa nhóm hàng đang có sản phẩm.")]
        [SwaggerResponse(404, "Không tìm thấy nhóm hàng với ID đã cho.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return SuccessResponse("Xóa nhóm sản phẩm thành công.");
        }
    }
}
