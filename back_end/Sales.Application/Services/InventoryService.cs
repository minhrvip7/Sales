using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.Application.DTOs.Inventory;
using Sales.Application.IServices;
using Sales.Domain.Entities.Inventory;
using Sales.Domain.Enums;
using Sales.Domain.IRepositories;

namespace Sales.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InventoryBalanceDto>> GetBalancesAsync()
        {
            var repo = _unitOfWork.Repository<InventoryBalance>();
            var balances = await repo.GetAsync(includeProperties: "Product");

            return balances.Select(b => new InventoryBalanceDto
            {
                Id = b.Id,
                ProductId = b.ProductId,
                ProductCode = b.Product?.Code ?? string.Empty,
                ProductName = b.Product?.Name ?? string.Empty,
                OnHandQty = b.OnHandQty,
                AllocatedQty = b.AllocatedQty,
                AvailableQty = b.AvailableQty
            });
        }

        public async Task<IEnumerable<InventoryTransactionDto>> GetTransactionsAsync(Guid productId)
        {
            var repo = _unitOfWork.Repository<InventoryTransaction>();
            var transactions = await repo.GetAsync(
                filter: t => t.ProductId == productId,
                orderBy: q => q.OrderByDescending(t => t.CreatedDate),
                includeProperties: "Product,TransactedUom"
            );

            return transactions.Select(t => new InventoryTransactionDto
            {
                Id = t.Id,
                ProductId = t.ProductId,
                ProductName = t.Product?.Name ?? string.Empty,
                Type = t.Type,
                ReferenceNumber = t.ReferenceNumber,
                TransactedQty = t.TransactedQty,
                TransactedUomName = t.TransactedUom?.Name ?? string.Empty,
                BaseQty = t.BaseQty,
                Reason = t.Reason,
                CreatedDate = t.CreatedDate
            });
        }

        public async Task ProcessInboundAsync(ProcessInboundDto request, Guid userId)
        {
            var balanceRepo = _unitOfWork.Repository<InventoryBalance>();
            var transactionRepo = _unitOfWork.Repository<InventoryTransaction>();
            var costRepo = _unitOfWork.Repository<ProductCost>();
            var productRepo = _unitOfWork.Repository<Sales.Domain.Entities.Product.Product>();

            var balance = await balanceRepo.FirstOrDefaultAsync(b => b.ProductId == request.ProductId);
            if (balance == null)
            {
                balance = new InventoryBalance
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    CreatedBy = userId
                };
                await balanceRepo.InsertAsync(balance);
            }
            else
            {
                balance.UpdatedBy = userId;
                balance.UpdatedDate = DateTime.UtcNow;
            }

            var product = await productRepo.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (product != null)
            {
                // Tính Moving Average Cost
                var currentCost = product.Cost;
                var currentQty = balance.OnHandQty;
                
                var newTotalQty = currentQty + request.BaseQty;
                decimal newCost = 0;
                if (newTotalQty > 0)
                {
                    newCost = ((currentQty * currentCost) + (request.BaseQty * request.UnitCost)) / newTotalQty;
                }

                // Cập nhật Product Cost
                product.Cost = newCost;
                product.StockQuantity = newTotalQty;
                productRepo.Update(product);

                // Lưu ProductCost History
                await costRepo.InsertAsync(new ProductCost
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    MovingAverageCost = newCost,
                    EffectiveDate = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }

            balance.OnHandQty += request.BaseQty;
            balance.AvailableQty = balance.OnHandQty - balance.AllocatedQty;

            await transactionRepo.InsertAsync(new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Type = TransactionType.Inbound,
                ReferenceNumber = request.ReferenceNumber,
                TransactedQty = request.TransactedQty,
                TransactedUomId = request.TransactedUomId,
                BaseQty = request.BaseQty,
                Reason = request.Reason,
                CreatedBy = userId
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ProcessOutboundAsync(ProcessOutboundDto request, Guid userId)
        {
            var balanceRepo = _unitOfWork.Repository<InventoryBalance>();
            var transactionRepo = _unitOfWork.Repository<InventoryTransaction>();
            var productRepo = _unitOfWork.Repository<Sales.Domain.Entities.Product.Product>();

            var balance = await balanceRepo.FirstOrDefaultAsync(b => b.ProductId == request.ProductId);
            if (balance == null) throw new Exception("Không tìm thấy thông tin tồn kho của sản phẩm.");

            if (request.IsSalesIssue)
            {
                if (balance.OnHandQty < request.BaseQty)
                    throw new Exception("Tồn kho On-hand không đủ để xuất.");
                
                // Xuất từ đơn hàng: giảm On-hand và giảm Allocated
                balance.OnHandQty -= request.BaseQty;
                balance.AllocatedQty -= request.BaseQty;
                // Available giữ nguyên vì đã trừ lúc tạo đơn.
            }
            else
            {
                if (balance.AvailableQty < request.BaseQty)
                    throw new Exception("Tồn kho Available không đủ để xuất cho giao dịch khác.");
                
                balance.OnHandQty -= request.BaseQty;
                balance.AvailableQty = balance.OnHandQty - balance.AllocatedQty;
            }

            balance.UpdatedBy = userId;
            balance.UpdatedDate = DateTime.UtcNow;

            var product = await productRepo.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (product != null)
            {
                product.StockQuantity = balance.OnHandQty;
                productRepo.Update(product);
            }

            await transactionRepo.InsertAsync(new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Type = request.IsSalesIssue ? TransactionType.Outbound : TransactionType.OtherIssue,
                ReferenceNumber = request.ReferenceNumber,
                TransactedQty = request.TransactedQty,
                TransactedUomId = request.TransactedUomId,
                BaseQty = request.BaseQty,
                Reason = request.Reason,
                CreatedBy = userId
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AllocateInventoryAsync(Guid productId, int qty, Guid userId)
        {
            var balanceRepo = _unitOfWork.Repository<InventoryBalance>();
            var balance = await balanceRepo.FirstOrDefaultAsync(b => b.ProductId == productId);
            if (balance == null)
            {
                balance = new InventoryBalance
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    CreatedBy = userId,
                    OnHandQty = 0,
                    AllocatedQty = qty,
                    AvailableQty = -qty
                };
                await balanceRepo.InsertAsync(balance);
            }
            else
            {
                balance.AllocatedQty += qty;
                balance.AvailableQty = balance.OnHandQty - balance.AllocatedQty;
                balance.UpdatedBy = userId;
                balance.UpdatedDate = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
