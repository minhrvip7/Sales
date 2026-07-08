using System;
using System.Threading.Tasks;
using Evo.Sales.Application.DTOs.Product;
using Evo.Sales.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Evo.Sales.Api.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllProductsAsync();
            return OkResponse(result, "Lấy danh sách sản phẩm thành công.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return OkResponse(result, "Lấy thông tin sản phẩm thành công.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            return OkResponse(result, "Tạo sản phẩm thành công.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto dto)
        {
            await _productService.UpdateProductAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin sản phẩm thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return SuccessResponse("Xóa sản phẩm thành công.");
        }
    }
}
