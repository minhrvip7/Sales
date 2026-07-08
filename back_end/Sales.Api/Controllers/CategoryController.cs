using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Category;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return OkResponse(result, "Lấy danh sách nhóm sản phẩm thành công.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return OkResponse(result, "Lấy thông tin nhóm sản phẩm thành công.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            return OkResponse(result, "Tạo nhóm sản phẩm thành công.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto dto)
        {
            await _categoryService.UpdateCategoryAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin nhóm sản phẩm thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return SuccessResponse("Xóa nhóm sản phẩm thành công.");
        }
    }
}
