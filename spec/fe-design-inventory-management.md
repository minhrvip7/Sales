# Frontend Design: Quản lý Tồn kho (Inventory Management)

**Tài liệu tham khảo:** spec-inventory-management.md, design-inventory-management.md

## 1. Cấu trúc Route & Pages

### `/inventory` (Màn hình tổng quan tồn kho)
- **Mục đích:** Xem số dư tồn kho của các sản phẩm.
- **Layout:**
  - Tiêu đề: "Quản lý Tồn kho"
  - Bộ lọc (Filter): Tìm kiếm theo Tên SP, Mã SP.
  - Component: `Antd Table`.
    - Các cột: Mã SP, Tên SP, Đơn vị cơ bản, Giá vốn (Moving Average Cost), Tồn kho thực tế (On-hand), Hàng giữ chỗ (Allocated), Tồn kho khả dụng (Available).
  - Action: Nút "Kiểm kê / Điều chỉnh", Nút "Xem thẻ kho".

### `/inventory/transactions/:productId` (Màn hình Thẻ kho)
- **Mục đích:** Xem lịch sử giao dịch (thẻ kho) của một sản phẩm.
- **Layout:**
  - Dùng `Antd Modal` hoặc trang riêng (Page) tuỳ vào thiết kế UI chung.
  - Bộ lọc: Từ ngày - Đến ngày, Loại giao dịch (Inbound, Outbound, Adjustment...).
  - Component: `Antd Table`.
    - Các cột: Ngày giao dịch, Loại giao dịch, Mã tham chiếu, Đơn vị thao tác, Số lượng (Thao tác), Số lượng (Quy đổi), Lý do.

### `/inventory/adjustments` (Quản lý Phiếu Điều chỉnh)
- **Mục đích:** Danh sách các phiếu kiểm kê / điều chỉnh kho.
- **Component:** `Antd Table` hiển thị danh sách phiếu (Mã phiếu, Trạng thái, Ngày tạo, Người tạo).

### `/inventory/adjustments/create` (Tạo Phiếu Kiểm kê/Điều chỉnh)
- **Mục đích:** Snapshot tồn kho hiện tại và nhập số liệu đếm được.
- **Layout:**
  - Form Master: Ghi chú, Lý do kiểm kê.
  - Table Detail: (Editable Table)
    - Chọn sản phẩm (Lookup component / Select).
    - Hiển thị On-hand hệ thống (Readonly).
    - Cột nhập `Counted Qty` (InputNumber).
    - Cột tính tự động `Variance` (Chênh lệch) và gợi ý loại điều chỉnh (Thừa/Thiếu).
  - Actions: Save Draft, Confirm (Chốt phiếu điều chỉnh).

## 2. API Call Map (Dự kiến với Axios)
Tạo file `src/services/inventoryService.ts` chứa các API call sau:
- `GET /api/inventory/balances` -> Fetch list `InventoryBalanceDto`
- `GET /api/inventory/transactions/{productId}` -> Fetch list `InventoryTransactionDto`
- `POST /api/inventory/adjustments` -> Create `CreateInventoryAdjustmentDto`
- `PUT /api/inventory/adjustments/{id}/confirm` -> Confirm phiếu điều chỉnh
- `GET /api/inventory/costs` -> Fetch list `ProductCostDto`

## 3. UI Components & Variant
- Sử dụng `Tag` của Antd để hiển thị trạng thái của Phiếu Điều chỉnh:
  - Draft: `color="default"`
  - Completed: `color="success"`
  - Cancelled: `color="error"`
- **Quét Barcode:** Tại tính năng xuất kho bán hàng (ở module Sales), ô Input Barcode sẽ lắng nghe sự kiện Enter từ máy quét, gọi API tra cứu Sản phẩm/Đơn vị theo Barcode và tự động append vào giỏ hàng xuất với `Transacted_Qty = 1`.

## 4. State Management (Redux/Zustand)
- State `inventoryBalances`: Lưu cache list để filter nhanh.
- State `currentAdjustment`: Quản lý giỏ hàng/danh sách chi tiết trong quá trình tạo phiếu điều chỉnh (vì có editable table).
