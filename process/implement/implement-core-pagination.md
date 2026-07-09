# Kế hoạch Triển khai: Pagination (Hub)

**Document ID:** IMPL-HUB-CORE-PAGINATION
**Spec:** `spec-core-pagination.md`

Đây là file Hub đóng vai trò điều phối giữa Backend và Frontend để triển khai phân trang.

## Trạng thái các luồng (Spokes)

- [ ] **Backend Spoke** (`implement-be-core-pagination.md`): Chưa bắt đầu
- [ ] **Frontend Spoke** (`implement-fe-core-pagination.md`): Chưa bắt đầu

## Hướng dẫn thực thi

1. Mở session AI mới (hoặc tiếp tục) để xử lý **Backend Spoke**.
   - Agent cần đọc: `spec-core-pagination.md` và `design-core-pagination.md`.
   - Quality Gate: Đảm bảo build thành công và Swagger hiển thị đúng endpoint GetPaged.
2. Sau khi Backend xong và Quality Gate Xanh, tiếp tục xử lý **Frontend Spoke**.
   - Agent cần đọc: `spec-core-pagination.md` và `fe-design-core-pagination.md`.
   - Cập nhật Axios, Antd Table, và Virtualized Select.
3. Cuối cùng thực hiện Phase 4 (Test UI) và Phase 5 (Review).
