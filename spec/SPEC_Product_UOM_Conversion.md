# Tài liệu Tổng hợp các Thay đổi và Bổ sung Đặc biệt (Special Changes Specification)
## Tính năng: Quy đổi Đơn vị tính Sản phẩm

Tài liệu này tổng hợp các điểm thay đổi, bổ sung cụ thể trên cấu trúc thư mục và mã nguồn của dự án hiện tại để hiện thực hóa tính năng Quy đổi Đơn vị tính Sản phẩm dựa trên tài liệu SRS.

---

## 1. THAY ĐỔI VỀ CƠ SỞ DỮ LIỆU & ENTITY (BACKEND)

### 1.1. Thực thể `Product`
*   **File:** [Product.cs](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Domain/Entities/Product/Product.cs)
*   **Chi tiết thay đổi:**
    *   Đổi tên thuộc tính `UnitId` thành `BaseUnitId` (đổi tên cả cột tương ứng trong database).
    *   Thêm thuộc tính điều hướng để liên kết với thực thể quy đổi đơn vị tính mới:
        ```csharp
        public virtual ICollection<ProductUnitConversion> Conversions { get; set; } = new List<ProductUnitConversion>();
        ```

### 1.2. Thêm mới Thực thể `ProductUnitConversion`
*   **File:** `ProductUnitConversion.cs` (tạo mới trong thư mục [Entities/Product](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Domain/Entities/Product))
*   **Nội dung chính:**
    ```csharp
    using System;

    namespace Sales.Domain.Entities.Product
    {
        public class ProductUnitConversion
        {
            public Guid Id { get; set; }
            public Guid ProductId { get; set; }
            public Guid AlternativeUnitId { get; set; }
            public decimal ConversionRate { get; set; } // Phải lớn hơn 0
            public string? Barcode { get; set; } // Phải là duy nhất trên toàn hệ thống
            public decimal? Price { get; set; } // Giá bán ghi đè (Có thể null)

            // Relationships
            public virtual Product Product { get; set; } = null!;
            public virtual Unit AlternativeUnit { get; set; } = null!;
        }
    }
    ```

### 1.3. Thực thể `OrderDetail`
*   **File:** [OrderDetail.cs](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Domain/Entities/Order/OrderDetail.cs)
*   **Chi tiết thay đổi:**
    *   Bổ sung trường `UnitId` (Guid) để lưu vết đơn vị tính được chọn tại thời điểm mua hàng.
    *   Bổ sung trường `ConversionRate` (decimal) để lưu hệ số quy đổi tại thời điểm giao dịch phát sinh.
    *   Thêm thuộc tính điều hướng tới bảng `Unit`:
        ```csharp
        public virtual Unit Unit { get; set; } = null!;
        ```

### 1.4. Cấu hình DbContext (`SalesDbContext`)
*   **File:** [SalesDbContext.cs](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Infrastructure/DataContext/SalesDbContext.cs)
*   **Chi tiết thay đổi:**
    *   Khai báo `DbSet<ProductUnitConversion> ProductUnitConversions { get; set; }`
    *   Cấu hình Fluent API cho bảng `ProductUnitConversions` (bao gồm thiết lập Index Unique cho trường `Barcode` và độ chính xác cho `ConversionRate`, `Price`).
    *   Cấu hình ràng buộc Unique Barcode xuyên suốt toàn bộ hệ thống bằng cách kiểm tra cả bảng `Products` và `ProductUnitConversions`.

---

## 2. THAY ĐỔI VỀ LOGIC DỊCH VỤ (BACKEND SERVICES)

### 2.1. Logic Quản lý Sản phẩm (`ProductService`)
*   **File:** [ProductService.cs](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Application/Services/ProductService.cs)
*   **Chi tiết bổ sung:**
    *   **Khóa đơn vị cơ bản:** Thực hiện truy vấn kiểm tra trong `OrderDetails` trước khi cập nhật. Nếu sản phẩm đã phát sinh đơn hàng, chặn hành động thay đổi `BaseUnitId` và ném lỗi nghiệp vụ.
    *   **Validate khi thêm/sửa:** Kiểm tra hệ số quy đổi phải $> 0$, đơn vị quy đổi phải hợp lệ trong danh mục hệ thống và không trùng lặp đơn vị cơ bản hoặc đơn vị quy đổi khác của sản phẩm đó.
    *   **Validate Barcode duy nhất:** Trước khi lưu, kiểm tra trùng lặp barcode trên cả hai thực thể sản phẩm và đơn vị quy đổi của tất cả các sản phẩm khác trong database.

### 2.2. Logic Bán hàng & Trừ kho (`OrderService`)
*   **File:** [OrderService.cs](file:///c:/Users/Public/Apps/Test/TestAI/back_end/Sales.Application/Services/OrderService.cs)
*   **Chi tiết thay đổi:**
    *   **Nhận diện giá bán:**
        *   Nếu đơn vị mua trùng với `BaseUnitId` -> Lấy `product.Price`.
        *   Nếu là đơn vị quy đổi: Ưu tiên lấy giá ghi đè `ProductUnitConversion.Price`. Nếu không thiết lập (bằng null) -> tự động tính toán $product.Price \times ConversionRate$.
    *   **Nhận diện giá vốn:** Luôn tự động tính toán bằng: $product.Cost \times ConversionRate$.
    *   **Kiểm tra tồn kho quy đổi:** Quy đổi số lượng mua theo đơn vị quy đổi về đơn vị cơ bản ($Quantity \times ConversionRate$) trước khi so sánh với tồn kho của sản phẩm (`product.StockQuantity`).
    *   **Trừ/Hoàn kho:** Trừ tồn kho theo số lượng quy đổi thực tế. Khi hủy đơn hàng (`CancelOrderAsync`), cộng trả lại tồn kho theo công thức tương tự.
    *   Lưu các thông tin `UnitId` và `ConversionRate` vào bảng `OrderDetails`.

---

## 3. THAY ĐỔI GIAO DIỆN NGƯỜI DÙNG (FRONTEND)

### 3.1. Form Quản lý Sản phẩm
*   **File:** [ProductList.tsx](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/features/product/ProductList.tsx)
*   **Chi tiết thay đổi:**
    *   Thêm tab **"Đơn vị tính & Quy đổi"** vào Modal thêm/sửa sản phẩm.
    *   Gọi API backend để kiểm tra xem sản phẩm đã có lịch sử giao dịch chưa, nếu có thì khóa (disable) dropdown chọn đơn vị tính cơ bản.
    *   Xây dựng bảng nhập danh sách đơn vị quy đổi cho phép người dùng thêm dòng, chọn đơn vị tính từ danh sách hệ thống, nhập hệ số, barcode và giá bán ghi đè.

### 3.2. Giao diện Bán hàng POS
*   **File:** [CreateOrder.tsx](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/features/order/CreateOrder.tsx)
*   **Chi tiết thay đổi:**
    *   Thêm dropdown đơn vị tính tại bảng danh sách giỏ hàng. Dropdown này sẽ hiển thị đơn vị cơ bản cùng toàn bộ đơn vị quy đổi của sản phẩm đó.
    *   Khi người dùng thay đổi đơn vị tính trong dropdown: hệ thống tự động cập nhật đơn giá của sản phẩm và tính toán lại tổng tiền của đơn hàng.
    *   **Quét barcode tự động:** Bổ sung logic xử lý khi quét mã vạch. Nếu mã vạch khớp với mã vạch của đơn vị quy đổi, hệ thống tự động thêm sản phẩm đó vào giỏ với đơn vị tính là đơn vị quy đổi tương ứng (số lượng mặc định là 1).
