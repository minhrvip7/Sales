using System;

namespace Sales.Application.DTOs.Unit
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
