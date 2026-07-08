# Kỹ năng 2: Thiết kế DTO & Mappings (AutoMapper)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn chi tiết quy trình thiết kế lớp truyền dữ liệu (DTO), cấu hình ánh xạ tự động AutoMapper và kỹ thuật tùy biến xử lý các quan hệ dữ liệu phức tạp.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Thiết kế DTOs (Data Transfer Objects)
*   **Mô tả:** Tạo các lớp chứa thông tin phục vụ truyền nhận dữ liệu qua API. Phân tách rõ ràng giữa DTO đầu vào (Request DTO - không chứa trường thừa) và DTO đầu ra (Response DTO - làm phẳng cấu trúc dữ liệu để UI dễ hiển thị).

> [!TIP]
> **Ví dụ cụ thể:** Thiết kế `ProductDto` (đầu ra) và `ProductUnitConversionDto` (đầu vào/ra):
> ```csharp
> // ProductDto.cs
> public class ProductDto
> {
>     public Guid Id { get; set; }
>     public string Name { get; set; } = string.Empty;
>     public string Code { get; set; } = string.Empty;
>     public string? Barcode { get; set; }
>     public decimal Price { get; set; }
>     public decimal Cost { get; set; }
>     public int StockQuantity { get; set; }
>     
>     // Làm phẳng tên liên kết thay vì trả nguyên Entity Category
>     public Guid CategoryId { get; set; }
>     public string CategoryName { get; set; } = string.Empty; 
>     
>     public Guid BaseUnitId { get; set; }
>     public string BaseUnitName { get; set; } = string.Empty;
>     
>     // Danh sách đơn vị quy đổi đi kèm
>     public List<ProductUnitConversionDto> Conversions { get; set; } = new();
> }
> 
> // ProductUnitConversionDto.cs
> public class ProductUnitConversionDto
> {
>     public Guid AlternativeUnitId { get; set; }
>     public string AlternativeUnitName { get; set; } = string.Empty;
>     public decimal ConversionRate { get; set; }
>     public string? Barcode { get; set; }
>     public decimal? Price { get; set; }
> }
> ```

---

### Bước 2: Khai báo ánh xạ trong Mapping Profile
*   **Mô tả:** Đăng ký ánh xạ các class trong một lớp kế thừa từ `Profile` của AutoMapper.

> [!TIP]
> **Ví dụ cụ thể:** Đăng ký trong `MappingProfile.cs`:
> ```csharp
> using AutoMapper;
> using Sales.Application.DTOs.Product;
> using Sales.Domain.Entities.Product;
> 
> namespace Sales.Application.Mappings
> {
>     public class MappingProfile : Profile
>     {
>         public MappingProfile()
>         {
>             // Ánh xạ từ Product sang ProductDto
>             CreateMap<Product, ProductDto>()
>                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
>                 .ForMember(dest => dest.BaseUnitName, opt => opt.MapFrom(src => src.BaseUnit.Name))
>                 .ForMember(dest => dest.Conversions, opt => opt.MapFrom(src => src.Conversions));
> 
>             // Ánh xạ từ ProductUnitConversion sang ProductUnitConversionDto
>             CreateMap<ProductUnitConversion, ProductUnitConversionDto>()
>                 .ForMember(dest => dest.AlternativeUnitName, opt => opt.MapFrom(src => src.AlternativeUnit.Name));
>         }
>     }
> }
> ```

---

### Bước 3: Tùy biến bỏ qua Mapping đối với các quan hệ phức tạp
*   **Mô tả:** Khi lưu một thực thể phức tạp (như Product có chứa danh sách UOM), không nên cho AutoMapper tự động tạo mới các bản ghi con. Cần bỏ qua (`Ignore`) thuộc tính con ở Profile và xử lý cập nhật thủ công ở Service layer.

> [!TIP]
> **Ví dụ cụ thể:** Thiết lập trong `MappingProfile.cs` bỏ qua UOM:
> ```csharp
> CreateMap<CreateProductDto, Product>()
>     .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
>     .ForMember(dest => dest.Conversions, opt => opt.Ignore()); // Bỏ qua Conversions để tự xử lý logic kiểm tra duy nhất mã vạch chéo bảng
> ```

---

### Bước 4: Tiêm (Inject) và sử dụng IMapper
*   **Mô tả:** Sử dụng AutoMapper trong Service/Controller để chuyển đổi dữ liệu.

> [!TIP]
> **Ví dụ cụ thể:** Sử dụng trong `ProductService`:
> ```csharp
> public class ProductService : IProductService
> {
>     private readonly IUnitOfWork _unitOfWork;
>     private readonly IMapper _mapper;
> 
>     public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
>     {
>         _unitOfWork = unitOfWork;
>         _mapper = mapper;
>     }
> 
>     public async Task<ProductDto> GetProductByIdAsync(Guid id)
>     {
>         var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
>             filter: p => p.Id == id,
>             includeProperties: "Category,BaseUnit,Conversions.AlternativeUnit");
>         
>         // Map từ Entity sang DTO
>         return _mapper.Map<ProductDto>(product);
>     }
> }
> ```
