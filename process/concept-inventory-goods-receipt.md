# Concept: Tính năng Nhập Kho (Goods Receipt)

## 1. Nguồn tài liệu (SRS/HDSD)
Dựa trên tài liệu Hướng dẫn sử dụng Phân hệ Nhập kho (Inbound Inventory), các quy trình chính bao gồm:
- Tạo mới Phiếu Nhập Kho (Header).
- Chọn Loại phiếu (Purchase, Return, Transfer) và Tham chiếu (Mã PO).
- Kiểm đếm và cập nhật số lượng sản phẩm (Lines) qua thao tác quét mã vạch hoặc nhập thủ công.
- Hoàn tất và xác nhận (Posting): Chốt tồn kho, tính toán lại giá vốn, chuyển trạng thái "Completed", khóa phiếu.
- Đảm bảo tính toàn vẹn dữ liệu: Không xóa cứng (Hard Delete), chỉ điều chỉnh (Inventory Adjustment).

## 2. Phân tích & Phản biện (BA Review)
Sau khi đọc tài liệu thô, dưới đây là các câu hỏi (Open Questions) cần BA làm rõ trước khi chốt Spec (chuyển sang Phase 1.5):

### A. Thông tin chung (Header)
1. **Trường dữ liệu mở rộng**: Ngoài `Loại phiếu` và `Tham chiếu`, phiếu nhập kho có cần thêm các trường cơ bản như: `Ngày nhập` (Receipt Date), `Ghi chú` (Notes), `Nhà cung cấp` (Supplier), `Kho nhập` (Warehouse ID) không?
2. **Tham chiếu cho loại Return/Transfer**: Tài liệu nhắc tới chọn Mã Đơn mua hàng (PO) khi Loại phiếu là "Purchase". Vậy khi Loại phiếu là "Return" (Trả hàng) hoặc "Transfer" (Điều chuyển) thì có cần chọn Tham chiếu (như Sales Order, Transfer Order) không? Nếu có, hệ thống có tự động tải danh sách sản phẩm như đối với PO không?

### B. Quản lý Sản phẩm (Lines)
1. **Logic quét mã vạch**:
   - Nếu quét mã vạch Đơn vị quy đổi (VD: Thùng), hệ thống sẽ tăng số lượng tương ứng theo Thùng. Vậy Backend cần cung cấp một API phân giải mã vạch trả về thông tin Sản phẩm + Đơn vị tính (UoM) + Tỷ lệ quy đổi, đúng không?
   - Nếu phiếu nhập đã có sẵn danh sách sản phẩm từ PO, khi quét mã vạch, hệ thống sẽ: (1) Cập nhật số lượng đếm thực tế (Actual Quantity) của dòng sản phẩm tương ứng, hay (2) Tách ra một dòng mới? (Thông thường là tìm dòng trùng Sản phẩm + Đơn vị tính để cộng dồn).
2. **Kiểm tra vượt mức PO**: Nếu nhân viên quét mã vạch/nhập thủ công số lượng vượt quá số lượng đặt mua trên PO thì hệ thống có cho phép không (Báo lỗi chặn lại hay chỉ hiện cảnh báo)?

### C. Hoàn tất & Xác nhận (Posting)
1. **Phương pháp tính giá vốn**: Tài liệu ghi "tính toán lại giá vốn". Backend sử dụng phương pháp tính giá vốn nào (Ví dụ: Bình quân gia quyền - Moving Average Cost, hay FIFO)?
2. **Xử lý ghi nhận tồn kho**: Thao tác cập nhật tồn kho sẽ chạy đồng bộ (Synchronous) hay có sử dụng background task/event broker nào không? (Nếu dự án đang làm đơn giản thì sẽ chạy đồng bộ qua transaction).

## 3. Scope In / Scope Out (Dự kiến)
- **Scope In**:
  - Thêm, sửa (Draft), hiển thị Phiếu Nhập Kho.
  - Quản lý danh sách sản phẩm trong phiếu (nhập thủ công, quét mã vạch quy đổi đơn vị).
  - Hoàn tất (Posting) phiếu: Cập nhật tồn kho (Quantity on Hand / Available), tính giá vốn, đổi trạng thái phiếu.
- **Scope Out**:
  - Phiếu Điều chỉnh (Inventory Adjustment) (Sẽ nằm ở module khác để sửa lỗi sau khi hoàn tất).
  - Quản lý vị trí lưu kho chi tiết (Bin/Location Management) - *giả định đợt này chỉ nhập vào Kho (Warehouse) nói chung*.

---
**Đề xuất từ AI:**
Dựa trên best practices của hệ thống kho/bán lẻ, tôi xin đưa ra các đề xuất sau:

1. **Về Thông tin chung (Header):**
   - **Thêm các trường thiết yếu:** `ReceiptDate` (Ngày nhập thực tế), `WarehouseId` (Kho nhập), và `Notes` (Ghi chú). Trường `SupplierId` (Nhà cung cấp) nên được tự động lấy từ Tham chiếu (PO) và chỉ hiển thị (Read-only) để tránh sai lệch.
   - **Tham chiếu:**
     - Loại "Purchase" -> Tham chiếu tới `PurchaseOrder`.
     - Loại "Return" -> Tham chiếu tới `ReturnOrder` (hoặc Đơn bán hàng `SalesOrder` có trạng thái hoàn trả).
     - Loại "Transfer" -> Tham chiếu tới `TransferOrder`.

2. **Về Quản lý Sản phẩm (Lines):**
   - **Logic quét mã vạch:** Khi quét mã, Backend gọi API phân giải ra (ProductId, UoM, ConversionRate). Tại Frontend, hệ thống sẽ **tìm dòng có sẵn** trùng `ProductId` và `UoM` (Đơn vị tính) để **cộng dồn** vào `ActualQuantity`. Nếu chưa có dòng này trong danh sách tải từ PO, sẽ **tạo thêm một dòng mới** (cảnh báo là hàng nằm ngoài PO).
   - **Kiểm tra vượt mức:** Khi số lượng thực tế (ActualQuantity) lớn hơn số lượng dự kiến (ExpectedQuantity từ PO), hệ thống sẽ **không chặn (block)** mà chỉ **hiển thị cảnh báo (warning)** màu cam/đỏ trên UI. Lý do: Thực tế thủ kho vẫn phải ghi nhận đúng những gì nhập vào kho, việc xử lý lệch PO thuộc thẩm quyền của bộ phận Mua hàng.

3. **Về Hoàn tất & Xác nhận (Posting):**
   - **Tính giá vốn:** Đề xuất áp dụng phương pháp **Bình quân gia quyền** (Moving Average Cost) - phương pháp phổ biến và dễ triển khai nhất.
   - **Cập nhật tồn kho:** Đề xuất xử lý **đồng bộ (Synchronous)** bên trong một Database Transaction (gồm: Cập nhật trạng thái phiếu, Tính lại giá vốn, Cộng tồn kho). Điều này đảm bảo tính toàn vẹn dữ liệu (ACID) mà không làm phức tạp hệ thống với Message Queue ở giai đoạn này.

---
**Chờ xác nhận từ BA:**
Bạn có đồng ý với các đề xuất trên không? Nếu chốt phương án này, chúng ta sẽ chuyển sang **Phase 1.5 - Spec Extraction** để tạo file `spec-inventory-goods-receipt.md`.
