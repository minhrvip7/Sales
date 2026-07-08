using System;

namespace Sales.Application.DTOs.Order
{
    public class CreateOrderDetailDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double DiscountPercentage { get; set; }
    }
}

