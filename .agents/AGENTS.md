# AGENTS.md – Quy tắc Backend Dự án Sales

Các quy tắc này áp dụng chuyên biệt cho phần **Backend** của dự án (nằm trong thư mục `back_end/`). Khi Agent làm việc với codebase C# (.NET), bắt buộc phải tuân thủ nghiêm ngặt các tiêu chuẩn dưới đây.

---

## 1. Kiến trúc & Cấu trúc dự án

- Dự án backend theo kiến trúc **Clean Architecture** gồm 4 layer:
  - `Sales.Domain` – Entity, Enum, Interface Repository
  - `Sales.Application` – DTO, Service, Mapping, IService
  - `Sales.Infrastructure` – DbContext (EF Core), Repository, Migration
  - `Sales.Api` – Controller, Middleware, Program.cs
- **Không** đặt business logic trong Controller; logic thuộc về Service (`Sales.Application`).
- **Không** tham chiếu trực tiếp `Sales.Infrastructure` từ `Sales.Application` hay `Sales.Domain`.

---

## 2. Enum

- Mọi trường trạng thái (status, type, state) **phải dùng enum** thay vì `int` thô.
- Enum đặt tại `Sales.Domain/Enums/`, mỗi enum một file riêng.
- Mỗi giá trị enum **phải có** `/// <summary>` mô tả ý nghĩa bằng tiếng Việt.
- EF Core lưu enum xuống DB dưới dạng `int` qua `.HasConversion<int>()` trong `SalesDbContext`.
- Enum hiện có:
  - `OrderStatus`: `Draft=0`, `Confirmed=1`, `Completed=2`, `Cancelled=3`
  - `PaymentStatus`: `Unpaid=0`, `PartiallyPaid=1`, `FullyPaid=2`

---

## 3. XML Documentation Comment

- **Mọi class và property** trong `Sales.Domain` (Entity, Enum) và `Sales.Application` (DTO) **phải có** `/// <summary>`.
- Comment viết **bằng tiếng Việt**, rõ ràng, nêu được mục đích và ràng buộc (ví dụ: bắt buộc, duy nhất, tối đa N ký tự).
- Property khóa ngoại phải ghi rõ tên bảng tham chiếu, ví dụ: `FK → Orders`.
- `GenerateDocumentationFile=true` và `NoWarn 1591` phải được bật trong cả 3 csproj: `Sales.Api`, `Sales.Application`, `Sales.Domain`.

---

## 4. Swagger / OpenAPI

- Mọi **action trong Controller** phải có:
  - `/// <summary>` – mô tả ngắn (hiển thị title trên Swagger)
  - `[SwaggerOperation(Summary = "...", Description = "...")]` – mô tả chi tiết
  - `[SwaggerResponse(statusCode, "...")]` cho tất cả HTTP status codes có thể trả về
- `Program.cs` phải cấu hình `AddSwaggerGen` với:
  - `SwaggerDoc` có `Title`, `Version`, `Description`
  - `c.EnableAnnotations()`
  - `c.IncludeXmlComments()` cho cả 3 XML file (Api, Application, Domain)
  - `c.UseInlineDefinitionsForEnums()`
- Package `Swashbuckle.AspNetCore.Annotations` phải được tham chiếu trong `Sales.Api.csproj`.

---

## 5. Database – EF Core & Migration

- **Mọi bảng** phải có `HasComment()` mô tả mục đích bảng trong `SalesDbContext`.
- **Mọi cột quan trọng** (business logic, FK, enum, tiền tệ, trạng thái) phải có `.HasComment()` bằng tiếng Việt.
- Sau khi thay đổi cấu hình `SalesDbContext` (thêm comment, thêm bảng, thay đổi column), **luôn tạo migration mới** với tên mô tả rõ ràng, ví dụ: `AddDatabaseComments`, `AddCustomerTable`.
- Convention đặt tên migration: `PascalCase`, tên phải mô tả đúng nội dung thay đổi.

---

## 6. Cột tiền tệ & Số thập phân

- Cột lưu **tiền tệ** (VNĐ): dùng `decimal` với `.HasPrecision(18, 2)`.
- Cột lưu **tỷ lệ quy đổi** (ConversionRate): dùng `decimal` với `.HasPrecision(18, 4)`.
- **Không** dùng `float` hay `double` cho dữ liệu tài chính; chỉ dùng `double` cho `DiscountPercentage` (phần trăm phi tài chính).

---

## 7. Quy ước đặt tên

- **Entity**: `PascalCase`, số ít (ví dụ: `Order`, `OrderDetail`, `Product`).
- **DTO**: Prefix `Create` cho input DTO; suffix `Dto` cho tất cả (ví dụ: `CreateOrderDto`, `OrderDto`).
- **Controller action**: dùng tên tiếng Anh mô tả hành động (`GetAll`, `GetById`, `Create`, `Update`, `Delete`, `Cancel`).
- **Migration**: `PascalCase` mô tả thay đổi (ví dụ: `InitialCreate`, `AddProductUomConversion`, `AddDatabaseComments`).
- **Thông báo response**: viết bằng **tiếng Việt** (ví dụ: `"Tạo đơn hàng thành công."`).

---

## 8. Xóa dữ liệu (Soft Delete)

- **Tuyệt đối không hard-delete (xóa vật lý)** bất kỳ dữ liệu nào dưới backend.
- **Tất cả các Entity** phải implement interface `ISoftDelete` (`IsDeleted`, `DeletedDate`, `DeletedBy`).
- Repository tự động xử lý khi gọi `Delete()`: chuyển thành `UPDATE IsDeleted = true`.
- DbContext đã cấu hình **Global Query Filter** (`HasQueryFilter(e => !e.IsDeleted)`) để tự động ẩn dữ liệu đã xóa ở mọi truy vấn thông thường.
- Khi cần query lịch sử (vd: xem chi tiết đơn hàng cũ có chứa sản phẩm đã xóa mềm), dùng tham số `ignoreQueryFilters = true` trong `IGenericRepository` để vượt qua filter và xem được dữ liệu đầy đủ.

---

## 9. Mẫu Entity Chuẩn

Mọi Entity khi tạo mới cần tuân theo cấu trúc chuẩn sau đây, đảm bảo có đủ XML Comment (tiếng Việt), triển khai `ISoftDelete`, và khai báo các trường Audit (`CreatedDate`, `UpdatedDate`, v.v.):

```csharp
using System;
using System.Collections.Generic;
using Sales.Domain.Interfaces;

namespace Sales.Domain.Entities.Sample
{
    /// <summary>
    /// Mô tả tóm tắt mục đích của Entity này (bằng tiếng Việt).
    /// </summary>
    public class SampleEntity : ISoftDelete
    {
        /// <summary>Khóa chính (UUID).</summary>
        public Guid Id { get; set; }

        /// <summary>Mã định danh nội bộ, duy nhất, tối đa 50 ký tự.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Tên hiển thị, tối đa 150 ký tự, bắt buộc.</summary>
        public string Name { get; set; } = string.Empty;

        // --- Các thuộc tính Audit (Bắt buộc) ---
        /// <summary>Ngày giờ tạo bản ghi (UTC).</summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>ID người dùng tạo bản ghi.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>Ngày giờ cập nhật gần nhất (UTC). Null nếu chưa cập nhật.</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>ID người dùng cập nhật lần cuối.</summary>
        public Guid? UpdatedBy { get; set; }

        // --- ISoftDelete (Bắt buộc) ---
        /// <summary>Cờ xóa mềm: true = đã bị xóa mềm, không hiển thị trong các query thông thường.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Ngày giờ thực hiện xóa mềm (UTC). Null nếu chưa xóa.</summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm.</summary>
        public Guid? DeletedBy { get; set; }

        // --- Relationships (Navigation Properties) ---
        // (Sử dụng virtual cho lazy loading nếu có)
    }
}
```

## 10. Quy trình Phát triển Tính năng (Workflow)
- Khi nhận yêu cầu có chứa từ khóa **"phát triển"** hoặc **"develop"**, bắt buộc phải tuân thủ nghiêm ngặt các bước của quy trình trong thư mục process/ (đặc biệt là workflow-srs-to-feature.md).
- Bắt đầu bằng **Phase 1** (BA Review & Concept): Đọc SRS/PRD, đặt câu hỏi phản biện, chốt phương án và tạo file concept-{module}-{feature}.md.
- TUYỆT ĐỐI không được đi thẳng vào lập Kế hoạch triển khai (Implementation Plan) hay viết code mà bỏ qua các bước tạo tài liệu Concept, Spec, và Design.
- Quan trọng: Sau mỗi bước (phase) của quy trình, BẮT BUỘC phải dừng lại, xuất kết quả để user review và phải đợi user confirm thì mới được làm bước tiếp theo.
