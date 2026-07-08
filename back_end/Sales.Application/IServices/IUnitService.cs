using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Application.DTOs.Unit;

namespace Sales.Application.IServices
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> GetAllUnitsAsync();
        Task<UnitDto> GetUnitByIdAsync(Guid id);
        Task<UnitDto> CreateUnitAsync(CreateUnitDto dto);
        Task<bool> UpdateUnitAsync(Guid id, CreateUnitDto dto);
        Task<bool> DeleteUnitAsync(Guid id);
    }
}
