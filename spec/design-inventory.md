# BE Design: Inventory Management

## 1. Sales.Domain (Entities & Enums)

### Enums
- `InventoryTransactionType`: Inbound, Outbound, StocktakeAdjustment, Reversal.
- `DocumentStatus`: Draft, Completed, Cancelled.

### Entities modifications
- `Product.cs`: Đổi tên `StockQuantity` thành `OnHandQuantity`. Bổ sung `AllocatedQuantity` (int). Tạo property `AvailableQuantity => OnHandQuantity - AllocatedQuantity;`.

### New Entities (Tất cả implement ISoftDelete)
1. **`GoodsReceipt`**: Quản lý phiếu nhập. ReferenceNumber, Notes, Status.
2. **`GoodsReceiptDetail`**: ProductId, UnitId, Quantity, BaseQuantity, UnitCost.
3. **`GoodsIssue`**: Quản lý phiếu xuất. OrderId, Notes, Status.
4. **`GoodsIssueDetail`**: ProductId, UnitId, Quantity, BaseQuantity.
5. **`Stocktake`**: Quản lý kiểm kê. Status.
6. **`StocktakeDetail`**: ProductId, SystemQty, CountedQty, Variance.
7. **`InventoryAdjustment`**: Quản lý phiếu điều chỉnh. StocktakeId, Reason, Status.
8. **`InventoryAdjustmentDetail`**: ProductId, UnitId, AdjustedQty (âm hoặc dương).
9. **`InventoryTransaction`**: Lịch sử giao dịch/Thẻ kho. ProductId, TransactionType, TransactedQty, TransactedUOMId, BaseQty, ReferenceId, DocumentType.

## 2. Sales.Infrastructure (Database)
- Thêm `DbSet` cho 9 bảng trên.
- Sử dụng `.HasComment()` cho tất cả Entities và Enum.
- Add migration `Added_InventoryManagement`.

## 3. Sales.Application (Services & DTOs)
- **DTOs**: `GoodsReceiptDto`, `CreateGoodsReceiptDto`, ...
- **IInventoryService**:
  - `CreateGoodsReceiptAsync`, `CompleteGoodsReceiptAsync`
  - `CreateGoodsIssueAsync`, `CompleteGoodsIssueAsync` (Block negative inventory)
  - `CreateStocktakeAsync`, `CompleteStocktakeAsync` -> auto-sinh `CompleteInventoryAdjustmentAsync`.
  - `GetInventoryTransactionsAsync`.

## 4. Sales.Api (Controllers)
- `InventoryController`: Cung cấp RESTful endpoints cho các nghiệp vụ kho, gán nhãn Swagger.
