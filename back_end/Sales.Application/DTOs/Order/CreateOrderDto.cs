using System;
using System.Collections.Generic;

namespace Sales.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public string? Notes { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
    }
}

