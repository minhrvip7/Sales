# Frontend Design: Tính năng Nhập Kho (Goods Receipt)

## 1. Routing
- `/inventory/goods-receipt`: Danh sách Phiếu Nhập Kho.
- `/inventory/goods-receipt/create`: Trang tạo Phiếu Nhập Kho mới.
- `/inventory/goods-receipt/:id/edit`: Trang chỉnh sửa (chỉ khả dụng khi phiếu ở trạng thái Draft).
- `/inventory/goods-receipt/:id`: Trang xem chi tiết (khi phiếu đã Completed).

## 2. API Mapping (Axios)
- `GET /api/goods-receipts`: Lấy danh sách (phân trang).
- `GET /api/goods-receipts/{id}`: Xem chi tiết.
- `POST /api/goods-receipts`: Tạo phiếu mới.
- `PUT /api/goods-receipts/{id}`: Cập nhật phiếu Draft.
- `POST /api/goods-receipts/{id}/complete`: Chốt phiếu (Posting).
- `GET /api/goods-receipts/resolve-barcode?barcode={barcode}`: Phân giải mã vạch thành sản phẩm.

## 3. UI Layout & Components (React + Ant Design)
- **Trang Danh sách (List Page):**
  - Search/Filter section: Theo Mã phiếu, Trạng thái, Loại phiếu.
  - `Table` hiển thị dữ liệu với pagination tiêu chuẩn. Nút "+ Tạo mới" ở góc phải.
- **Trang Tạo/Sửa (Form Page):**
  - **Layout:** Dùng hệ thống lưới `<Row>` và `<Col>` (như `xs={24}`, `sm={12}`, `md={6}`) để bố trí form responsive, KHÔNG dùng width cứng.
  - **Header Section (Form chính):** `Select` Loại phiếu, `Select` Tham chiếu PO/SO/TO, `DatePicker` Ngày nhập, `Select` Kho nhập. Các select box có lượng dữ liệu lớn phải áp dụng *Virtualized Select* (infinite scroll).
  - **Barcode Scan Section:** Một `<Input>` chuyên dụng dành cho thao tác quét mã vạch, có event `onPressEnter` gọi API.
  - **Lines Section (Table nhập liệu):**
    - Sử dụng Component Editable Table của Antd cho danh sách sản phẩm.
    - Cột `ExpectedQuantity` (ReadOnly).
    - Cột `ActualQuantity` (InputNumber để chỉnh sửa).
    - Logic cảnh báo: Nếu `row.ActualQuantity > row.ExpectedQuantity`, row đó sẽ được highlight (ví dụ text hoặc background chuyển cam/đỏ).
  - **Footer Actions:** Nút "Lưu Nháp" (Save Draft) và "Hoàn tất" (Post) với double-confirm popup.
