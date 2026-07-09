using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Sales.Application.DTOs.Product;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sales.Domain.Entities.Product;
using Sales.Domain.Entities.Order;
using Sales.Domain.IRepositories;

namespace Sales.Application.Services
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
                includeProperties: "Category,BaseUnit,Conversions.AlternativeUnit");
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            foreach (var dto in dtos)
            {
                dto.HasTransactions = await _unitOfWork.Repository<OrderDetail>().CountAsync(od => od.ProductId == dto.Id) > 0;
            }
            return dtos;
        }

        public async Task<PagedResponse<ProductDto>> GetPagedProductsAsync(PagedRequest request)
        {
            var query = _unitOfWork.Repository<Product>().GetQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword) || x.Code.Contains(request.Keyword) || x.Barcode.Contains(request.Keyword));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .Include("Category")
                .Include("BaseUnit")
                .Include("Conversions.AlternativeUnit")
                .OrderByDescending(x => x.CreatedDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ProductDto>>(data);
            foreach (var dto in dtos)
            {
                dto.HasTransactions = await _unitOfWork.Repository<OrderDetail>().CountAsync(od => od.ProductId == dto.Id) > 0;
            }

            return new PagedResponse<ProductDto>(dtos, totalRecords, request.PageNumber, request.PageSize);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
                filter: p => p.Id == id,
                includeProperties: "Category,BaseUnit,Conversions.AlternativeUnit");

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {id}");
            }

            var dto = _mapper.Map<ProductDto>(product);
            dto.HasTransactions = await _unitOfWork.Repository<OrderDetail>().CountAsync(od => od.ProductId == id) > 0;
            return dto;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            // Verify Category exists
            var category = await _unitOfWork.Repository<Category>().FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Không tìm thấy nhóm sản phẩm với Id: {dto.CategoryId}");
            }

            // Verify Base Unit exists
            var baseUnit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == dto.BaseUnitId);
            if (baseUnit == null)
            {
                throw new ArgumentException($"Không tìm thấy đơn vị tính với Id: {dto.BaseUnitId}");
            }

            // Validate Barcode uniqueness
            await ValidateBarcodeUniqueAsync(dto.Barcode);

            var product = _mapper.Map<Product>(dto);
            
            // Validate and map conversions
            var conversions = new List<ProductUnitConversion>();
            var seenUnits = new HashSet<Guid>();
            foreach (var convDto in dto.Conversions)
            {
                if (convDto.AlternativeUnitId == dto.BaseUnitId)
                {
                    throw new ArgumentException("Đơn vị quy đổi không được trùng với đơn vị cơ bản.");
                }
                if (seenUnits.Contains(convDto.AlternativeUnitId))
                {
                    throw new ArgumentException("Các đơn vị quy đổi của sản phẩm không được trùng lặp.");
                }
                seenUnits.Add(convDto.AlternativeUnitId);
                if (convDto.ConversionRate <= 0)
                {
                    throw new ArgumentException("Hệ số quy đổi phải lớn hơn 0.");
                }

                // Verify alternative unit exists
                var altUnit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == convDto.AlternativeUnitId);
                if (altUnit == null)
                {
                    throw new ArgumentException($"Không tìm thấy đơn vị quy đổi với Id: {convDto.AlternativeUnitId}");
                }

                await ValidateBarcodeUniqueAsync(convDto.Barcode);

                conversions.Add(new ProductUnitConversion
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    AlternativeUnitId = convDto.AlternativeUnitId,
                    ConversionRate = convDto.ConversionRate,
                    Barcode = convDto.Barcode,
                    Price = convDto.Price
                });
            }

            product.Conversions = conversions;

            await _unitOfWork.Repository<Product>().InsertAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Load relationships for mapped response
            product.Category = category;
            product.BaseUnit = baseUnit;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateProductAsync(Guid id, CreateProductDto dto)
        {
            var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
                filter: p => p.Id == id,
                includeProperties: "Conversions");
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

            // Verify Base Unit exists
            var baseUnit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == dto.BaseUnitId);
            if (baseUnit == null)
            {
                throw new ArgumentException($"Không tìm thấy đơn vị tính với Id: {dto.BaseUnitId}");
            }

            // Lock check for Base Unit change
            if (product.BaseUnitId != dto.BaseUnitId)
            {
                var hasTransactions = await _unitOfWork.Repository<OrderDetail>().CountAsync(od => od.ProductId == id) > 0;
                if (hasTransactions)
                {
                    throw new InvalidOperationException("Không thể thay đổi Đơn vị tính cơ bản vì sản phẩm đã phát sinh giao dịch tồn kho.");
                }
            }

            // Validate Barcode uniqueness
            await ValidateBarcodeUniqueAsync(dto.Barcode, id);

            // Clear existing conversions in DbContext
            var existingConversions = await _unitOfWork.Repository<ProductUnitConversion>().GetAsync(c => c.ProductId == id);
            if (existingConversions != null && existingConversions.Any())
            {
                _unitOfWork.Repository<ProductUnitConversion>().DeleteRange(existingConversions);
            }

            // Validate and map new conversions
            var conversions = new List<ProductUnitConversion>();
            var seenUnits = new HashSet<Guid>();
            foreach (var convDto in dto.Conversions)
            {
                if (convDto.AlternativeUnitId == dto.BaseUnitId)
                {
                    throw new ArgumentException("Đơn vị quy đổi không được trùng với đơn vị cơ bản.");
                }
                if (seenUnits.Contains(convDto.AlternativeUnitId))
                {
                    throw new ArgumentException("Các đơn vị quy đổi của sản phẩm không được trùng lặp.");
                }
                seenUnits.Add(convDto.AlternativeUnitId);
                if (convDto.ConversionRate <= 0)
                {
                    throw new ArgumentException("Hệ số quy đổi phải lớn hơn 0.");
                }

                // Verify alternative unit exists
                var altUnit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == convDto.AlternativeUnitId);
                if (altUnit == null)
                {
                    throw new ArgumentException($"Không tìm thấy đơn vị quy đổi với Id: {convDto.AlternativeUnitId}");
                }

                await ValidateBarcodeUniqueAsync(convDto.Barcode, id);

                conversions.Add(new ProductUnitConversion
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    AlternativeUnitId = convDto.AlternativeUnitId,
                    ConversionRate = convDto.ConversionRate,
                    Barcode = convDto.Barcode,
                    Price = convDto.Price
                });
            }

            _mapper.Map(dto, product);
            product.UpdatedDate = DateTime.UtcNow;
            product.Conversions = conversions;

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

            // Soft-delete: đánh dấu IsĐeleted = true thay vì xóa vật lý
            // Dữ liệu vẫn được giữ lại để hiển thị trong lịch sử đơn hàng và báo cáo
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task ValidateBarcodeUniqueAsync(string? barcode, Guid? currentProductId = null)
        {
            if (string.IsNullOrEmpty(barcode)) return;

            // Check in Products table (base unit)
            var duplicateProduct = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
                p => p.Barcode == barcode && (currentProductId == null || p.Id != currentProductId.Value));
            if (duplicateProduct != null)
            {
                throw new ArgumentException($"Mã vạch '{barcode}' đã tồn tại trên sản phẩm '{duplicateProduct.Name}'.");
            }

            // Check in ProductUnitConversions table (alternative units)
            var duplicateConversion = await _unitOfWork.Repository<ProductUnitConversion>().FirstOrDefaultAsync(
                c => c.Barcode == barcode && (currentProductId == null || c.ProductId != currentProductId.Value),
                includeProperties: "Product");
            if (duplicateConversion != null)
            {
                throw new ArgumentException($"Mã vạch '{barcode}' đã tồn tại ở đơn vị quy đổi của sản phẩm '{duplicateConversion.Product.Name}'.");
            }
        }
    }
}

