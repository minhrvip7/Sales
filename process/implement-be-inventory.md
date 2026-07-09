# Inventory Management — Backend Implementation (Phase 3a)

> **Hub:** [`implement-inventory.md`](./implement-inventory.md)
> **Spec:** [`../spec/spec-inventory.md`](../spec/spec-inventory.md)
> **Design:** [`../spec/design-inventory.md`](../spec/design-inventory.md)
> **Status:** 🚧 Phase 1 🚧 Phase 2 🚧 Phase 3 🚧 Phase 4 🚧 Phase 5 🚧 Phase 6

---

## 🚀 How to start (Backend session)
1. **Chỉ load các file Backend**: Hub, BE Spoke này, Spec, BE Design, và `.agents/AGENTS.md`.
2. **Tuân thủ Clean Architecture**.
3. **Mọi Entity phải tuân thủ chuẩn Mẫu**: Kế thừa `ISoftDelete`, đủ XML Comment tiếng Việt. Xem rule số 9 trong `AGENTS.md`.
4. **Verify mỗi phase**: `dotnet build` trong `back_end/` phải 0 errors.

---

## Phase map (Backend track)

| # | Phase | Layer |
|---|---|---|
| 1 | Enums, Constants, Interfaces | `Sales.Domain` |
| 2 | Domain Entities | `Sales.Domain` |
| 3 | EF Core mapping & Migration | `Sales.Infrastructure` |
| 4 | DTOs & Mappings | `Sales.Application` |
| 5 | Services | `Sales.Application` |
| 6 | API Controllers & Swagger | `Sales.Api` |

---

## Phase 1 — Enums, Constants, Interfaces
**Mục tiêu**: Tạo các enums và constants nền tảng.
- [ ] Thêm Enum `InventoryTransactionType`, `DocumentStatus` tại `Sales.Domain/Enums/`. Đảm bảo có `/// <summary>`.
- [ ] `dotnet build`

## Phase 2 — Domain Entities
**Mục tiêu**: Định nghĩa các bảng CSDL bằng C# Class.
- [ ] Cập nhật bảng `Product`: Đổi tên `StockQuantity` -> `OnHandQuantity`, thêm `AllocatedQuantity`, thêm `AvailableQuantity`.
- [ ] Tạo các Entities liên quan đến phiếu Nhập, Xuất, Kiểm kê, Điều chỉnh và Lịch sử thẻ kho theo chuẩn mẫu (ISoftDelete, Audit).
- [ ] `dotnet build`

## Phase 3 — EF Core mapping & Migration
**Mục tiêu**: Config mapping và update CSDL.
- [ ] Mở `SalesDbContext.cs`, thêm `DbSet<{Entity}>`.
- [ ] Trong `OnModelCreating`, cấu hình entity: bảng, khóa ngoại, `HasComment()`.
- [ ] BẮT BUỘC thêm `HasQueryFilter(e => !e.IsDeleted)`.
- [ ] Chạy lệnh thêm migration và update database.

## Phase 4 — DTOs & Mappings
**Mục tiêu**: Chuẩn bị luồng dữ liệu trung gian.
- [ ] Tạo các file DTO trong `Sales.Application/DTOs/Inventory/`.
- [ ] Cập nhật `MappingProfile.cs`.

## Phase 5 — Services
**Mục tiêu**: Xử lý business logic.
- [ ] Viết interface `IInventoryService.cs`.
- [ ] Viết class `InventoryService.cs` (xử lý logic Moving Average, Block Negative Inventory, tự sinh Phiếu Điều Chỉnh).
- [ ] Đăng ký service vào hệ thống DI.

## Phase 6 — API Controllers & Swagger
**Mục tiêu**: Xuất API cho Frontend sử dụng.
- [ ] Tạo `InventoryController.cs` trong `Sales.Api/Controllers/`.
- [ ] BẮT BUỘC thêm `[SwaggerOperation]` và `/// <summary>` cho từng endpoint.
- [ ] Verify: Chạy `dotnet run`, mở Swagger UI kiểm tra endpoint.
- [ ] Đổi status Phase này và toàn bộ track thành ✅. Quay lại file Hub để cập nhật Quality Gate.
