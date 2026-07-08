# Kỹ năng 4: Kiến trúc Clean Architecture & Repository Pattern
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn chi tiết cách tổ chức logic nghiệp vụ thông qua mô hình Generic Repository phối hợp Unit of Work và áp dụng lập trình bất đồng bộ hiệu quả.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Khai thác Truy vấn dữ liệu có kèm liên kết thực thể (Eager Loading)
*   **Mô tả:** Sử dụng tham số `includeProperties` dạng chuỗi phân tách bằng dấu phẩy trong Generic Repository để tải trước các thực thể liên quan (ví dụ: lấy sản phẩm kèm theo danh mục, đơn vị cơ bản và các đơn vị quy đổi).

> [!TIP]
> **Ví dụ cụ thể:** Sử dụng trong `ProductService.GetAllProductsAsync`:
> ```csharp
> public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
> {
>     // Eager loading n cấp: Conversions và tiếp tục tải AlternativeUnit nằm trong Conversions
>     var products = await _unitOfWork.Repository<Product>().GetAsync(
>         includeProperties: "Category,BaseUnit,Conversions.AlternativeUnit");
>         
>     return _mapper.Map<IEnumerable<ProductDto>>(products);
> }
> ```

---

### Bước 2: Quản lý nhiều Repository chung một Transaction (Unit of Work)
*   **Mô tả:** Khi thực hiện nghiệp vụ cập nhật nhiều bảng cùng một lúc, hãy thực hiện toàn bộ tác vụ chèn/sửa rồi gọi duy nhất một lần lưu ở cuối hàm thông qua Unit of Work để tạo database transaction đồng bộ (Atomic Transaction).

> [!TIP]
> **Ví dụ cụ thể:** Tạo đơn hàng trong `OrderService.CreateOrderAsync`:
> ```csharp
> public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
> {
>     // 1. Tạo đơn hàng (Chèn vào repository Orders)
>     var order = new Order { ... };
>     
>     foreach (var item in dto.OrderDetails)
>     {
>         // 2. Trừ tồn kho (Cập nhật repository Products)
>         product.StockQuantity -= requiredBaseQty;
>         _unitOfWork.Repository<Product>().Update(product);
> 
>         // 3. Tạo dòng chi tiết (Thêm vào list để chèn cùng Order)
>         var detail = new OrderDetail { ... };
>         order.OrderDetails.Add(detail);
>     }
> 
>     // 4. Thực hiện chèn đơn hàng kèm toàn bộ dòng chi tiết
>     await _unitOfWork.Repository<Order>().InsertAsync(order);
> 
>     // 5. Commit duy nhất 1 lần cho toàn bộ phiên làm việc (Atomic transaction)
>     await _unitOfWork.SaveChangesAsync(); 
> 
>     return _mapper.Map<OrderDto>(order);
> }
> ```

---

### Bước 3: Lập trình Bất đồng bộ (Async/Await)
*   **Mô tả:** Sử dụng các phương thức có hậu tố `Async` cho toàn bộ các thao tác tương tác với database (`GetAsync`, `FirstOrDefaultAsync`, `InsertAsync`, `CountAsync`, `SaveChangesAsync`) và trả về kiểu dữ liệu `Task` hoặc `Task<T>`.

> [!TIP]
> **Ví dụ cụ thể:** Triển khai trong `OrderService.CancelOrderAsync`:
> ```csharp
> // Khai báo phương thức trả về Task<bool>
> public async Task<bool> CancelOrderAsync(Guid id)
> {
>     // Sử dụng await khi gọi các tác vụ bất đồng bộ của repository
>     var order = await _unitOfWork.Repository<Order>().FirstOrDefaultAsync(
>         filter: o => o.Id == id,
>         includeProperties: "OrderDetails");
>         
>     // ... xử lý logic cập nhật ...
>     
>     await _unitOfWork.SaveChangesAsync();
>     return true;
> }
> ```
