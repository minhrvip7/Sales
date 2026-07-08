using System;

namespace Evo.Sales.Domain.Entities.Order
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public double DiscountPercentage { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; }

        // Relationships
        public virtual Order Order { get; set; } = null!;
        public virtual Product.Product Product { get; set; } = null!;
    }
}
