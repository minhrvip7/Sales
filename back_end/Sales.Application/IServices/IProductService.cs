using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Product;
using Sales.Application.DTOs.Common;

namespace Sales.Application.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<PagedResponse<ProductDto>> GetPagedProductsAsync(PagedRequest request);
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<bool> UpdateProductAsync(Guid id, CreateProductDto dto);
        Task<bool> DeleteProductAsync(Guid id);
    }
}

