using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Category;
using Sales.Application.DTOs.Common;

namespace Sales.Application.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<PagedResponse<CategoryDto>> GetPagedCategoriesAsync(PagedRequest request);
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<bool> UpdateCategoryAsync(Guid id, CreateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
