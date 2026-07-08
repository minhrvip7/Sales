# Kỹ năng 4: Xử lý Logic Giỏ hàng POS & Đổi đơn vị tính (State Management)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn cách quản lý giỏ hàng POS, tự động cập nhật đơn giá dòng dựa trên chính sách giá sỉ/lẻ của UOM, và kiểm tra tồn kho quy đổi khi thay đổi số lượng.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Quản lý giỏ hàng bằng React State và Tính toán Tổng tiền
*   **Mô tả:** Sử dụng state mảng lưu trữ danh sách sản phẩm trong giỏ hàng. Thực hiện tính toán lại tổng tiền hóa đơn mỗi lần state của giỏ hàng cập nhật.

### Bước 2: Thiết lập sự kiện thay đổi Đơn vị tính (Unit Change)
*   **Mô tả:** Khi thay đổi đơn vị tính trong giỏ:
    1.  Tìm kiếm hệ số quy đổi và giá ghi đè tương ứng của UOM đó.
    2.  Tính toán đơn giá mới: ưu tiên giá ghi đè, nếu không có giá ghi đè thì nhân theo hệ số quy đổi ($Giá cơ bản \times Hệ số$).
    3.  Kiểm tra xem số lượng yêu cầu quy đổi có vượt quá tồn kho khả dụng của sản phẩm không ($Quantity \times ConversionRate > StockQuantity$). Nếu vượt quá, tự động giảm số lượng mua xuống mức tối đa được phép trong UOM đó.

> [!TIP]
> **Ví dụ cụ thể:** Hàm `handleUnitChange` cập nhật giỏ hàng khi người dùng thay đổi dropdown đơn vị tính tại tệp [CreateOrder.tsx](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/features/order/CreateOrder.tsx):
> ```typescript
> const handleUnitChange = (productId: string, unitId: string) => {
>   setSelectedItems(selectedItems.map(item => {
>     if (item.key !== productId) return item;
> 
>     const product = item.product;
>     let conversionRate = 1;
>     let unitPrice = product.price;
> 
>     // 1. Xác định hệ số và đơn giá mới
>     if (unitId === product.baseUnitId) {
>       conversionRate = 1;
>       unitPrice = product.price;
>     } else {
>       const conv = product.conversions?.find(c => c.alternativeUnitId == unitId);
>       if (conv) {
>         conversionRate = conv.conversionRate;
>         // Ưu tiên giá bán riêng, nếu không có thì nhân theo hệ số
>         unitPrice = conv.price !== null && conv.price !== undefined ? conv.price : (product.price * conv.conversionRate);
>       }
>     }
> 
>     // 2. Kiểm tra giới hạn tồn kho cơ bản
>     const requiredBaseQty = item.quantity * conversionRate;
>     if (requiredBaseQty > product.stockQuantity) {
>       const maxQty = Math.floor(product.stockQuantity / conversionRate);
>       if (maxQty <= 0) {
>         message.error(`Không đủ tồn kho để chuyển sang đơn vị này (Tồn kho: ${product.stockQuantity} ${product.baseUnit?.name || 'Lon'}).`);
>         return item; // Giữ nguyên đơn vị cũ
>       }
>       message.warning(`Số lượng được điều chỉnh giảm xuống ${maxQty} để vừa với tồn kho tối đa.`);
>       return {
>         ...item,
>         selectedUnitId: unitId,
>         conversionRate,
>         unitPrice,
>         quantity: maxQty
>       };
>     }
> 
>     // 3. Cập nhật dòng sản phẩm trong giỏ hàng
>     return {
>       ...item,
>       selectedUnitId: unitId,
>       conversionRate,
>       unitPrice
>     };
>   }));
> };
> ```
