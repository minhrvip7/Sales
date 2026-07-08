using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Evo.Sales.Application.DTOs.Product;
using Evo.Sales.Application.IServices;
using Evo.Sales.Domain.Entities.Product;
using Evo.Sales.Domain.IRepositories;

namespace Evo.Sales.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>().GetAsync(
                includeProperties: "Category,Unit");
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
                filter: p => p.Id == id,
                includeProperties: "Category,Unit");

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {id}");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            // Verify Category exists
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Không tìm thấy nhóm sản phẩm với Id: {dto.CategoryId}");
            }

            // Verify Unit exists
            var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == dto.UnitId);
            if (unit == null)
            {
                throw new ArgumentException($"Không tìm thấy đơn vị tính với Id: {dto.UnitId}");
            }

            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.Repository<Product>().InsertAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Load relationships for mapped response
            product.Category = category;
            product.Unit = unit;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateProductAsync(Guid id, CreateProductDto dto)
        {
            var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {id}");
            }

            // Verify Category exists
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Không tìm thấy nhóm sản phẩm với Id: {dto.CategoryId}");
            }

            // Verify Unit exists
            var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == dto.UnitId);
            if (unit == null)
            {
                throw new ArgumentException($"Không tìm thấy đơn vị tính với Id: {dto.UnitId}");
            }

            _mapper.Map(dto, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {id}");
            }

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
