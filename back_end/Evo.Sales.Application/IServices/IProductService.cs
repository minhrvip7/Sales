using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evo.Sales.Application.DTOs.Product;

namespace Evo.Sales.Application.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<bool> UpdateProductAsync(Guid id, CreateProductDto dto);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
