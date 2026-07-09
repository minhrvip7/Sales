using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs.Inventory;
using Sales.Application.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace Sales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Lấy danh sách số dư tồn kho
        /// </summary>
        /// <returns>Danh sách các số dư tồn kho của tất cả sản phẩm.</returns>
        [HttpGet("balances")]
        [SwaggerOperation(Summary = "Lấy danh sách số dư tồn kho", Description = "Trả về danh sách tồn kho On-hand, Allocated, Available của các sản phẩm.")]
        [SwaggerResponse(200, "Lấy dữ liệu thành công.", typeof(IEnumerable<InventoryBalanceDto>))]
        public async Task<IActionResult> GetBalances()
        {
            var balances = await _inventoryService.GetBalancesAsync();
            return Ok(balances);
        }

        /// <summary>
        /// Lấy lịch sử giao dịch (thẻ kho) của một sản phẩm
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần xem thẻ kho.</param>
        /// <returns>Danh sách các giao dịch liên quan đến sản phẩm.</returns>
        [HttpGet("transactions/{productId}")]
        [SwaggerOperation(Summary = "Lấy thẻ kho sản phẩm", Description = "Trả về danh sách chi tiết các lần nhập, xuất, điều chỉnh của một sản phẩm.")]
        [SwaggerResponse(200, "Lấy dữ liệu thành công.", typeof(IEnumerable<InventoryTransactionDto>))]
        public async Task<IActionResult> GetTransactions(Guid productId)
        {
            var transactions = await _inventoryService.GetTransactionsAsync(productId);
            return Ok(transactions);
        }
        [HttpPost("test-inbound")]
        [SwaggerOperation(Summary = "Test Nhập Kho", Description = "API Debug để giả lập hành động nhập kho.")]
        public async Task<IActionResult> TestInbound([FromBody] ProcessInboundDto request)
        {
            await _inventoryService.ProcessInboundAsync(request, Guid.Empty);
            return Ok(new { message = "Nhập kho thành công" });
        }

        [HttpPost("test-outbound")]
        [SwaggerOperation(Summary = "Test Xuất Kho", Description = "API Debug để giả lập hành động xuất kho.")]
        public async Task<IActionResult> TestOutbound([FromBody] ProcessOutboundDto request)
        {
            await _inventoryService.ProcessOutboundAsync(request, Guid.Empty);
            return Ok(new { message = "Xuất kho thành công" });
        }
    }
}
