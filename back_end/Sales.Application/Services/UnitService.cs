using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Sales.Application.DTOs.Unit;
using Sales.Application.DTOs.Common;
using Sales.Application.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sales.Domain.Entities.Product;
using Sales.Domain.IRepositories;

namespace Sales.Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitDto>> GetAllUnitsAsync()
        {
            var units = await _unitOfWork.Repository<Unit>().GetAsync();
            return _mapper.Map<IEnumerable<UnitDto>>(units);
        }

        public async Task<PagedResponse<UnitDto>> GetPagedUnitsAsync(PagedRequest request)
        {
            var query = _unitOfWork.Repository<Unit>().GetQueryable();

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

            var dtoList = _mapper.Map<IEnumerable<UnitDto>>(data);

            return new PagedResponse<UnitDto>(dtoList, totalRecords, request.PageNumber, request.PageSize);
        }

        public async Task<UnitDto> GetUnitByIdAsync(Guid id)
        {
            var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == id);
            if (unit == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn vị tính với Id: {id}");
            }
            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<UnitDto> CreateUnitAsync(CreateUnitDto dto)
        {
            // Check if code already exists
            var existing = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Code == dto.Code);
            if (existing != null)
            {
                throw new ArgumentException($"Mã đơn vị tính '{dto.Code}' đã tồn tại.");
            }

            var unit = _mapper.Map<Unit>(dto);
            unit.Id = Guid.NewGuid();
            unit.Status = true;
            unit.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Unit>().InsertAsync(unit);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<bool> UpdateUnitAsync(Guid id, CreateUnitDto dto)
        {
            var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == id);
            if (unit == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn vị tính với Id: {id}");
            }

            // Check if code already exists for another unit
            var existing = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Code == dto.Code && u.Id != id);
            if (existing != null)
            {
                throw new ArgumentException($"Mã đơn vị tính '{dto.Code}' đã được sử dụng bởi đơn vị khác.");
            }

            _mapper.Map(dto, unit);
            unit.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Unit>().Update(unit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUnitAsync(Guid id)
        {
            var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == id);
            if (unit == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn vị tính với Id: {id}");
            }

            // Check if there are any products in this unit as base unit
            var productsCount = await _unitOfWork.Repository<Product>().CountAsync(p => p.BaseUnitId == id);
            if (productsCount > 0)
            {
                throw new InvalidOperationException("Không thể xóa đơn vị tính này vì vẫn còn sản phẩm sử dụng đơn vị tính làm đơn vị cơ bản.");
            }

            // Check if there are any products in this unit as alternative unit
            var conversionsCount = await _unitOfWork.Repository<ProductUnitConversion>().CountAsync(c => c.AlternativeUnitId == id);
            if (conversionsCount > 0)
            {
                throw new InvalidOperationException("Không thể xóa đơn vị tính này vì vẫn còn sản phẩm sử dụng làm đơn vị quy đổi.");
            }

            _unitOfWork.Repository<Unit>().Delete(unit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
