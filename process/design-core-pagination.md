# Backend Design: Pagination

## 1. Cấu trúc dùng chung (Common DTOs)
Tạo thư mục `Sales.Application/DTOs/Common`.

**1.1. `PagedRequest.cs`**
```csharp
namespace Sales.Application.DTOs.Common
{
    /// <summary>
    /// Request chuẩn dùng cho phân trang và tìm kiếm cơ bản.
    /// </summary>
    public class PagedRequest
    {
        /// <summary>Trang hiện tại (bắt đầu từ 1).</summary>
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 20;

        /// <summary>Số lượng bản ghi trên một trang (tối đa 100).</summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 100) ? 100 : (value <= 0 ? 20 : value);
        }

        /// <summary>Từ khóa tìm kiếm (tùy chọn).</summary>
        public string? Keyword { get; set; }
    }
}
```

**1.2. `PagedResponse.cs`**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Application.DTOs.Common
{
    /// <summary>
    /// Response chuẩn trả về dữ liệu phân trang.
    /// </summary>
    public class PagedResponse<T>
    {
        /// <summary>Danh sách dữ liệu của trang hiện tại.</summary>
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        /// <summary>Tổng số bản ghi thỏa mãn điều kiện tìm kiếm.</summary>
        public int TotalRecords { get; set; }

        /// <summary>Tổng số trang.</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>Trang hiện tại.</summary>
        public int PageNumber { get; set; }

        /// <summary>Số lượng bản ghi trên một trang.</summary>
        public int PageSize { get; set; }

        public PagedResponse(IEnumerable<T> data, int totalRecords, int pageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
```

## 2. Thay đổi Service & Interface
Nâng cấp các Interface tại `Sales.Application/IServices/`:
```csharp
Task<PagedResponse<CategoryDto>> GetPagedCategoriesAsync(PagedRequest request);
Task<PagedResponse<OrderDto>> GetPagedOrdersAsync(PagedRequest request);
Task<PagedResponse<ProductDto>> GetPagedProductsAsync(PagedRequest request);
Task<PagedResponse<UnitDto>> GetPagedUnitsAsync(PagedRequest request);
```

## 3. Thay đổi Controller
Nâng cấp các Controller API (`CategoryController`, `OrderController`, `ProductController`, `UnitController`) để nhận `[FromQuery] PagedRequest request`. Đổi hàm `GetAll` thành `GetPaged`.

```csharp
/// <summary>
/// Lấy danh sách phân trang và tìm kiếm.
/// </summary>
[HttpGet]
[SwaggerOperation(Summary = "Lấy danh sách phân trang", Description = "Lấy danh sách có hỗ trợ phân trang và tìm kiếm theo từ khóa.")]
[SwaggerResponse(200, "Thành công")]
public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
{
    var result = await _service.GetPaged...Async(request);
    return Ok(result);
}
```

## 4. EF Core IQueryable (Logic implementation)
Trong Service layer (ví dụ: `CategoryService.cs`):
```csharp
public async Task<PagedResponse<CategoryDto>> GetPagedCategoriesAsync(PagedRequest request)
{
    // Cần lấy IQueryable từ GenericRepository hoặc implement trực tiếp ở Service nếu Repo hỗ trợ IQueryable
    // Tạm gọi _repository.GetQueryable()
    var query = _categoryRepository.GetQueryable();

    if (!string.IsNullOrEmpty(request.Keyword))
    {
        query = query.Where(x => x.Name.Contains(request.Keyword) || x.Code.Contains(request.Keyword));
    }

    var totalRecords = await query.CountAsync();

    var data = await query
        .OrderByDescending(x => x.CreatedDate)
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .ToListAsync();

    var dtoList = _mapper.Map<IEnumerable<CategoryDto>>(data);

    return new PagedResponse<CategoryDto>(dtoList, totalRecords, request.PageNumber, request.PageSize);
}
```
*Lưu ý:* Cần bổ sung phương thức `GetQueryable()` vào `IGenericRepository` nếu chưa có.
