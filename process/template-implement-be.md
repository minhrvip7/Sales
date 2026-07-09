# Template: `implement-be-{module}-{feature}.md` (Backend spoke)

**Mục đích**: Chứa checklist cụ thể cho phần Backend Implementation dựa trên kiến trúc C# Clean Architecture.

**Áp dụng**: Sinh ở Phase 2.5, thực thi ở Phase 3a.

---

# {Feature Name} — Backend Implementation (Phase 3a)

> **Hub:** [`implement-{module}-{feature}.md`](./implement-{module}-{feature}.md)
> **Spec:** [`docs/spec/{module}/spec-{module}-{feature}.md`](../../spec/{module}/spec-{module}-{feature}.md)
> **Design:** [`docs/design/{module}/design-{module}-{feature}.md`](../../design/{module}/design-{module}-{feature}.md)
> **Status:** 🚧 Phase 1 🚧 Phase 2 ... 🚧 Phase {N}

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
- [ ] Thêm Enum tại `Sales.Domain/Enums/{EnumName}.cs`. Đảm bảo có `/// <summary>`.
- [ ] Khai báo Interfaces nếu có logic đặc thù tại `Sales.Domain/Interfaces/`.
- [ ] `dotnet build`

## Phase 2 — Domain Entities
**Mục tiêu**: Định nghĩa các bảng CSDL bằng C# Class.
- [ ] Tạo `Sales.Domain/Entities/{Module}/{Entity}.cs` theo mẫu chuẩn.
- [ ] Implement `ISoftDelete`. Đầy đủ properties audit (`CreatedDate`, `CreatedBy`, ...).
- [ ] `dotnet build`

## Phase 3 — EF Core mapping & Migration
**Mục tiêu**: Config mapping và update CSDL.
- [ ] Mở `SalesDbContext.cs`, thêm `DbSet<{Entity}>`.
- [ ] Trong `OnModelCreating`, cấu hình entity: bảng, khóa ngoại, `HasComment()`, `HasPrecision()` cho decimal.
- [ ] BẮT BUỘC thêm `HasQueryFilter(e => !e.IsDeleted)`.
- [ ] Chạy lệnh `dotnet ef migrations add Added_{Entity} --project Sales.Infrastructure --startup-project Sales.Api`.
- [ ] Chạy lệnh `dotnet ef database update --project Sales.Infrastructure --startup-project Sales.Api`.

## Phase 4 — DTOs & Mappings
**Mục tiêu**: Chuẩn bị luồng dữ liệu trung gian.
- [ ] Tạo các file DTO trong `Sales.Application/DTOs/{Entity}/` (Vd: `{Entity}Dto.cs`, `Create{Entity}Dto.cs`). Đảm bảo có `/// <summary>`.
- [ ] Cập nhật (hoặc tạo) `MappingProfile.cs` trong `Sales.Application/Mappings/` để config AutoMapper mapping từ Entity <-> DTO.

## Phase 5 — Services
**Mục tiêu**: Xử lý business logic.
- [ ] Viết interface `I{Entity}Service.cs` trong `Sales.Application/Services/`.
- [ ] Viết class `{Entity}Service.cs` implement interface trên.
- [ ] DI tiêm `IGenericRepository` vào service. 
- [ ] *Chú ý:* Xóa dữ liệu dùng `_repository.Delete()` (sẽ tự xử lý Soft Delete).
- [ ] Đăng ký service vào `Program.cs` hoặc file setup DI.

## Phase 6 — API Controllers & Swagger
**Mục tiêu**: Xuất API cho Frontend sử dụng.
- [ ] Tạo `{Entity}Controller.cs` trong `Sales.Api/Controllers/`.
- [ ] BẮT BUỘC thêm `[SwaggerOperation]` và `/// <summary>` cho từng endpoint.
- [ ] Verify: Chạy `dotnet run`, mở Swagger UI kiểm tra endpoint.
- [ ] **Đổi status Phase này và toàn bộ track thành ✅. Quay lại file Hub để cập nhật Quality Gate.**

---

## ❓ Open Questions (Backend Only)
- **Q1**: ...
