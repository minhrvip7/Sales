using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Unit;
using Sales.Application.DTOs.Common;

namespace Sales.Application.IServices
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> GetAllUnitsAsync();
        Task<PagedResponse<UnitDto>> GetPagedUnitsAsync(PagedRequest request);
        Task<UnitDto> GetUnitByIdAsync(Guid id);
        Task<UnitDto> CreateUnitAsync(CreateUnitDto dto);
        Task<bool> UpdateUnitAsync(Guid id, CreateUnitDto dto);
        Task<bool> DeleteUnitAsync(Guid id);
    }
}
