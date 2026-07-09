# Implementation: Pagination (Frontend Spoke)

## Checklist triển khai Frontend

### 1. Core / Types
- [ ] Bổ sung/Cập nhật file `src/types/common.ts`: Thêm interface `PagedRequest` và `PagedResponse<T>`.
- [ ] Bổ sung component dùng chung `PaginatedSelect` vào `src/components/` dựa theo hướng dẫn trong SKILL Virtualized Select.

### 2. Services (Axios)
- [ ] Cập nhật hàm gọi API danh sách (ví dụ: `getCategories`, `getProducts`, `getOrders`, `getUnits`) thành dạng có nhận parameter `PagedRequest`.

### 3. Table Views (Cập nhật phân trang)
- [ ] Cập nhật trang danh sách **Category**: thêm logic state (`pageNumber`, `pageSize`, `totalRecords`, `keyword`), truyền vào thuộc tính `pagination` của `<Table>`.
- [ ] Cập nhật trang danh sách **Product**: tương tự trên.
- [ ] Cập nhật trang danh sách **Order**: tương tự trên.
- [ ] Cập nhật trang danh sách **Unit**: tương tự trên.

### 4. Form Views (Sử dụng Virtualized Select)
- [ ] Cập nhật form tạo/sửa **Product**: thay thế thẻ `<Select>` cho Category, Unit bằng `<PaginatedSelect>`.
- [ ] Cập nhật form tạo/sửa **Order** (nếu có sử dụng Select sản phẩm/khách hàng): thay thế bằng `<PaginatedSelect>`.

### 5. Verification
- [ ] Chạy `npm run build` thành công.
- [ ] Test UI danh sách (kiểm tra phân trang số lượng 20/trang, chuyển trang thành công).
- [ ] Test Dropdown (kiểm tra cuộn xuống tự load thêm trang).
