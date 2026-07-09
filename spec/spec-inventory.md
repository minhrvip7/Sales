# Spec: Quản lý Tồn kho (Inventory Management)

## 1. Bối cảnh
- Tính năng: Quản lý Tồn kho (Inventory Management).
- Input: `PRD-INV-001`.
- Mục tiêu: Quản lý chính xác số lượng tồn kho theo 3 trạng thái, xử lý xuất/nhập/kiểm kê, tuân thủ chặt chẽ cấu trúc UOM của Product, và đáp ứng tiêu chuẩn ISMS.

## 2. Yêu cầu Hệ thống (System Requirements)
### 2.1. Quản lý trạng thái tồn kho
Cần theo dõi 3 trạng thái cho mỗi sản phẩm (lưu trữ theo Đơn vị cơ bản - Base Unit):
- `OnHandQuantity`: Tồn kho vật lý thực tế.
- `AllocatedQuantity`: Tồn kho đã giữ chỗ (nằm trong Sales Order đã xác nhận nhưng chưa xuất).
- `AvailableQuantity`: Tồn kho khả dụng (= `OnHandQuantity` - `AllocatedQuantity`).

### 2.2. Các luồng nghiệp vụ cốt lõi
1. **Nhập kho (Goods Receipt)**: 
   - Tăng `OnHandQuantity`.
   - Cập nhật giá vốn (`Cost`) theo thuật toán Moving Average.
2. **Xuất kho (Goods Issue)**:
   - Giảm `OnHandQuantity`, trừ `AllocatedQuantity` tương ứng.
   - Chặn xuất âm (Block negative inventory).
3. **Kiểm kê & Điều chỉnh (Stocktake & Adjustment)**:
   - Tạo snapshot tồn kho hệ thống.
   - Nhập tồn kho đếm thực tế -> tính `Variance`.
   - Sinh Phiếu Nhập Điều chỉnh (thừa) hoặc Xuất Điều chỉnh (thiếu).

### 2.3. Quy tắc Đơn vị tính (UOM Rules)
Bảng lịch sử giao dịch (`InventoryTransactions`) phải lưu 3 thông tin:
- `TransactedQty`: Số lượng lúc giao dịch.
- `TransactedUOMId`: Đơn vị tính lúc giao dịch.
- `BaseQty`: Số lượng quy đổi ra đơn vị cơ bản (cộng/trừ vào kho).

### 2.4. Tiêu chuẩn ISMS
- Không xóa cứng (Hard Delete) chứng từ (áp dụng Soft Delete).
- Bắt buộc dùng Phiếu điều chỉnh ngược (Reversal) nếu sai sót sau khi chứng từ đã hoàn tất.
- Lưu trữ log Audit (CreatedBy, CreatedDate).
