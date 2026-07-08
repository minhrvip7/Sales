using System;

namespace Evo.Sales.Domain.Entities.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public int StockQuantity { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public Guid UnitId { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        // Relationships
        public virtual Category Category { get; set; } = null!;
        public virtual Unit Unit { get; set; } = null!;
    }
}
