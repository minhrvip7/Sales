# Spec: Pagination toàn dự án

**Document ID:** SPEC-CORE-PAGINATION
**Status:** Ready for design
**Module:** Core / Common
**Feature:** Phân trang và Tải dữ liệu động

## 1. Mục tiêu
Bổ sung tính năng phân trang cho toàn bộ hệ thống (danh sách hiển thị bảng) và tải dữ liệu động (infinite scroll) cho các dropdown/combobox để cải thiện hiệu năng khi số lượng dữ liệu lớn.

## 2. Quy tắc chung
- **Mặc định:** Hiển thị 20 bản ghi trên 1 trang (`PageSize = 20`).
- **Tùy chỉnh:** Người dùng có thể thay đổi số lượng bản ghi hiển thị trên mỗi trang (ví dụ: 10, 20, 50, 100).
- **Giới hạn an toàn:** Kích thước trang tối đa (MaxPageSize) không được vượt quá 100 bản ghi để bảo vệ backend.

## 3. Phạm vi áp dụng (Backend)
- **Cấu trúc chung:**
  - `PagedRequest`: Các class request kế thừa hoặc sử dụng object chứa tham số `PageNumber` (int), `PageSize` (int).
  - `PagedResponse<T>`: Wrapper chứa `IEnumerable<T> Data`, `int TotalRecords`, `int TotalPages`, `int PageNumber`, `int PageSize`.
- **Domain áp dụng:** 
  - `Category`
  - `Order`
  - `Product`
  - `Unit`
- **Thay đổi logic Controller/Service/Repository:**
  - Nâng cấp hàm `GetAllAsync()` thành các hàm hỗ trợ truyền parameter phân trang (ví dụ: `GetPagedAsync(PagedRequest request)`).
  - Các hàm trả về danh sách phục vụ Dropdown cũng áp dụng phân trang, hỗ trợ tìm kiếm theo `Keyword`.

## 4. Phạm vi áp dụng (Frontend)
- **Danh sách (Table):**
  - Giao diện sử dụng component `Table` của Ant Design, tích hợp thanh phân trang ở dưới cùng.
  - Khi người dùng click chuyển trang hoặc thay đổi Page Size, thực hiện gọi lại API với tham số mới.
- **Dropdown/Combobox (Select):**
  - Không sử dụng kiểu load tất cả (Get All).
  - Áp dụng **Virtualized Select (Infinite Scroll)**: Mặc định load trang 1, khi scroll xuống dưới danh sách popup thì trigger fetch trang tiếp theo nối vào.
  - Hỗ trợ gõ text để tìm kiếm (Debounced search gọi API với tham số `Keyword`).

## 5. Các ràng buộc dữ liệu (Constraints)
- `PageNumber` >= 1. Nếu truyền <= 0, mặc định gán là 1.
- `PageSize` > 0 && <= 100. Nếu lớn hơn 100, tự động gán là 100. Nếu nhỏ hơn hoặc bằng 0, mặc định 20.
- Sắp xếp (Sorting): Tạm thời áp dụng sắp xếp mặc định theo ID hoặc Ngày tạo (giảm dần) ở backend để đảm bảo phân trang không bị lộn xộn.

## 6. Kế hoạch tiếp theo
- Chuyển sang **Phase 2 & 2b (Backend & Frontend Design)** để sinh ra cấu trúc mã chi tiết cho Backend (DTO, API endpoints) và Frontend (Service calls, Components).
