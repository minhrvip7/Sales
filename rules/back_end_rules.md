# Rules – Swagger, Enum & XML Documentation

## Phạm vi áp dụng
Áp dụng cho `Sales.Api`, `Sales.Application`, `Sales.Domain`.

---

## 1. Enum

- Mọi trường trạng thái (status, type, state) **phải dùng enum** thay vì `int` thô.
- Enum đặt tại `Sales.Domain/Enums/`, mỗi enum một file riêng.
- Mỗi giá trị enum **phải có** `/// <summary>` mô tả ý nghĩa bằng tiếng Việt.
- EF Core lưu enum xuống DB dưới dạng `int` qua `.HasConversion<int>()` trong `SalesDbContext`.
- Khi dùng enum trong Service, **phải import namespace** `Sales.Domain.Enums`.
- Enum hiện có:

| Enum | Giá trị |
|---|---|
| `OrderStatus` | `Draft=0`, `Confirmed=1`, `Completed=2`, `Cancelled=3` |
| `PaymentStatus` | `Unpaid=0`, `PartiallyPaid=1`, `FullyPaid=2` |

---

## 2. XML Documentation Comment

- **Mọi class và property** trong `Sales.Domain` (Entity, Enum) và `Sales.Application` (DTO) **phải có** `/// <summary>`.
- Comment viết **bằng tiếng Việt**, rõ ràng, nêu được mục đích và ràng buộc (bắt buộc, duy nhất, tối đa N ký tự).
- Property khóa ngoại phải ghi rõ tên bảng tham chiếu: `FK → Orders`.
- Phải bật trong cả 3 csproj:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);1591</NoWarn>
```

---

## 3. Swagger / OpenAPI

### Controller Action
Mỗi action phải có đủ 3 thành phần:

```csharp
/// <summary>Mô tả ngắn hiển thị title Swagger.</summary>
[SwaggerOperation(
    Summary = "...",
    Description = "..."
)]
[SwaggerResponse(200, "Thành công.")]
[SwaggerResponse(400, "Dữ liệu không hợp lệ.")]
[SwaggerResponse(404, "Không tìm thấy.")]
```

### Program.cs – AddSwaggerGen

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "...", Version = "v1", Description = "..." });
    c.EnableAnnotations();
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Sales.Api.xml"), true);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Sales.Application.xml"), true);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Sales.Domain.xml"), true);
    c.UseInlineDefinitionsForEnums();
});
```

### Package bắt buộc trong `Sales.Api.csproj`

```xml
<PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.6.2" />
```

---

## 4. DB Column Comments – EF Core

- **Mọi bảng** phải có `HasComment()` mô tả mục đích trong `SalesDbContext`.
- **Mọi cột quan trọng** (FK, enum, tiền tệ, trạng thái, audit) phải có `.HasComment()` bằng tiếng Việt.

```csharp
entity.ToTable("Orders", t => t.HasComment("Đơn hàng bán lẻ..."));
entity.Property(e => e.OrderStatus)
    .HasConversion<int>()
    .HasComment("Trạng thái: 0=Draft, 1=Confirmed, 2=Completed, 3=Cancelled.");
```

- Sau mỗi thay đổi `SalesDbContext`, **luôn tạo migration mới** với tên `PascalCase` mô tả rõ thay đổi.
