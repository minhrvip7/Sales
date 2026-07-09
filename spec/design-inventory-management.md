# Backend Design: Quản lý Tồn kho (Inventory Management)

**Tài liệu tham khảo:** spec-inventory-management.md

## 1. Enums (`Sales.Domain/Enums/`)

### `TransactionType.cs`
```csharp
/// <summary>Các loại giao dịch kho.</summary>
public enum TransactionType
{
    /// <summary>Nhập kho từ nhà cung cấp (Purchase Receipt).</summary>
    Inbound = 1,
    /// <summary>Xuất kho bán hàng (Sales Issue).</summary>
    Outbound = 2,
    /// <summary>Nhập kho điều chỉnh (Thừa).</summary>
    AdjustmentIn = 3,
    /// <summary>Xuất kho điều chỉnh (Thiếu).</summary>
    AdjustmentOut = 4,
    /// <summary>Các loại xuất kho khác (Hủy, tiêu hao).</summary>
    OtherIssue = 5
}
```

### `AdjustmentStatus.cs`
```csharp
/// <summary>Trạng thái của phiếu điều chỉnh kho.</summary>
public enum AdjustmentStatus
{
    /// <summary>Nháp, chưa ảnh hưởng tồn kho.</summary>
    Draft = 0,
    /// <summary>Hoàn tất, đã cập nhật tồn kho.</summary>
    Completed = 1,
    /// <summary>Đã hủy.</summary>
    Cancelled = 2
}
```

## 2. Entities (`Sales.Domain/Entities/Inventory/`)

### `InventoryBalance.cs`
```csharp
/// <summary>Số dư tồn kho của sản phẩm.</summary>
public class InventoryBalance : ISoftDelete
{
    public Guid Id { get; set; }
    /// <summary>FK → Products</summary>
    public Guid ProductId { get; set; }
    
    /// <summary>Số lượng vật lý đang có trong kho.</summary>
    public int OnHandQty { get; set; }
    
    /// <summary>Số lượng hàng đã giữ chỗ cho các đơn hàng Confirmed.</summary>
    public int AllocatedQty { get; set; }
    
    /// <summary>Số lượng khả dụng (Available = OnHandQty - AllocatedQty). Có thể config dạng computed column hoặc field được tính toán.</summary>
    public int AvailableQty { get; set; }

    // Audit & ISoftDelete properties...
}
```

### `InventoryTransaction.cs`
```csharp
/// <summary>Lịch sử giao dịch kho (Thẻ kho).</summary>
public class InventoryTransaction : ISoftDelete
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    
    /// <summary>Loại giao dịch.</summary>
    public TransactionType Type { get; set; }
    
    /// <summary>Mã chứng từ liên quan (PO, SO, AdjustmentTicket).</summary>
    public string ReferenceNumber { get; set; } = string.Empty;
    
    /// <summary>Số lượng giao dịch theo đơn vị lúc thao tác.</summary>
    public int TransactedQty { get; set; }
    
    /// <summary>FK → Units. Đơn vị tính lúc thao tác.</summary>
    public Guid TransactedUomId { get; set; }
    
    /// <summary>Số lượng quy đổi tuyệt đối cộng/trừ vào kho theo đơn vị cơ bản.</summary>
    public int BaseQty { get; set; }
    
    /// <summary>Lý do (Reversal, Note).</summary>
    public string? Reason { get; set; }

    // Audit & ISoftDelete properties...
}
```

### `ProductCost.cs`
```csharp
/// <summary>Lịch sử giá vốn bình quân gia quyền của sản phẩm.</summary>
public class ProductCost : ISoftDelete
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    
    /// <summary>Giá vốn bình quân theo đơn vị cơ bản tại thời điểm tính.</summary>
    public decimal MovingAverageCost { get; set; }
    
    /// <summary>Ngày giờ hiệu lực của giá vốn này.</summary>
    public DateTime EffectiveDate { get; set; }

    // Audit & ISoftDelete properties...
}
```

### `InventoryAdjustment.cs` & `InventoryAdjustmentDetail.cs`
(Thiết kế Master-Detail cho phiếu kiểm kê & điều chỉnh. Lưu `AdjustmentStatus`, `Reason`, v.v.)

## 3. DTOs (`Sales.Application/DTOs/Inventory/`)
- `InventoryBalanceDto`
- `InventoryTransactionDto`
- `ProductCostDto`
- `CreateInventoryAdjustmentDto`, `InventoryAdjustmentDto`, `InventoryAdjustmentDetailDto`

## 4. App Services (`Sales.Application/Interfaces/`)
### `IInventoryService.cs`
- `GetBalanceAsync(Guid productId)`
- `ProcessInboundAsync(InboundRequestDto request)`: Cập nhật OnHand, tạo Transaction, cập nhật Moving Average Cost.
- `ProcessOutboundAsync(OutboundRequestDto request)`: Check Available âm, trừ OnHand, trừ Allocated (nếu từ SO), tạo Transaction.
- `AllocateInventoryAsync(Guid productId, int qty)`: Tăng Allocated khi SO Confirmed.

## 5. EF Core Configuration (`Sales.Infrastructure/Data/Config/`)
- Bảng `InventoryTransactions`: HasComment("Bảng thẻ kho ghi nhận lịch sử giao dịch").
- Cột `MovingAverageCost` trong `ProductCosts`: `.HasPrecision(18, 2)`.
- Cấu hình Enum `HasConversion<int>()`.
- Tự động Migration: `AddInventoryManagementSystem`.

## 6. API Controllers (`Sales.Api/Controllers/`)
- `InventoryController` (GetBalances, GetTransactions)
- `InventoryAdjustmentController` (Create, Confirm, Cancel)
