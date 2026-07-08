# Kỹ năng 3: Triển khai Logic Nghiệp vụ & Validation (Service Layer)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn chi tiết cách viết mã nguồn cho tầng Service nghiệp vụ để xử lý kiểm tra duy nhất mã vạch chéo bảng, khóa đơn vị tính cơ bản khi đã phát sinh hóa đơn và tính toán quy đổi tồn kho.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Viết logic kiểm tra trùng lặp Barcode chéo bảng (Unique Cross-table Check)
*   **Mô tả:** Đảm bảo mã vạch của sản phẩm (Base Unit) và mã vạch của UOM quy đổi (Alternative Unit) không bị trùng lặp trên toàn hệ thống.

> [!TIP]
> **Ví dụ cụ thể:** Thêm hàm kiểm tra bất đồng bộ trong `ProductService.cs`:
> ```csharp
> private async Task ValidateBarcodeUniqueAsync(string? barcode, Guid? currentProductId = null)
> {
>     if (string.IsNullOrEmpty(barcode)) return;
> 
>     // 1. Kiểm tra bảng Products (Đơn vị tính cơ bản)
>     var duplicateProduct = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
>         p => p.Barcode == barcode && (currentProductId == null || p.Id != currentProductId.Value));
>     if (duplicateProduct != null)
>     {
>         throw new ArgumentException($"Mã vạch '{barcode}' đã tồn tại trên sản phẩm '{duplicateProduct.Name}'.");
>     }
> 
>     // 2. Kiểm tra bảng ProductUnitConversions (Đơn vị tính quy đổi)
>     var duplicateConversion = await _unitOfWork.Repository<ProductUnitConversion>().FirstOrDefaultAsync(
>         c => c.Barcode == barcode && (currentProductId == null || c.ProductId != currentProductId.Value),
>         includeProperties: "Product");
>     if (duplicateConversion != null)
>     {
>         throw new ArgumentException($"Mã vạch '{barcode}' đã tồn tại ở đơn vị quy đổi của sản phẩm '{duplicateConversion.Product.Name}'.");
>     }
> }
> ```

---

### Bước 2: Triển khai cơ chế Khóa tính toàn vẹn (Data Integrity Lock)
*   **Mô tả:** Chặn không cho sửa đổi các cấu hình cốt lõi nếu thực thể đã phát sinh liên kết lịch sử hóa đơn.

> [!TIP]
> **Ví dụ cụ thể:** Khóa thay đổi `BaseUnitId` trong hàm `UpdateProductAsync` của `ProductService.cs`:
> ```csharp
> public async Task<bool> UpdateProductAsync(Guid id, CreateProductDto dto)
> {
>     var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == id);
>     
>     // Nếu người dùng thay đổi đơn vị cơ bản
>     if (product.BaseUnitId != dto.BaseUnitId)
>     {
>         // Kiểm tra xem sản phẩm đã có mặt trong đơn hàng nào chưa
>         var hasTransactions = await _unitOfWork.Repository<OrderDetail>().CountAsync(od => od.ProductId == id) > 0;
>         if (hasTransactions)
>         {
>             throw new InvalidOperationException("Không thể thay đổi Đơn vị tính cơ bản vì sản phẩm đã phát sinh giao dịch tồn kho.");
>         }
>     }
>     
>     // ... thực hiện update
> }
> ```

---

### Bước 3: Triển khai thuật toán quy đổi tồn kho khi Bán hàng và Hủy đơn
*   **Mô tả:** Mọi tính toán tồn kho thực tế đều dựa trên đơn vị tính cơ bản. Khi giao dịch bằng đơn vị quy đổi, hệ thống bắt buộc nhân với hệ số để có số lượng tồn kho tương ứng.

> [!TIP]
> **Ví dụ cụ thể (Tạo đơn hàng):** Trong `OrderService.CreateOrderAsync`:
> ```csharp
> // Xác định hệ số quy đổi
> decimal conversionRate = 1;
> decimal unitPrice = product.Price;
> 
> if (itemDto.UnitId == product.BaseUnitId)
> {
>     conversionRate = 1;
>     unitPrice = product.Price;
> }
> else
> {
>     var conversion = product.Conversions.FirstOrDefault(c => c.AlternativeUnitId == itemDto.UnitId);
>     conversionRate = conversion.ConversionRate;
>     unitPrice = conversion.Price ?? (product.Price * conversion.ConversionRate); // Giá ghi đè hoặc tự nhân theo hệ số
> }
> 
> // Quy đổi số lượng khách đặt sang đơn vị cơ bản
> int requiredBaseQty = (int)Math.Round(itemDto.Quantity * conversionRate);
> 
> // Kiểm tra tồn kho cơ bản
> if (product.StockQuantity < requiredBaseQty)
> {
>     throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ hàng trong kho (Còn {product.StockQuantity}, yêu cầu {requiredBaseQty}).");
> }
> 
> // Trừ kho theo đơn vị cơ bản
> product.StockQuantity -= requiredBaseQty;
> ```

> [!TIP]
> **Ví dụ cụ thể (Hủy đơn hàng):** Trong `OrderService.CancelOrderAsync`:
> ```csharp
> // Cộng trả lại kho theo số lượng đã quy đổi ra đơn vị cơ bản
> foreach (var detail in order.OrderDetails)
> {
>     var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == detail.ProductId);
>     if (product != null)
>     {
>         int restoreQty = (int)Math.Round(detail.Quantity * detail.ConversionRate);
>         product.StockQuantity += restoreQty;
>         _unitOfWork.Repository<Product>().Update(product);
>     }
> }
> ```
