using System;
using System.Collections.Generic;

namespace Sales.Domain.Entities.Customer
{
    public class CustomerGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double DiscountPercentage { get; set; } = 0;
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        // Relationships
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}

