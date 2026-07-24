using System;
using System.Threading.Tasks;
using Sales.Application.DTOs.Product;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Quản lý sản phẩm hàng hóa: thêm, sửa, xóa, tra cứu sản phẩm và cấu hình đơn vị tính phụ.
    /// </summary>
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>Lấy danh sách sản phẩm có phân trang.</summary>
        /// <remarks>Lấy danh sách sản phẩm có hỗ trợ phân trang và tìm kiếm theo từ khóa.</remarks>
        /// <response code="200">Lấy danh sách sản phẩm thành công.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            var result = await _productService.GetPagedProductsAsync(request);
            return OkResponse(result, "Lấy danh sách sản phẩm thành công.");
        }

        /// <summary>Lấy thông tin chi tiết một sản phẩm theo ID.</summary>
        /// <remarks>Trả về thông tin đầy đủ của sản phẩm theo ID, bao gồm giá bán, giá vốn, tồn kho, nhóm hàng, đơn vị tính cơ bản và toàn bộ cấu hình quy đổi đơn vị tính phụ.</remarks>
        /// <param name="id">ID của sản phẩm cần lấy.</param>
        /// <response code="200">Lấy thông tin sản phẩm thành công.</response>
        /// <response code="404">Không tìm thấy sản phẩm với ID đã cho.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return OkResponse(result, "Lấy thông tin sản phẩm thành công.");
        }

        /// <summary>Tạo mới một sản phẩm.</summary>
        /// <remarks>Thêm một sản phẩm mới vào hệ thống. Mã sản phẩm (Code) phải duy nhất. Có thể kèm theo cấu hình đơn vị tính phụ trong field Conversions.</remarks>
        /// <response code="200">Tạo sản phẩm thành công. Trả về thông tin sản phẩm vừa tạo.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc mã sản phẩm đã tồn tại.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            return OkResponse(result, "Tạo sản phẩm thành công.");
        }

        /// <summary>Cập nhật thông tin sản phẩm.</summary>
        /// <remarks>Cập nhật thông tin sản phẩm theo ID. Toàn bộ dữ liệu sản phẩm sẽ được ghi đè theo body request, bao gồm cả danh sách quy đổi đơn vị tính phụ.</remarks>
        /// <param name="id">ID của sản phẩm cần cập nhật.</param>
        /// <param name="dto">Dữ liệu sản phẩm cần cập nhật.</param>
        /// <response code="200">Cập nhật thông tin sản phẩm thành công.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="404">Không tìm thấy sản phẩm với ID đã cho.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto dto)
        {
            await _productService.UpdateProductAsync(id, dto);
            return SuccessResponse("Cập nhật thông tin sản phẩm thành công.");
        }

        /// <summary>Xóa sản phẩm.</summary>
        /// <remarks>Xóa sản phẩm khỏi hệ thống theo ID. Không thể xóa sản phẩm đã xuất hiện trong đơn hàng (HasTransactions = true). Trong trường hợp đó, hãy đặt Status = false để ngừng kinh doanh thay vì xóa.</remarks>
        /// <param name="id">ID của sản phẩm cần xóa.</param>
        /// <response code="200">Xóa sản phẩm thành công.</response>
        /// <response code="400">Không thể xóa sản phẩm đã có giao dịch.</response>
        /// <response code="404">Không tìm thấy sản phẩm với ID đã cho.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return SuccessResponse("Xóa sản phẩm thành công.");
        }
    }
}
