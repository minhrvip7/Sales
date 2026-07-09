using System;

namespace Sales.Application.DTOs.Inventory
{
    public class ProcessInboundDto
    {
        public Guid ProductId { get; set; }
        public int TransactedQty { get; set; }
        public Guid TransactedUomId { get; set; }
        public int BaseQty { get; set; }
        public decimal UnitCost { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }

    public class ProcessOutboundDto
    {
        public Guid ProductId { get; set; }
        public int TransactedQty { get; set; }
        public Guid TransactedUomId { get; set; }
        public int BaseQty { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public bool IsSalesIssue { get; set; } = true;
    }
}
