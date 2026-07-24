# Backend Design: Tính năng Nhập Kho (Goods Receipt)

## 1. Domain Entities & Enums (Sales.Domain)
- **Enums**:
  - `GoodsReceiptType`: `Purchase = 1`, `Return = 2`, `Transfer = 3`
  - `GoodsReceiptStatus`: `Draft = 0`, `Completed = 1`, `Cancelled = 2`
- **Entities**:
  - `GoodsReceipt`: `Id`, `Code`, `Type`, `ReferenceId`, `ReferenceCode`, `SupplierId`, `WarehouseId`, `ReceiptDate`, `Notes`, `Status`, `TotalQuantity`. Kế thừa `ISoftDelete` và Audit.
  - `GoodsReceiptLine`: `Id`, `GoodsReceiptId`, `ProductId`, `UoMId`, `ConversionRate`, `ExpectedQuantity`, `ActualQuantity`, `Notes`. Kế thừa `ISoftDelete` và Audit.

## 2. DTOs (Sales.Application)
- `CreateGoodsReceiptDto` / `CreateGoodsReceiptLineDto`
- `UpdateGoodsReceiptDto` / `UpdateGoodsReceiptLineDto`
- `GoodsReceiptDto` (hiển thị chi tiết)
- `GoodsReceiptSummaryDto` (hiển thị danh sách có phân trang)
- `ResolveBarcodeDto` (kết quả quét mã vạch: `ProductId`, `UoMId`, `ConversionRate`)

## 3. Interfaces & Services (Sales.Application)
- `IGoodsReceiptService`:
  - `GetPagedAsync(...)`
  - `GetByIdAsync(...)`
  - `CreateAsync(...)`
  - `UpdateAsync(...)`: Trả về exception nếu phiếu đã Completed.
  - `CompleteAsync(...)`: Chốt phiếu (Posting).
  - `ResolveBarcodeAsync(...)`: Phân giải mã vạch.

## 4. Logic Xử lý Chốt Phiếu - CompleteAsync
Bắt buộc chạy trong `IDbContextTransaction`:
1. Đổi `Status` của `GoodsReceipt` sang `Completed`.
2. Lặp qua các `GoodsReceiptLine` (bỏ qua những dòng có ActualQuantity = 0):
   - Tạo bản ghi `InventoryTransaction` với loại Inbound.
   - Cập nhật tồn kho trong bảng `InventoryBalance` (`Quantity` cộng thêm `ActualQuantity * ConversionRate`).
   - Cập nhật giá vốn (Moving Average Cost) lưu vào bảng `ProductCost`.
3. Lưu toàn bộ thay đổi (`SaveChangesAsync`).

## 5. Cấu hình DB (Sales.Infrastructure)
- Dùng `HasPrecision(18,4)` cho các trường tỷ lệ (`ConversionRate`) và `HasPrecision(18,2)` cho `Quantity`.
- Cấu hình 1-N giữa `GoodsReceipt` và `GoodsReceiptLine`.
- Cấu hình `HasQueryFilter(e => !e.IsDeleted)`.
- Gắn `HasComment` tiếng Việt đầy đủ.
- Sinh EF Migration (ví dụ: `AddGoodsReceiptEntities`).
