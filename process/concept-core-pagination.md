# Concept: Pagination (Phân trang toàn dự án)

## 1. Yêu cầu nghiệp vụ
- Bổ sung tính năng phân trang (pagination) cho tất cả các màn hình danh sách hiện có trong dự án.
- Mặc định hiển thị: 20 bản ghi trên 1 trang.
- Cho phép người dùng tùy chọn/thay đổi số lượng bản ghi hiển thị trên mỗi trang.

## 2. Phạm vi áp dụng (Scope In/Out)
**Scope In:**
- **Backend:**
  - Tạo cấu trúc request chuẩn: `PagedRequest` chứa `PageNumber`, `PageSize`.
  - Tạo cấu trúc response chuẩn: `PagedResponse<T>` chứa `Data`, `TotalRecords`, `TotalPages`, `PageNumber`, `PageSize`.
  - Nâng cấp các API `GetAll...` thành `GetPaged...` (hoặc thay đổi tham số `GetAll`) trong Controller, Service, Repository cho các domain: `Category`, `Order`, `Product`, `Unit`.
- **Frontend:**
  - Cập nhật Axios clients để gửi kèm `pageNumber` và `pageSize`.
  - Cấu hình lại component `Table` của Ant Design tại các trang danh sách để hiển thị thanh phân trang, kích hoạt sự kiện `onChange` để fetch dữ liệu trang mới.

**Scope Out:**
- Không sửa đổi logic business của các tính năng CRUD.
- Tạm thời không phân trang các API chỉ dùng cho Dropdown/Select (thường trả về toàn bộ dữ liệu nhỏ) nếu không có yêu cầu đặc biệt.

## 3. Các quyết định kỹ thuật
- **Tham số mặc định:** `PageNumber = 1`, `PageSize = 20`.
- **Bảo vệ hệ thống:** Ràng buộc `MaxPageSize` (VD: 100 hoặc 500) để ngăn chặn request lấy hàng triệu bản ghi gây tràn RAM backend.
- **Tính đồng nhất:** Sử dụng Generics `<T>` cho dữ liệu trả về để tái sử dụng ở mọi module. Dùng `IQueryable` ở layer Repository và áp dụng `.Skip().Take()` sau khi đếm tổng (`.CountAsync()`).

## 4. Open Questions (Câu hỏi mở để User xác nhận)
1. Có giới hạn số lượng bản ghi tối đa (MaxPageSize) trên 1 trang không? (Đề xuất: 100)
2. Đối với các API dùng để đổ dữ liệu vào Combobox/Dropdown (ví dụ: lấy danh sách Category để chọn khi tạo Product), chúng ta có áp dụng phân trang không, hay giữ nguyên GetAll lấy tất cả? (Đề xuất: Giữ nguyên lấy tất cả cho Dropdown để dễ code, chỉ phân trang màn hình danh sách chính).
