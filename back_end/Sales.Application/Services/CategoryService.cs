using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Sales.Application.DTOs.Category;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sales.Domain.Entities.Product;
using Sales.Domain.IRepositories;

namespace Sales.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<PagedResponse<CategoryDto>> GetPagedCategoriesAsync(PagedRequest request)
        {
            var query = _unitOfWork.Repository<Category>().GetQueryable();

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

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhóm sản phẩm với Id: {id}");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            // Check if code already exists
            var existing = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Code == dto.Code);
            if (existing != null)
            {
                throw new ArgumentException($"Mã nhóm sản phẩm '{dto.Code}' đã tồn tại.");
            }

            var category = _mapper.Map<Category>(dto);
            category.Id = Guid.NewGuid();
            category.Status = true;
            category.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Category>().InsertAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, CreateCategoryDto dto)
        {
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhóm sản phẩm với Id: {id}");
            }

            // Check if code already exists for another category
            var existing = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Code == dto.Code && c.Id != id);
            if (existing != null)
            {
                throw new ArgumentException($"Mã nhóm sản phẩm '{dto.Code}' đã được sử dụng bởi nhóm khác.");
            }

            _mapper.Map(dto, category);
            category.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhóm sản phẩm với Id: {id}");
            }

            // Check if there are any products in this category
            var productsCount = await _unitOfWork.Repository<Product>().CountAsync(p => p.CategoryId == id);
            if (productsCount > 0)
            {
                throw new InvalidOperationException("Không thể xóa nhóm sản phẩm này vì vẫn còn sản phẩm đang thuộc nhóm.");
            }

            _unitOfWork.Repository<Category>().Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
