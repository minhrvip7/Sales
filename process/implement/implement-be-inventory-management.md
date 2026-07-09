# Backend Implementation: Quản lý Tồn kho (Inventory Management)

**Tài liệu tham khảo:** 
- Spec: `../../spec/spec-inventory-management.md`
- Design: `../../spec/design-inventory-management.md`

## Checklist Lập trình Backend

### 1. Domain Layer (`Sales.Domain`)
- [ ] Tạo Enum `TransactionType` và `AdjustmentStatus` tại `Sales.Domain/Enums/`.
- [ ] Tạo Entity `InventoryBalance` (kế thừa `ISoftDelete`).
- [ ] Tạo Entity `InventoryTransaction` (kế thừa `ISoftDelete`).
- [ ] Tạo Entity `ProductCost` (kế thừa `ISoftDelete`).
- [ ] Bổ sung đầy đủ XML Comment bằng tiếng Việt cho các Entity theo đúng Rule tại `AGENTS.md`.

### 2. Infrastructure Layer (`Sales.Infrastructure`)
- [ ] Đăng ký các Entity mới vào `DbSet` trong `SalesDbContext`.
- [ ] Cấu hình Entity bằng Fluent API (OnModelCreating):
  - `.HasComment()` mô tả rõ ràng mục đích của bảng và các cột quan trọng bằng tiếng Việt.
  - Cột tiền tệ / giá vốn (`MovingAverageCost`) dùng `.HasPrecision(18, 2)`.
  - Enum dùng `.HasConversion<int>()`.
- [ ] Tạo Migration `AddInventoryManagement` (qua terminal) và Update Database.

### 3. Application Layer (`Sales.Application`)
- [ ] Tạo các DTO: `InventoryBalanceDto`, `InventoryTransactionDto`. Bổ sung `/// <summary>` tiếng Việt.
- [ ] Định nghĩa interface `IInventoryService`.
- [ ] Triển khai `InventoryService` để xử lý logic:
  - `ProcessInboundAsync`: Tăng On-hand, tạo Transaction Inbound, cập nhật Moving Average Cost.
  - `ProcessOutboundAsync`: Cảnh báo và chặn On-hand/Available âm, giảm On-hand, lưu Transaction Outbound.
  - `AllocateInventoryAsync`: Tăng Allocated.
  - `GetBalancesAsync`, `GetTransactionsAsync`.

### 4. Api Layer (`Sales.Api`)
- [ ] Khởi tạo `InventoryController`.
- [ ] Bổ sung Swagger attributes: `/// <summary>`, `[SwaggerOperation]`, `[SwaggerResponse]` cho từng Action.
- [ ] Đăng ký Dependency Injection cho `IInventoryService` trong `Program.cs`.

## Quality Gate (Kiểm tra trước khi đóng Session)
- [ ] Lệnh `dotnet build` chạy thành công không có Warning về XML Comment.
- [ ] Lệnh `dotnet ef database update` hoàn tất không lỗi.
- [ ] Tích hợp API test qua Swagger UI trả về kết quả đúng cấu trúc DTO.
