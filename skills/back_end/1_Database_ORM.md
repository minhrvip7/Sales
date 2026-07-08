# Kỹ năng 1: Thiết kế Cơ sở Dữ liệu & ORM (EF Core)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn chi tiết các bước thiết kế thực thể dữ liệu, cấu hình Fluent API, thiết lập chỉ mục duy nhất và quản lý di dân (Migrations) kèm theo ví dụ thực tế trong dự án.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Khai báo lớp Thực thể (Entity Class)
*   **Mô tả:** Định nghĩa thực thể đại diện cho bảng CSDL trong phân lớp Domain. Đảm bảo khai báo các khóa ngoại dạng thô (`Guid ProductId`) và các thuộc tính điều hướng (`virtual Product Product`) để EF Core liên kết dữ liệu.

> [!TIP]
> **Ví dụ cụ thể:** Tạo thực thể quy đổi đơn vị tính trong tệp `ProductUnitConversion.cs`:
> ```csharp
> using System;
> 
> namespace Sales.Domain.Entities.Product
> {
>     public class ProductUnitConversion
>     {
>         public Guid Id { get; set; }
>         public Guid ProductId { get; set; }
>         public Guid AlternativeUnitId { get; set; }
>         public decimal ConversionRate { get; set; } // Hệ số quy đổi
>         public string? Barcode { get; set; } // Mã vạch riêng
>         public decimal? Price { get; set; } // Giá bán ghi đè
> 
>         // Thuộc tính điều hướng (Navigation Properties)
>         public virtual Product Product { get; set; } = null!;
>         public virtual Unit AlternativeUnit { get; set; } = null!;
>     }
> }
> ```

---

### Bước 2: Cấu hình Fluent API trong DbContext
*   **Mô tả:** Định nghĩa quy tắc ánh xạ thực thể xuống bảng thực tế trong Database. Sử dụng Fluent API trong phương thức `OnModelCreating` của DbContext để tách biệt logic cấu hình khỏi thực thể.
*   **Các quy chuẩn thiết lập:**
    1.  Tên bảng thực tế (`ToTable`).
    2.  Khóa chính (`HasKey`).
    3.  Độ chính xác số thập phân (`HasPrecision`) cho các cột số tiền, tỷ lệ.
    4.  Cấu hình Unique Index để đảm bảo tính duy nhất ở tầng DB.
    5.  Thiết lập khóa ngoại và hành vi xóa (`OnDelete(DeleteBehavior.Restrict/Cascade)`).

> [!TIP]
> **Ví dụ cụ thể:** Cấu hình trong `SalesDbContext.cs`:
> ```csharp
> modelBuilder.Entity<ProductUnitConversion>(entity =>
> {
>     // 1. Tên bảng
>     entity.ToTable("ProductUnitConversions");
>     
>     // 2. Khóa chính
>     entity.HasKey(e => e.Id);
>     
>     // 3. Định dạng dữ liệu và độ chính xác
>     entity.Property(e => e.ConversionRate).HasPrecision(18, 4); // Hệ số hỗ trợ 4 số thập phân
>     entity.Property(e => e.Barcode).HasMaxLength(50);
>     entity.Property(e => e.Price).HasPrecision(18, 2); // Giá tiền có 2 số thập phân
>     
>     // 4. Chỉ mục duy nhất (Unique Index)
>     entity.HasIndex(e => e.Barcode).IsUnique();
> 
>     // 5. Thiết lập quan hệ & Khóa ngoại
>     entity.HasOne(d => d.Product)
>         .WithMany(p => p.Conversions)
>         .HasForeignKey(d => d.ProductId)
>         .OnDelete(DeleteBehavior.Cascade); // Xóa sản phẩm thì tự động xóa UOM đi kèm
> 
>     entity.HasOne(d => d.AlternativeUnit)
>         .WithMany()
>         .HasForeignKey(d => d.AlternativeUnitId)
>         .OnDelete(DeleteBehavior.Restrict); // Không cho xóa đơn vị tính nếu đang được tham chiếu
> });
> ```

---

### Bước 3: Tạo bản di dân Cơ sở Dữ liệu (Add Migration)
*   **Mô tả:** Chạy lệnh EF Core CLI để sinh ra mã SQL cập nhật CSDL tự động dựa trên thay đổi của DbContext.

> [!TIP]
> **Ví dụ cụ thể:** Cách thực hiện (Chạy ở thư mục gốc `back_end`):
> ```bash
> dotnet ef migrations add AddProductUomConversion --startup-project Sales.Api --project Sales.Infrastructure
> ```
> *Lưu ý:* Dự án phải được biên dịch thành công không có lỗi trước khi chạy lệnh này.

---

### Bước 4: Áp dụng thay đổi xuống Database (Database Update)
*   **Mô tả:** Thực thi bản di dân vào hệ quản trị CSDL thực tế (ví dụ: PostgreSQL hoặc SQLite).

> [!TIP]
> **Ví dụ cụ thể:** Cách thực hiện:
> ```bash
> dotnet ef database update --startup-project Sales.Api --project Sales.Infrastructure
> ```
