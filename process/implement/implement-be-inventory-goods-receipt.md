# Goods Receipt — Backend Implementation (Phase 3a)

> **Hub:** [`implement-inventory-goods-receipt.md`](./implement-inventory-goods-receipt.md)
> **Spec:** [`../spec-inventory-goods-receipt.md`](../spec-inventory-goods-receipt.md)
> **Design:** [`../design-inventory-goods-receipt.md`](../design-inventory-goods-receipt.md)
> **Status:** 🚧 Phase 1 🚧 Phase 2 🚧 Phase 3 🚧 Phase 4 🚧 Phase 5 🚧 Phase 6

---

## 🚀 How to start (Backend session)
1. **Chỉ load các file Backend**: Hub, BE Spoke này, Spec, BE Design, và `.agents/AGENTS.md`. KHÔNG load FE Design.
2. **Tuân thủ Clean Architecture**: 
   - `Sales.Domain`: Entities, Interfaces, Enums.
   - `Sales.Application`: DTOs, AutoMapper, Services.
   - `Sales.Infrastructure`: DbContext, Repositories, Migrations.
   - `Sales.Api`: Controllers, Program.cs.
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
- [ ] Thêm Enum `GoodsReceiptType` và `GoodsReceiptStatus` tại `Sales.Domain/Enums/`. Đảm bảo có `/// <summary>`.
- [ ] Khai báo Interfaces nếu có logic đặc thù tại `Sales.Domain/Interfaces/`.
- [ ] `dotnet build`

## Phase 2 — Domain Entities
**Mục tiêu**: Định nghĩa các bảng CSDL bằng C# Class.
- [ ] Tạo `GoodsReceipt.cs` và `GoodsReceiptLine.cs` trong `Sales.Domain/Entities/Inventory` theo mẫu chuẩn.
- [ ] Implement `ISoftDelete`. Đầy đủ properties audit (`CreatedDate`, `CreatedBy`, ...).
- [ ] `dotnet build`

## Phase 3 — EF Core mapping & Migration
**Mục tiêu**: Config mapping và update CSDL.
- [ ] Mở `SalesDbContext.cs`, thêm `DbSet<GoodsReceipt>` và `GoodsReceiptLine`.
- [ ] Trong `OnModelCreating`, cấu hình entity: bảng, khóa ngoại, `HasComment()`, `HasPrecision(18,4)` cho `ConversionRate`, `HasPrecision(18,2)` cho quantity.
- [ ] BẮT BUỘC thêm `HasQueryFilter(e => !e.IsDeleted)`.
- [ ] Chạy lệnh `dotnet ef migrations add Added_GoodsReceipt_Entities --project Sales.Infrastructure --startup-project Sales.Api`.
- [ ] Chạy lệnh `dotnet ef database update --project Sales.Infrastructure --startup-project Sales.Api`.

## Phase 4 — DTOs & Mappings
**Mục tiêu**: Chuẩn bị luồng dữ liệu trung gian.
- [ ] Tạo các file DTO trong `Sales.Application/DTOs/Inventory/` (`GoodsReceiptDto.cs`, `CreateGoodsReceiptDto.cs`, `ResolveBarcodeDto.cs`...).
- [ ] Cập nhật (hoặc tạo) `MappingProfile.cs` trong `Sales.Application/Mappings/` để config AutoMapper mapping từ Entity <-> DTO.

## Phase 5 — Services
**Mục tiêu**: Xử lý business logic.
- [ ] Viết interface `IGoodsReceiptService.cs` trong `Sales.Application/Services/`.
- [ ] Viết class `GoodsReceiptService.cs` implement interface trên. Áp dụng Transaction khi gọi `CompleteAsync`.
- [ ] Đăng ký service vào `Program.cs` hoặc setup DI.

## Phase 6 — API Controllers & Swagger
**Mục tiêu**: Xuất API cho Frontend sử dụng.
- [ ] Tạo `GoodsReceiptsController.cs` trong `Sales.Api/Controllers/`.
- [ ] BẮT BUỘC thêm `[SwaggerOperation]` và `/// <summary>` cho từng endpoint.
- [ ] Verify: Chạy `dotnet run`, mở Swagger UI kiểm tra endpoint.
- [ ] **Đổi status Phase này và toàn bộ track thành ✅. Quay lại file Hub để cập nhật Quality Gate.**
