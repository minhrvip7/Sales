# Goods Receipt — Frontend Implementation (Phase 3b)

> **Hub:** [`implement-inventory-goods-receipt.md`](./implement-inventory-goods-receipt.md)
> **Spec:** [`../spec-inventory-goods-receipt.md`](../spec-inventory-goods-receipt.md)
> **FE Design:** [`../fe-design-inventory-goods-receipt.md`](../fe-design-inventory-goods-receipt.md)
> **Status:** 🚧 Phase 1 🚧 Phase 2 🚧 Phase 3 🚧 Phase 4 🚧 Phase 5

---

## 🚀 How to start (Frontend session)

1. **Pre-flight check (BẮT BUỘC):** Mở file Hub và đảm bảo **Quality Gate (Phase 3a -> 3b)** đã được đánh dấu ✅. Nếu backend chưa xong, hãy dừng lại.
2. **Chỉ load các file Frontend**: Hub, FE Spoke này, Spec, FE Design.
3. **Tech Stack**: React 19, Redux Toolkit, Axios, Ant Design.
4. **Kiểm tra mỗi phase**: Run `npm run build` hoặc `vite build`. Mở trình duyệt xem UI có render đúng và không có lỗi Console hay không.

---

## Phase map (Frontend track)

| # | Phase | Layer |
|---|---|---|
| 1 | API Integration (Axios/RTK) | `src/api` |
| 2 | Routing & Menu | `src/routes` |
| 3 | List Page (Antd Table) | `src/pages/Inventory/GoodsReceipt` |
| 4 | Form Create/Edit (Antd Modal/Form) | `src/pages/Inventory/GoodsReceipt/Form` |
| 5 | UI Polish & Integration | `src/pages` |

---

## Phase 1 — API Integration (Axios/Redux Toolkit)
- [ ] Tạo file `api/goodsReceiptApi.ts`.
- [ ] Viết các hàm axios (`getAll`, `getById`, `create`, `update`, `complete`, `resolveBarcode`).
- [ ] Định nghĩa các Types/Interfaces Typescript (tương đương DTO bên BE).

## Phase 2 — Routing & Menu
- [ ] Khai báo route trong hệ thống Router. Các đường dẫn: `/inventory/goods-receipt` (List), `/inventory/goods-receipt/create` (Form Create), `/inventory/goods-receipt/:id/edit` (Form Edit).
- [ ] Thêm mục menu Nhập kho tương ứng.

## Phase 3 — List Page (Antd Table)
- [ ] Tạo Component `GoodsReceiptList.tsx`.
- [ ] Sử dụng `useEffect` (hoặc RTK Query) gọi API `getAll`.
- [ ] Render component `Table` của Ant Design.
- [ ] Xử lý Phân trang (Pagination) và Lọc (Filter) bằng state.

## Phase 4 — Form Create/Edit (Antd Modal/Form)
- [ ] Tạo Component `GoodsReceiptForm.tsx`.
- [ ] Sử dụng Layout Grid (`Row`, `Col`), KHÔNG dùng fixed width.
- [ ] Tích hợp tính năng Virtualized Select (Tham khảo skill virtualized-select) cho Dropdown Sản phẩm và Kho.
- [ ] Thêm ô Input nhập / quét mã vạch -> gọi hàm resolveBarcode -> update row table.
- [ ] Hiển thị cảnh báo (Warning) nổi bật ở dòng nào có `ActualQuantity > ExpectedQuantity`.

## Phase 5 — UI Polish & Integration
- [ ] Thêm popup Double-confirm (Antd Modal.confirm) khi user nhấn nút "Hoàn tất" (Post) vì thao tác này không thể hoàn tác.
- [ ] Hiện loading overlay khi submit để tránh user click nhiều lần.
- [ ] **Đổi status Phase này và toàn bộ track thành ✅. Cập nhật lại Hub file.**
