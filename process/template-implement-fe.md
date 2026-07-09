# Template: `implement-fe-{module}-{feature}.md` (Frontend spoke)

**Mục đích**: Chứa checklist cụ thể cho phần Frontend Implementation (React + Vite + Ant Design + Redux).

**Áp dụng**: Thực thi ở Phase 3b. Session FE chỉ load file này + spec + FE design + API Specs (hoặc thư mục API).

---

# {Feature Name} — Frontend Implementation (Phase 3b)

> **Hub:** [`implement-{module}-{feature}.md`](./implement-{module}-{feature}.md)
> **Spec:** [`docs/spec/{module}/spec-{module}-{feature}.md`](../../spec/{module}/spec-{module}-{feature}.md)
> **FE Design:** [`docs/design/{module}/fe-design-{module}-{feature}.md`](../../design/{module}/fe-design-{module}-{feature}.md)
> **Status:** 🚧 Phase 1 🚧 Phase 2 ... 🚧 Phase {N}

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
| 1 | API Integration (Axios/RTK) | `src/api` hoặc `src/store` |
| 2 | Routing & Menu | `src/routes` |
| 3 | List Page (Antd Table) | `src/pages/{Feature}/List` |
| 4 | Form Create/Edit (Antd Modal/Form) | `src/pages/{Feature}/Form` |
| 5 | UI Polish & Integration | `src/pages` |

---

## Phase 1 — API Integration (Axios/Redux Toolkit)
**Mục tiêu**: Chuẩn bị kết nối với Backend API.
- [ ] Dựa vào endpoint đã định nghĩa bên backend (ví dụ `/api/orders`), tạo file `api/{Entity}Api.ts`.
- [ ] Viết các hàm axios (`getAll`, `getById`, `create`, `update`, `delete`).
- [ ] Định nghĩa các Types/Interfaces Typescript (tương đương DTO bên BE).
- [ ] (Tuỳ chọn) Setup Redux Slice nếu cần thiết.

## Phase 2 — Routing & Menu
**Mục tiêu**: Tạo đường dẫn URL cho trang.
- [ ] Tạo file rỗng `{Entity}Page.tsx` trong `src/pages/{Entity}/`.
- [ ] Khai báo route trong hệ thống Router (ví dụ `react-router-dom`).
- [ ] Thêm mục menu (Antd Menu) tương ứng cho tính năng này.
- [ ] Chạy dev server kiểm tra chuyển trang thành công.

## Phase 3 — List Page (Antd Table)
**Mục tiêu**: Hiển thị dữ liệu danh sách có phân trang và filter.
- [ ] Tạo Component `{Entity}List.tsx`.
- [ ] Sử dụng `useEffect` (hoặc RTK Query) gọi API `getAll`.
- [ ] Render component `Table` của Ant Design. Khai báo các cột (Columns) đúng như FE Design.
- [ ] Xử lý Phân trang (Pagination) và Lọc (Filter).

## Phase 4 — Form Create/Edit (Antd Modal/Form)
**Mục tiêu**: Giao diện thêm mới và cập nhật.
- [ ] Tạo Component `{Entity}Form.tsx` (có thể bọc trong Modal hoặc là trang riêng).
- [ ] Khởi tạo `Form` của Ant Design. 
- [ ] Thêm các `Form.Item` với rules validation tương ứng từ Spec.
- [ ] Map các API Lookups (danh sách Category, Unit,...) vào dropdown `Select` của Antd.
- [ ] Xử lý logic gọi API `create` hoặc `update` khi submit form. Xử lý hiển thị Notification success/error.

## Phase 5 — UI Polish & Integration
**Mục tiêu**: Rà soát, làm mịn UX/UI.
- [ ] Xử lý hiển thị các badge Trạng thái (Ví dụ Status 1 -> Màu xanh, 0 -> Màu xám).
- [ ] Bổ sung Skeleton hoặc Spin (Loading state) khi đợi call API.
- [ ] Xóa code thừa, console.log.
- [ ] **Đổi status Phase này và toàn bộ track thành ✅. Cập nhật lại Hub file.**

---

## ❓ Open Questions (Frontend Only)
- **Q1**: ...
