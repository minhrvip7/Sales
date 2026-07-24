using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs.Inventory;
using Sales.Application.IServices;

namespace Sales.Api.Controllers
{
    /// <summary>
    /// Tra cứu tồn kho, lịch sử giao dịch thẻ kho và kiểm thử nhập/xuất kho.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>Lấy danh sách số dư tồn kho.</summary>
        /// <remarks>Trả về danh sách tồn kho On-hand, Allocated, Available của các sản phẩm.</remarks>
        /// <returns>Danh sách các số dư tồn kho của tất cả sản phẩm.</returns>
        /// <response code="200">Lấy dữ liệu thành công.</response>
        [HttpGet("balances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InventoryBalanceDto>))]
        public async Task<IActionResult> GetBalances()
        {
            var balances = await _inventoryService.GetBalancesAsync();
            return Ok(balances);
        }

        /// <summary>Lấy lịch sử giao dịch (thẻ kho) của một sản phẩm.</summary>
        /// <remarks>Trả về danh sách chi tiết các lần nhập, xuất, điều chỉnh của một sản phẩm.</remarks>
        /// <param name="productId">ID của sản phẩm cần xem thẻ kho.</param>
        /// <returns>Danh sách các giao dịch liên quan đến sản phẩm.</returns>
        /// <response code="200">Lấy dữ liệu thành công.</response>
        [HttpGet("transactions/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InventoryTransactionDto>))]
        public async Task<IActionResult> GetTransactions(Guid productId)
        {
            var transactions = await _inventoryService.GetTransactionsAsync(productId);
            return Ok(transactions);
        }

        /// <summary>Test Nhập Kho (Debug).</summary>
        /// <remarks>API Debug để giả lập hành động nhập kho.</remarks>
        /// <response code="200">Nhập kho thành công.</response>
        [HttpPost("test-inbound")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestInbound([FromBody] ProcessInboundDto request)
        {
            await _inventoryService.ProcessInboundAsync(request, Guid.Empty);
            return Ok(new { message = "Nhập kho thành công" });
        }

        /// <summary>Test Xuất Kho (Debug).</summary>
        /// <remarks>API Debug để giả lập hành động xuất kho.</remarks>
        /// <response code="200">Xuất kho thành công.</response>
        [HttpPost("test-outbound")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestOutbound([FromBody] ProcessOutboundDto request)
        {
            await _inventoryService.ProcessOutboundAsync(request, Guid.Empty);
            return Ok(new { message = "Xuất kho thành công" });
        }
    }
}
