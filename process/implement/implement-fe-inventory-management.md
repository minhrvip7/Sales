# Frontend Implementation: Quản lý Tồn kho (Inventory Management)

**Tài liệu tham khảo:**
- Spec: `../../spec/spec-inventory-management.md`
- Design: `../../spec/fe-design-inventory-management.md`

## Checklist Lập trình Frontend

### 1. Cấu hình Services & Axios
- [ ] Khởi tạo service `inventoryService.ts`.
- [ ] Cài đặt các hàm gọi API endpoint Backend đã dựng: Get Balances, Get Transactions.

### 2. Route & State
- [ ] Đăng ký route `/inventory` cho trang tổng quan tồn kho.
- [ ] Đăng ký route `/inventory/transactions/:productId` cho trang xem thẻ kho chi tiết.
- [ ] Tích hợp React Query / Redux (tuỳ codebase) để cache data hiệu quả.

### 3. Giao diện Tổng quan Tồn kho (`/inventory`)
- [ ] Tạo Component chính `InventoryList`.
- [ ] Dựng bộ lọc: Input Search (theo tên/mã SP).
- [ ] Dựng `Antd Table`: Mã SP, Tên SP, Đơn vị, Giá vốn, On-hand, Allocated, Available.
- [ ] Bổ sung nút bấm "Xem thẻ kho" chuyển hướng hoặc mở Modal.

### 4. Giao diện Thẻ Kho
- [ ] Tạo Component `InventoryTransactionHistory`.
- [ ] Dựng `Antd Table` hiển thị lịch sử: Ngày, Loại giao dịch (hiển thị màu sắc bằng `Tag`), Mã tham chiếu, Số lượng giao dịch, Số lượng quy đổi.

## Quality Gate (Kiểm tra trước khi đóng Session)
- [ ] Lệnh `npm run build` (hoặc vite build) chạy thành công không xuất hiện lỗi syntax.
- [ ] Màn hình UI render mượt mà, không gặp lỗi vòng lặp render hay lỗi văng React Console.
