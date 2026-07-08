using System;
using System.Collections.Generic;

namespace Evo.Sales.Domain.Entities.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public string? Notes { get; set; }
        
        // 0: Draft, 1: Confirmed, 2: Completed, 3: Cancelled
        public int OrderStatus { get; set; } = 0; 
        
        // 0: Unpaid, 1: Partially Paid, 2: Fully Paid
        public int PaymentStatus { get; set; } = 0;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        // Relationships
        public virtual Customer.Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
