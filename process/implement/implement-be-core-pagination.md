# Implementation: Pagination (Backend Spoke)

## Checklist triển khai Backend

### 1. DTOs & Cơ sở hạ tầng
- [ ] Bổ sung thư mục `Sales.Application/DTOs/Common`.
- [ ] Tạo file `PagedRequest.cs` trong `Common`.
- [ ] Tạo file `PagedResponse.cs` trong `Common`.
- [ ] Cập nhật interface `IGenericRepository` (nếu cần) để trả về `IQueryable<T>` hỗ trợ query LINQ.

### 2. Domain & Application Services
- [ ] Sửa `ICategoryService` và `CategoryService`: Thêm hàm `GetPagedCategoriesAsync`.
- [ ] Sửa `IOrderService` và `OrderService`: Thêm hàm `GetPagedOrdersAsync`.
- [ ] Sửa `IProductService` và `ProductService`: Thêm hàm `GetPagedProductsAsync`.
- [ ] Sửa `IUnitService` và `UnitService`: Thêm hàm `GetPagedUnitsAsync`.

### 3. API Controllers
- [ ] Sửa `CategoryController`: Đổi `GetAll` thành `GetPaged`, nhận `PagedRequest`.
- [ ] Sửa `OrderController`: Đổi `GetAll` thành `GetPaged`, nhận `PagedRequest`.
- [ ] Sửa `ProductController`: Đổi `GetAll` thành `GetPaged`, nhận `PagedRequest`.
- [ ] Sửa `UnitController`: Đổi `GetAll` thành `GetPaged`, nhận `PagedRequest`.
- [ ] Đảm bảo các annotation Swagger (`[SwaggerOperation]`) được cập nhật tương ứng.

### 4. Verification
- [ ] Chạy `dotnet build` tại thư mục `back_end/` không lỗi.
- [ ] Kiểm tra API bằng công cụ (Postman / Swagger) trả về đúng định dạng `PagedResponse`.
